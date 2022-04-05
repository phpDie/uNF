using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum PhoneState
{
    none,
    calling,
    dialog,
}

[System.Serializable]
public struct PhonceDilog{
    public AudioClip voice;
    public string login;
    public bool isAutoOpen;
    [TextArea()]
    public string subtitles;
}

public class PhoneCall : MonoBehaviour
{
    public Project_DatabaseClass proj;

    public PhoneState state = PhoneState.none;


    public List<PhonceDilog> RandomDialogList = new List<PhonceDilog>();
    public PhonceDilog currentDialog;


    public BinUKey btnOpen;
    public BinUKey btnSkip;



   public  AudioSource audioSourceVoice;
     AudioSource audioSourceEffect;


    public AudioClip audioPhoneCall;
    public AudioClip audioPhoneCallEnd;
    public AudioClip audioPhoneSkip;


    public Text frameLabelLogin;
    public Image framePhone;
    public Image frameSubtitles;



    public MusicPlayer musicPlayer;

    int cursorSub;
    void SubtitlesShowPartNext( )
    {
        cursorSub += 1;

        float soundLien = currentDialog.voice.length;
        string[] explode = currentDialog.subtitles.Split(' ');
        //print("Len sub " + explode.Length.ToString());

        

        int wordsInTake = 26;
        int partCount = Mathf.CeilToInt(soundLien / wordsInTake);


        cursorSub = Mathf.Min(Mathf.CeilToInt(explode.Length / wordsInTake), cursorSub);
        



        int startText = Mathf.FloorToInt(cursorSub * wordsInTake - wordsInTake);
        int endText = Mathf.FloorToInt(cursorSub * wordsInTake);

        endText = Mathf.Min(endText, explode.Length-1);
        startText = Mathf.Min(startText, explode.Length-1 - wordsInTake);
        /*
        print("sub cursor: " + cursorSub.ToString());
        print("startText: " + startText.ToString());
        print("endText: " + endText.ToString());
        */
        string result = "";
        for (int i = startText; i < endText; i++)
        {
            result += explode[i];
        }

        frameSubtitles.rectTransform.LeanScale(new Vector3(1, 1, 1) * 1.4f, 0.4f).setEasePunch();
        frameSubtitles.transform.Find("_login").GetComponent<Text>().text = currentDialog.login;
        frameSubtitles.transform.Find("_text").GetComponent<Text>().text = result; 
    }

    // Start is called before the first frame update
    void Start()
    {

        btnOpen.Render();
        btnOpen.myCallbackActionEvent.AddListener(ClickAction);
        
    
            btnSkip.Render();
            btnSkip.myCallbackActionEvent.AddListener(ClickAction);

        btnSkip.gameObject.SetActive(false);
        btnOpen.gameObject.SetActive(false);
        framePhone.gameObject.SetActive(false);
        frameSubtitles.gameObject.SetActive(false);

        audioSourceEffect = gameObject.AddComponent<AudioSource>();
        
        audioSourceEffect.outputAudioMixerGroup = proj.audioMixer.FindMatchingGroups("Master")[4];
   
        if (!audioSourceVoice)
        {
            audioSourceVoice = gameObject.AddComponent<AudioSource>();
        }
        audioSourceEffect.outputAudioMixerGroup = proj.audioMixer.FindMatchingGroups("Master")[3];
    }



    void ClickAction(string action)
    {
        if (action == "open")
        {
            PhoneUp();
        }
        if (action == "skip")
        {
            PhoneSkip(true);
        }
    }


    void PhoneSkip(bool isAgressive = false)
    {
        if (state == PhoneState.none)
        {
            return;
        }

        timer = -1f;

        framePhone.gameObject.SetActive(false);
        frameSubtitles.gameObject.SetActive(false);
        btnSkip.gameObject.SetActive(false);
        btnOpen.gameObject.SetActive(false);

        framePhone.transform.LeanScale(new Vector3(1, 1, 1) * 1.4f, 0.4f).setEasePunch();

        proj.audioMixer.FindSnapshot("Normal").TransitionTo(5);

        if (state == PhoneState.calling)
        {
            audioSourceEffect.Stop(); 
            state = PhoneState.none;
        }

        if (state == PhoneState.dialog)
        {
            audioSourceVoice.Stop();
            audioSourceVoice.clip = null; 
            state = PhoneState.none;
        }

        if (isAgressive)
        {

            audioSourceEffect.PlayOneShot(audioPhoneSkip);
        }
        else
        {

            audioSourceEffect.PlayOneShot(audioPhoneCallEnd);
        }

      //  musicPlayer.VoulmeCofficent = 1f;
    }


    void PhoneUp()
    {
        if (state != PhoneState.calling) return;
        state = PhoneState.dialog;

        audioSourceEffect.Stop();
        audioSourceEffect.PlayOneShot(audioPhoneCallEnd);

        audioSourceVoice.clip = currentDialog.voice;
        audioSourceVoice.Play();

        frameSubtitles.gameObject.SetActive(true);
        btnSkip.gameObject.SetActive(true);
        btnOpen.gameObject.SetActive(false);

        cursorSub = 0;
        timerDialogSubt = 0f;
        SubtitlesShowPartNext();

       // framePhone.rectTransform.LeanScale(new Vector3(1, 1, 1) * 1.4f, 0.4f).setEasePunch();
        framePhone.transform.LeanScale(new Vector3(1, 1, 1) * 1.2f, 0.4f).setEasePunch();

        //proj.audioMixer.TransitionToSnapshots(proj.audioMixer.FindSnapshot("Dialog"), 
        proj.audioMixer.FindSnapshot("Dialog").TransitionTo(1.1f);
    }


    public void CreateCall(PhonceDilog _newDialog)
    {
        if(state != PhoneState.none)return;


        state = PhoneState.calling;

        currentDialog = _newDialog;

        frameLabelLogin.text = currentDialog.login;

        audioSourceEffect.clip = audioPhoneCall;
        audioSourceEffect.Play();


        framePhone.gameObject.SetActive(true);
        btnSkip.gameObject.SetActive(false);
        btnOpen.gameObject.SetActive(true);

        framePhone.transform.LeanScale(new Vector3(1, 1, 1) * 1.15f, 0.2f).setEasePunch();
    }



     
    float timerDialogSubt = 0f;
    float timer = 0f;
 
    private void FixedUpdate()
    {

        timer += Time.fixedDeltaTime;
        if (timer > 4f)
        {
            CreateCall(RandomDialogList[Random.Range(0, RandomDialogList.Count )]);
            timer = 0f;
        }



        if (state == PhoneState.dialog)
        {
            timerDialogSubt += Time.deltaTime;
            if (timerDialogSubt > 5f)
            {
                SubtitlesShowPartNext();
                timerDialogSubt = 0f;
            }
            if (!audioSourceVoice.isPlaying)
            {
                PhoneSkip();
            }
        } 


    }
}
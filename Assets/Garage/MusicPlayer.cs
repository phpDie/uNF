using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicPlayer : MonoBehaviour
{

    public Project_DatabaseClass proj;

    
   

    public List<AudioClip> trackList = new List<AudioClip>();

    public Image Frame;

    AudioSource audioSource;

     AudioClip GetRandTrack()
    {
        return trackList[Random.Range(0, trackList.Count - 1)];
    }
     

    public void Next()
    {
        
       // print("next");

        audioSource.clip = GetRandTrack();
        audioSource.Play();
        ShowName(true);
        //showArtist(nextTrack);
        //_backGround.rectTransform.LeanMoveLocalX(0, 0.2f).setEaseInElastic();
        StartCoroutine(HideTrackName());
    }
     


    void Start()
    { 
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = proj.audioMixer.FindMatchingGroups("Master")[1];
         
        Next();
    }

    void showArtist(AudioClip _clip)
    {
        if (!_clip)
        {
            print("[mp3] Error clip!!");
            return;
        }
        string[] tokens = _clip.name.Split('-');

        Frame.transform.Find("_login").GetComponent<Text>().text = tokens[0];
        Frame.transform.Find("_track").GetComponent<Text>().text = tokens[1]; 
    }

    void ShowName(bool val)
    { 
        if (val)
        { 
            Frame.gameObject.SetActive(true);
            showArtist(audioSource.clip);
        }
        else
        {
            Frame.gameObject.SetActive(false);
        } 
    }


  

    float currCountdownValue;
    public IEnumerator HideTrackName()
    {
        currCountdownValue = 4;
        while (currCountdownValue > 0)
        {
          //  Debug.Log("Countdown: " + currCountdownValue);
            currCountdownValue--;
            if (currCountdownValue == 1) ShowName(false);
            yield return new WaitForSeconds(1.0f); 
            
        }
    }

    bool canCaledEnded = true;
    void Update()
    {
        

        if (canCaledEnded)
        {
            if (audioSource.isPlaying)
            { 
                if (audioSource.time > audioSource.clip.length - 1.5f) 
                {  
                    Next();
                    return;
                }
            }
            if (!audioSource.isPlaying && (audioSource.time == 0f))
            { 
                Next();
                return;
            }

        }
    }
}

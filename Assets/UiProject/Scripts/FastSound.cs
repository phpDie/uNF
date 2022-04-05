using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ClipSoundStruct{
    public string ind;
    public AudioClip audioClip;

    [HideInInspector]
    public AudioSource audioSource;
}


public class FastSound : MonoBehaviour
{
    [SerializeField]
    public List<ClipSoundStruct> listClip = new List<ClipSoundStruct>();
    public List<AudioSource> listSource = new List<AudioSource>();


    public void Play(string ind)
    {
        init();

        if (!listClip.Exists(item => item.ind == ind))
        {
            print("[FastSound] Error play in  " + gameObject.name + " Sound ind: " + ind);
            return;
        }

        int J = listClip.FindIndex(item => item.ind == ind);
        ClipSoundStruct data = listClip[J];
        listSource[J].PlayOneShot(data.audioClip);
    }

    bool isInit = false;
    public void init()
    {

        if (isInit) return;
        isInit = true;

        for (int i = 0; i < listClip.Count; i++)
        {
            AudioSource _audioSource = gameObject.AddComponent<AudioSource>();
            _audioSource.volume = 0.8f;
            listSource.Add(_audioSource);
        }
    }


    void Start()
    {
        init();
    }

}
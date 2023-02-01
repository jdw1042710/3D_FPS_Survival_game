using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
struct Sound
{
    public string name;
    public AudioClip audioClip;
}

public class SoundManager : MonoBehaviour
{
    private enum SoundType
    {
        Bgm,
        Effect,
        MaxCount,
    };
    
    public static SoundManager instance;

    private AudioSource[] audioSources = new AudioSource[(int)SoundType.MaxCount];
    [SerializeField]
    private Sound[] effects;
    [SerializeField]
    private Sound[] bgms;
    
    [SerializeField]
    private Dictionary<String, AudioClip> effectSounds = new Dictionary<string, AudioClip>();
    [SerializeField]
    private Dictionary<String, AudioClip> bgmSounds  = new Dictionary<string, AudioClip>();
    

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Init();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Init()
    {
        for (int i = 0; i < effects.Length; i++)
        {
            effectSounds.Add(effects[i].name, effects[i].audioClip);
        }


        for (int i = 0; i < bgms.Length; i++)
        {
            bgmSounds.Add(bgms[i].name, bgms[i].audioClip);
        }
        for (int i = 0; i < (int)SoundType.MaxCount; i++)
        {
            audioSources[i] = this.AddComponent<AudioSource>();
        }
        audioSources[(int)SoundType.Bgm].loop = true;
    }


    public void PlaySE(string _name)
    {
        AudioClip _clip;
        if (effectSounds.TryGetValue(_name, out _clip))
        {
            audioSources[(int)SoundType.Effect].clip = _clip;
            audioSources[(int)SoundType.Effect].Play();
        }
        else
        {
            Debug.Log(_name + "소리를 찾지 못했습니다.");
        }
    }

    public void StopSE()
    {
        audioSources[(int)SoundType.Effect].Stop();
    }
    
    public void PlayBgm(string _name)
    {
        AudioClip _clip;
        if (effectSounds.TryGetValue(_name, out _clip))
        {
            audioSources[(int)SoundType.Bgm].clip = _clip;
            audioSources[(int)SoundType.Bgm].Play();
        }
        Debug.Log(_name + "소리를 찾지 못했습니다.");
    }
    
    public void StopBgm()
    {
        audioSources[(int)SoundType.Bgm].Stop();
    }
    
}

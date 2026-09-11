using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using UnityEngine.Audio;


public enum MusicType
{
    MainMenu,
    LevelMusic,
    Shopmusic,
    Bossmusic,

    
}

[RequireComponent(typeof(AudioSource)), ExecuteInEditMode]
public class MusicManager : MonoBehaviour
{
    [SerializeField] private musicList[] musicList;
    private static MusicManager instance;
    private AudioSource audioSource;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public static void PlaySound(SoundType sound, float volume = 1)
    {
        AudioClip[] clips = instance.musicList[(int)sound].Sounds;
        AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];
        instance.audioSource.PlayOneShot(randomClip, volume);
    }

#if UNITY_EDITOR
    private void OnEnable()
    {
        string[] names = Enum.GetNames(typeof(SoundType));
        Array.Resize(ref musicList, names.Length);
        for (int i = 0; i < musicList.Length; i++)
        {
            musicList[i].name = names[i];
        }
    }
#endif

}

[Serializable]
public struct musicList
{
    public AudioClip[] Sounds { get => sounds; }
    [HideInInspector] public string name;
    [SerializeField] private AudioClip[] sounds;
}

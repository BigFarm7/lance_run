using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class AudioManager : MonoBehaviour
{
    public MenuSettingsUI menuSettingsUI;
    public static AudioManager instance;
    [Header("#BGM")]
    public AudioClip[] bgmClip;
    public float bgmVolume;
    public AudioSource[] bgmPlayers;

    [Header("#SFX")]
    public AudioClip[] sfxClips;
    public float sfxVolume;
    public int channels;
    public AudioSource[] sfxPlayers;
    int channelIndex;

    private void Awake()
    {
        instance = this;
        Init();
        
    }

    void Init()
    {
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayers = new AudioSource[2];

        for (int index = 0; index < bgmPlayers.Length; index++)
        {
            bgmPlayers[index] = bgmObject.AddComponent<AudioSource>();
            bgmPlayers[index].playOnAwake = false;
            bgmPlayers[index].loop = true;
            bgmPlayers[index].volume = bgmVolume;
            bgmPlayers[index].clip = bgmClip[index];
        }
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];

        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            sfxPlayers[index] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[index].playOnAwake = false;
            sfxPlayers[index].volume = sfxVolume;
        }
        PlayBgm1(true);
    }
    public void Turnoffslow()
    {
        bgmPlayers[0].DOFade(0, 1f).OnKill(() =>
        {
            bgmPlayers[0].Stop();
        });
    }
   

    public void PlayBgm1(bool isPlay)
    {
       
       
        if (isPlay) {
            bgmPlayers[0].volume = 0.75f;
            bgmPlayers[0].Play();
        }
        else
        {
            bgmPlayers[0].Stop();
        }
    }

    public void PlayBgm2(bool isPlay)
    {

        if (isPlay)
        {
            bgmPlayers[1].Play();
        }
        else
        {
            bgmPlayers[1].Stop();
        }
    }
    public void PlaySfx(int i)
    {
        for (int index = 0; index < sfxPlayers.Length; index++) {
            int loopIndex = (index + channelIndex) % sfxPlayers.Length;

            if (sfxPlayers[loopIndex].isPlaying)
                continue;

            channelIndex = loopIndex;

            sfxPlayers[loopIndex].clip = sfxClips[i];
            sfxPlayers[loopIndex].Play();
            break;

        }

       
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio_control : MonoBehaviour
{

    [Header("按钮声音")]
    public AudioClip audio_clip_btn;

    [Header("喝彩的声音")]
    public AudioClip audio_clip_applaud;

    [Header("模型出场音效")]
    public AudioClip audio_clip_show;

    [Header("钢琴按键声音")]
    public AudioClip[] piano_sound;

    //单列模式
    public static Audio_control instance;

    //音源
    private AudioSource audio_source;

    void Awake()
    {
        //单列模式
        Audio_control.instance = this;
    }

    // Use this for initialization
    void Start()
    {
        //查找音源
        this.audio_source = this.GetComponent<AudioSource>();
    }

    //播放按钮声音
    public void play_btn_sound()
    {
        audio_source.PlayOneShot(this.audio_clip_btn, 1f);
    }

    //播放钢琴按键声音
    public void play_piano_sound(int num)
    {
        audio_source.PlayOneShot(this.piano_sound[num]);
    }

    //播放成功弹奏后 喝彩的声音
    public void play_applaud_sound()
    {
        audio_source.PlayOneShot(this.audio_clip_applaud);
    }

    //播放出场音效
    public void play_show_sound()
    {
        audio_source.PlayOneShot(this.audio_clip_show, 1.5f);
    }
}

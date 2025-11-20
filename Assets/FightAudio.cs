using System;
using UnityEngine;

public class FightAudio : MonoBehaviour
{
    public static FightAudio instance { get; private set; }
    public AudioSource audioSource;
    public AudioClip fightMusic;

    public AudioSource SwordAudio1;
    public AudioSource SwordAudio2;
    public AudioSource DaggarAudio1;
    public AudioSource DaggarAudio2;
    public AudioSource MaceAudio1;
    public AudioSource MaceAudio2;
    public AudioSource BowAudio1;
    public AudioSource BowAudio2;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = fightMusic;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void PlayBow1()
    {
        BowAudio1.Play();
    }
    public void PlayBow2()
    {
        BowAudio2.Play();
    }
    public void PlaySword1()
    {
        SwordAudio1.Play();
    }
    public void PlaySword2()
    {
        SwordAudio2.Play();
    }
    public void PlayMace1()
    {
        MaceAudio1.Play();
    }
    public void PlayMace2()
    {
        MaceAudio2.Play();
    }
    public void PlayDaggar1()
    {
        DaggarAudio1.Play();   
    }
    public void PlayDaggar2()
    {
        DaggarAudio2.Play();
    }
}

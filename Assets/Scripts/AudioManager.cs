using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Start is called before the first frame update

    public static AudioClip jumpSound, shootSound, landingSound, powerUpSound, backgroundSound, hitSound, hurtSound, batDieSound, knightRunSound, dashSound, knightDieSound;
    static AudioSource audioSrc;
    void Start()
    {
        jumpSound = Resources.Load<AudioClip>("Jump");
        shootSound = Resources.Load<AudioClip>("Shoot");
        landingSound = Resources.Load<AudioClip>("Landing");
        powerUpSound = Resources.Load<AudioClip>("Power Up");
        backgroundSound = Resources.Load<AudioClip>("Background Music");
        hitSound = Resources.Load<AudioClip>("Hit");
        hurtSound = Resources.Load<AudioClip>("Hurt");
        batDieSound = Resources.Load<AudioClip>("BatDie");
        knightRunSound = Resources.Load<AudioClip>("KnightRun");
        knightDieSound = Resources.Load<AudioClip>("KnightDie");
        dashSound = Resources.Load<AudioClip>("Dash");

        audioSrc = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void PlaySound(string clip) 
    {
        switch (clip) {
            case "Jump":
                audioSrc.PlayOneShot(jumpSound);
                break;
            case "Shoot":
                audioSrc.PlayOneShot(shootSound);
                break;
            case "PowerUp":
                audioSrc.PlayOneShot(powerUpSound);
                break;
            case "Landing":
                audioSrc.PlayOneShot(landingSound);
                break;
            case "Hit":
                audioSrc.PlayOneShot(hitSound);
                break;
            case "Hurt":
                audioSrc.PlayOneShot(hurtSound);
                break;
            case "BatDie":
                audioSrc.PlayOneShot(batDieSound);
                break;
            case "KnightRun":
                audioSrc.PlayOneShot(knightRunSound);
                break;
            case "Dash":
                audioSrc.PlayOneShot(dashSound);
                break;
            case "KnightDie":
                audioSrc.PlayOneShot(knightDieSound);
                break;
        }

    }

    public static void StopSound(string clip)
    {
        switch (clip)
        {
            case "Jump":
                audioSrc.Stop();
                break;
        }

    }
}


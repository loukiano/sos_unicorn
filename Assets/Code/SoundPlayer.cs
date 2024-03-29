using UnityEngine;
using System.Collections;
using System;

public class SoundPlayer : MonoBehaviour
{
    static public SoundPlayer s;

    public enum Cue { start, pause, unpause, stop};

    public enum Sounds
    {
        background,
        dash,
        shoot,
        jump,
        stompFall,
        playerDeath,
        playerDmg,
        enemyDeath
    }

    public AudioSource backgroundMusic;
    public float initialBackgroundVol;
    private bool backgroundPaused;

    // PLAYER ACTIONS
    public AudioSource dash;
    public AudioSource shoot;
    public AudioSource jump;
    public AudioSource stompFall;
    public AudioSource playerDeath;
    public AudioSource playerDmg;
    public RandomSound randDmgClip;

    // ENEMY ACTIONS
    public AudioSource enemyDeath;
    public float enemyDeathSoundCooldown;
    private float lastEnemyDeathSound;

    // Use this for initialization
    void Start()
    {
        s = this;
        initialBackgroundVol = backgroundMusic.volume;
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public static void PlaySound(Sounds soundName, float vol = 1)
    {
        s.RouteSoundCue(Cue.start, soundName, vol);
    }

    public static void PauseSound(Sounds soundName, float vol = 1)
    {
        s.RouteSoundCue(Cue.pause, soundName, vol);
    }

    public static void UnPauseSound(Sounds soundName, float vol = 1)
    {
        s.RouteSoundCue(Cue.unpause, soundName, vol);
    }

    public static void StopSound(Sounds soundName, float vol = 1)
    {
        s.RouteSoundCue(Cue.stop, soundName, vol);
    }

    public static void HushBackground(float newVol)
    {
        s.ChangeBackgroundVol(newVol);
    }

    public static void RestoreBackground()
    {
        s.ChangeBackgroundVol(s.initialBackgroundVol);
    }

    public void ChangeBackgroundVol(float newVol)
    {
        backgroundMusic.volume = newVol;
    }


    public void RouteSoundCue(Cue cue, Sounds soundName, float vol)
    {
        switch (soundName)
        {
            case Sounds.background:
                DoSoundCue(cue, backgroundMusic, vol);
                break;
            case Sounds.dash:
                DoSoundCue(cue, dash, vol);
                break;
            case Sounds.shoot:
                DoSoundCue(cue, shoot, vol);
                break;
            case Sounds.jump:
                DoSoundCue(cue, jump, vol);
                break;
            case Sounds.stompFall:
                DoSoundCue(cue, stompFall, vol);
                break;
            case Sounds.playerDeath:
                //Debug.Log("Player died!");
                DoSoundCue(cue, playerDeath, vol);
                break;
            case Sounds.playerDmg:
                playerDmg.clip = randDmgClip.GetRandomClip();
                //Debug.Log("OUCH! Clip: " + playerDmg.clip.name);
                DoSoundCue(cue, playerDmg, vol);
                break;
            case Sounds.enemyDeath:
                if (Time.time > lastEnemyDeathSound + enemyDeathSoundCooldown)
                {
                    DoSoundCue(cue, enemyDeath, vol);
                    lastEnemyDeathSound = Time.time;
                }
                break;

            default:
                Debug.Log("WARNING: Unrecognized sound name: " + soundName);
                break;


        }
    }


    public void DoSoundCue(Cue cue, AudioSource sound, float vol)
    {
        switch (cue)
        {
            case Cue.start:
                sound.PlayOneShot(sound.clip, vol);
                break;
            case Cue.pause:
                sound.Pause();
                break;
            case Cue.unpause:
                sound.UnPause();
                break;
            case Cue.stop:
                sound.Stop();
                break;
            default:
                Debug.Log("WARNING: Unrecognized cue type: " + cue + "for sound: " + sound);
                break;

        }
    }
    

}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    public static GameAssets instance;
    public AudioSource audioSource;
    private float volume = 5f;

    public static GameAssets getInstance() 
    {
        return instance;
    }

    private void Awake()
    {
        instance = this;
    }
    public Transform pfPipa;
    public Transform pfRabiola;
    public Transform pfCerol;

    public Transform pfBlockHead;
    public Transform pfBlockBody;
    public Transform pfGround;
    public Transform pfBackground;

    public Transform pfBlockHead2;
    public Transform pfBlockBody2;
    public Transform pfGround2;
    public Transform pfBackground2;

    public Transform pfBlockHeadWax;
    public Transform pfBlockBodyWax;
    public Transform pfGroundWax;
    public Transform pfBackgroundWax;

    public AudioClip start;
    public AudioClip death;


    public void playStart()
    {
        audioSource.PlayOneShot(start, volume);

    }

    public void playDeath()
    {
        audioSource.PlayOneShot(death, volume);

    }
}

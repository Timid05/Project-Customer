using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [Header("Audio Clips")]

    [SerializeField]
     private AudioClip ambience;

     [SerializeField]
     private AudioClip tick;


     [Header("organ refrences")]

    [SerializeField]
    private OrganGrabManager organGrab;

    [SerializeField]
     private AudioClip organ;

     [SerializeField]
     private AudioClip organ2;

     [SerializeField]
     private AudioClip organ3;

    [Header("Player Refrences")]

    [SerializeField]
     private AudioClip walking;
     
    [SerializeField]
    private PlayerMovement playerMovement;

    [SerializeField]
    AudioSource audioSourceplayer;



public void OrganNoise()
{
    int randomNumber = GenerateRandomInt(1, 3);

     switch (randomNumber)
    {
         case 1:
        audioSourceplayer.clip = organ;
        break;
        case 2:
        audioSourceplayer.clip = organ2;
        break;
        case 3:
        audioSourceplayer.clip = organ3;
        break;
        default:
        Debug.Log("Unexpected value");
         return;
            }
            audioSourceplayer.loop = false;
            audioSourceplayer.Play();
         
        
    
   
}
private bool isWalkingNoisePlaying = false;



public void WalkingNoise(bool play)
{
    if (play)
    {
        audioSourceplayer.loop = true;
        if (!isWalkingNoisePlaying)
        {
            audioSourceplayer.clip = walking;
            audioSourceplayer.Play();
            isWalkingNoisePlaying = true;
        }
    }
    else
    {
        if (isWalkingNoisePlaying)
        {
            audioSourceplayer.Stop();  // Stop playing when not triggered
            isWalkingNoisePlaying = false;
        }
    }
}



    private int GenerateRandomInt(int min, int max)
    {
        
        return Random.Range(min, max + 1);
    }
}

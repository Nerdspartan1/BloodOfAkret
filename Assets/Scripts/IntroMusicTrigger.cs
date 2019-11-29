using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroMusicTrigger : MonoBehaviour
{
    public GameObject Player;

    public FMOD.Studio.EventInstance intromusicEvent;


    public void OntriggerEnter(Collider other)
    {
        if (other.gameObject == Player)
        {
            intromusicEvent = FMODUnity.RuntimeManager.CreateInstance(SoundManager.sm.intro);
            intromusicEvent.start();
        }
    }



}

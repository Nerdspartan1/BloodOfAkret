using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundAnimPlayer : MonoBehaviour
{
    public GameObject Skeleton;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void PlaySkelWarrMov()
    {
        //FMODUnity.RuntimeManager.PlayOneShot(SoundManager.sm.skelwarrmov);
        FMODUnity.RuntimeManager.PlayOneShotAttached(SoundManager.sm.skelwarrmov, Skeleton);
    }

}

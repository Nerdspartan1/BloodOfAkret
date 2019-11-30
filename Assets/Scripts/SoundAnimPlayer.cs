using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundAnimPlayer : MonoBehaviour
{
    private bool _isMummy = false;
    //private GameObject FirePlayer;

    public static SoundAnimPlayer sap;

    FMOD.Studio.EventInstance planeflamesEvent;

    void Awake()
    {

    }
    private void Start()
    {
        if (GetComponent<EnemyCharger>()) _isMummy = true;   
    }

    void Update()
    {
        
    }
    void PlayPlaneIntro()
    {
        FMODUnity.RuntimeManager.PlayOneShot(SoundManager.sm.plane);
    }
    void PlayPlaneFlames()
    {
        //planeflamesEvent = FMODUnity.RuntimeManager.CreateInstance(SoundManager.sm.planeflames);
        //planeflamesEvent.start();
    }
    void StopPlaneFlames()
    {
        //FirePlayer = GameObject.Find("FirePlayer");
        //Object.Destroy(FirePlayer);
        //planeflamesEvent.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        //laneflamesEvent.release();
    }
    //Skeleton and Mummy Warriors
    void PlaySkelWarrMov()
    {
        if (_isMummy)
        {
            FMODUnity.RuntimeManager.PlayOneShotAttached(SoundManager.sm.mummywarrmov, this.gameObject);
        }
        else
        {
            FMODUnity.RuntimeManager.PlayOneShotAttached(SoundManager.sm.skelwarrmov, this.gameObject);
        }
    }
    void PlaySkelWarrAttack()
    {
        if (_isMummy)
        {
            FMODUnity.RuntimeManager.PlayOneShotAttached(SoundManager.sm.mummywarrattack, this.gameObject);
        }
        else
        {
            FMODUnity.RuntimeManager.PlayOneShotAttached(SoundManager.sm.skelwarrattack, this.gameObject);
        }
    }

    void PlaySkelWarrVoice()
    {
        if (_isMummy)
        {
            FMODUnity.RuntimeManager.PlayOneShotAttached(SoundManager.sm.mummywarrvoice, this.gameObject);
        }
        else
        {
            FMODUnity.RuntimeManager.PlayOneShotAttached(SoundManager.sm.skelwarrvoice, this.gameObject);
        }
    }

    //Skeleton Mages
    void PlaySkelMageMov()
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached(SoundManager.sm.skelmagemov, this.gameObject);
    }
    void PlaySkelMageAttack()
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached(SoundManager.sm.skelmageattack, this.gameObject);
    }
    void PlaySkelMageVoice()
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached(SoundManager.sm.skelmagevoice, this.gameObject);
    }
    void PlayMummyCharge()
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached(SoundManager.sm.mummycharge, this.gameObject);
    }
    //Golems
    
    void PlayGolemMov1()
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached(SoundManager.sm.golemmov1, this.gameObject);
    }
    void PlayGolemMov2()
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached(SoundManager.sm.golemmov2, this.gameObject);
    }
    void PlayGolemAttack1()
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached(SoundManager.sm.golemattack1, this.gameObject);
    }
    
    //Boss
    void PlayBossUlt()
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached(SoundManager.sm.bossult, this.gameObject);
    }

}

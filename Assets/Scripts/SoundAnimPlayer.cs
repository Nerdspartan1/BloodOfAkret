using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundAnimPlayer : MonoBehaviour
{
    private bool _isMummy = false;

    private void Start()
    {
        if (GetComponent<EnemyCharger>()) _isMummy = true;
    }

    void Update()
    {
        
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
    //Skeleton Mages
    void PlaySkelMageMov()
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached(SoundManager.sm.skelmagemov, this.gameObject);
    }
    void PlaySkelMageAttack()
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached(SoundManager.sm.skelmageattack, this.gameObject);
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
}

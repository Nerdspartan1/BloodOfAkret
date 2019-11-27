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

}

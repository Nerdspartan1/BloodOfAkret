using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class SoundManager : MonoBehaviour
{
    public static SoundManager sM;

    [Header("Player Movement")]
    [FMODUnity.EventRef]
    public string footsteps;
    [FMODUnity.EventRef]
    public string jump;
    
    void Awake()
    {
        if (sM != null)
        {
            Destroy(this);
        }
        sM = this;
    }



    void Start()
    {
        
    }

    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Player Movement")]
    [FMODUnity.EventRef]
    public string footsteps;
    [FMODUnity.EventRef]
    public string jump;

    [Header("Ambience")]
    [FMODUnity.EventRef]
    public string sandstorm;
    [FMODUnity.EventRef]
    public string flames;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        Instance = this;
    }



    void Start()
    {
        
    }

    void Update()
    {
        
    }
}

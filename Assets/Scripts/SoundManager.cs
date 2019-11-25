using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class SoundManager : MonoBehaviour
{
    public static SoundManager sm;

    [Header("Music")]
    [FMODUnity.EventRef]
    public string plane;
    [FMODUnity.EventRef]
    public string menu;
    [FMODUnity.EventRef]
    public string music;

    [Header("Ambience")]
    [FMODUnity.EventRef]
    public string sandstorm;
    [FMODUnity.EventRef]
    public string flames;

    [Header("Player Movement")]
    [FMODUnity.EventRef]
    public string footsteps;
    [FMODUnity.EventRef]
    public string jump;

    [Header("Enemy Skeletons")]
    [FMODUnity.EventRef]
    public string skelwarrmov;
    [FMODUnity.EventRef]
    public string skelwarrattack;
    [FMODUnity.EventRef]
    public string skelwarrvoice;
    [FMODUnity.EventRef]
    public string skelsuicmov;
    [FMODUnity.EventRef]
    public string skelsuicattack;
    [FMODUnity.EventRef]
    public string skelsuicvoice;
    [FMODUnity.EventRef]
    public string skelmagemov;
    [FMODUnity.EventRef]
    public string skelmageattack;
    [FMODUnity.EventRef]
    public string skelmagevoice;

    [Header("Enemy Mummy")]
    [FMODUnity.EventRef]
    public string mummywarrmov;
    [FMODUnity.EventRef]
    public string mummywarrattack;
    [FMODUnity.EventRef]
    public string mummywarrvoice;

    [Header("Enemy Golem")]
    [FMODUnity.EventRef]
    public string golemmov;
    [FMODUnity.EventRef]
    public string golemattack1;
    [FMODUnity.EventRef]
    public string golemattack2;
    [FMODUnity.EventRef]
    public string golemvoice;




    void Awake()
    {
        if (sm != null)
        {
            Destroy(this);
        }
        sm = this;
    }



    void Start()
    {
        
    }

    void Update()
    {
        
    }
}

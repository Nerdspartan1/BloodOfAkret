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
    public string intro;
    [FMODUnity.EventRef]
    public string music;

    [Header("Ambience")]
    [FMODUnity.EventRef]
    public string sandstorm;
    [FMODUnity.EventRef]
    public string flames;
    [FMODUnity.EventRef]
    public string planeflames;

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
    public string skelmagefireballtravel;
    [FMODUnity.EventRef]
    public string fireballimpact;
    [FMODUnity.EventRef]
    public string skelmagevoice;
    [FMODUnity.EventRef]
    public string skelcommondeath;

    [Header("Enemy Mummy")]
    [FMODUnity.EventRef]
    public string mummywarrmov;
    [FMODUnity.EventRef]
    public string mummywarrattack;
    [FMODUnity.EventRef]
    public string mummycharge;
    [FMODUnity.EventRef]
    public string mummywarrvoice;
    [FMODUnity.EventRef]
    public string mummydeath;

    [Header("Enemy Golem")]
    [FMODUnity.EventRef]
    public string golemmov1;
    [FMODUnity.EventRef]
    public string golemmov2;
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

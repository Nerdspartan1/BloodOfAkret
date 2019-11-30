using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;

	public GameObject Player;
	public GameObject IntroSpawnLocation;
	public GameObject GameSpawnLocation;

	public GameObject Menu;
	public GameObject Intro;
	public GameObject Game;
	public GameObject PauseMenu;

	public GameObject TitleScreen;
	public GameObject ControlsScreen;
	public GameObject CreditsScreen;

	public float MouseSensitivity;
	public Slider SensitivitySlider;

	public GameObject SkipIntroButton;

	private bool _isPaused;
	private bool _mouseLocked = true;
	private bool _mouseLockedInGame;

	public bool CanPause = false;

    public FMOD.Studio.EventInstance menuEvent;
    FMOD.Studio.Bus MasterBus;

	private void Awake()
	{
		Instance = this;
	}

	void Start()
    {
		Menu.SetActive(true);
		ControlsScreen.SetActive(false);
		CreditsScreen.SetActive(false);
		TitleScreen.SetActive(true);

		bool canSkip = PlayerPrefs.GetInt("canSkip", 0) == 1;
		if (!canSkip) SkipIntroButton.SetActive(false);

		menuEvent = FMODUnity.RuntimeManager.CreateInstance(SoundManager.sm.menu);
        menuEvent.start();

        MasterBus = FMODUnity.RuntimeManager.GetBus("Bus:/");


        //STOP AND RELEASE IN GAME MUSIC!!! CHANGE DEATH PARAM CHANGE WAVE PARAM

		Player.SetActive(false);
		Intro.SetActive(false);
		Game.SetActive(false);
		PauseMenu.SetActive(false);
		RenderSettings.fogDensity = 0.002f;

		SensitivitySlider.value = PlayerPrefs.GetFloat("sensitivity", 5f);
		
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape) && CanPause)
		{
			if (!_isPaused) Pause();
			else Unpause();
		}
	}

	public void StartIntro()
	{
		Menu.SetActive(false);
		Player.transform.position = IntroSpawnLocation.transform.position;
		Intro.SetActive(true);
		Player.SetActive(true);
		Intro.GetComponent<DesertWandering>().PlayIntro();
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		RenderSettings.fogDensity = 0.08f;

        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI click");
        menuEvent.setParameterByName("Game Start", 1f);
    }

	public void StartGame()
	{
		Menu.SetActive(false);
		Player.transform.position = GameSpawnLocation.transform.position;
		Player.SetActive(true);
		Game.SetActive(true);
		
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		CanPause = true;
		
		Game.GetComponent<WaveManager>().StartGame();

        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI click");

        //menuEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        //menuEvent.release();
        menuEvent.setParameterByName("Game Start", 1f);
    }

	public void ResetGame()
	{
		Time.timeScale = 1.0f;
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		

		WaveManager.Instance.ingamemusicEvent.setParameterByName("Wave Prog", 0f);
        WaveManager.Instance.ingamemusicEvent.setParameterByName("Death", 0);
        WaveManager.Instance.ingamemusicEvent.setParameterByName("Boss Wave", 0);
        WaveManager.Instance.ingamemusicEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        WaveManager.Instance.ingamemusicEvent.release();

        MasterBus.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

        //menuEvent.setParameterByName("Game Start", 0f);
        //menuEvent.start();
    }

	public void OpenControls()
	{
		ControlsScreen.SetActive(true);
		TitleScreen.SetActive(false);
        
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI click");
    }

	public void OpenCredits()
	{
		CreditsScreen.SetActive(true);
		TitleScreen.SetActive(false);

        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI click");
    }


	public void BackToTitleScreen()
	{
		ControlsScreen.SetActive(false);
		CreditsScreen.SetActive(false);
		TitleScreen.SetActive(true);

        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI click");
    }

	public void Quit()
	{
		Application.Quit();
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI click");
    }

	public void Pause()
	{
		_isPaused = true;
		Time.timeScale = 0.0f;
		PauseMenu.SetActive(true);
		_mouseLockedInGame = _mouseLocked;
		FreeMouse();


        WaveManager.Instance.ingamemusicEvent.setPaused(true);
	}

	public void Unpause()
	{
		_isPaused = false;
		Time.timeScale = 1.0f;
		PauseMenu.SetActive(false);
		if (_mouseLockedInGame) LockMouse();

        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI click");
        WaveManager.Instance.ingamemusicEvent.setPaused(false);
    }

	public void FreeMouse()
	{
		_mouseLocked = false;
		Player.GetComponent<vp_FPInput>().MouseCursorForced = true;
		Player.GetComponent<vp_FPWeaponHandler>().enabled = false;
		Cursor.visible = true;
	}

	public void LockMouse()
	{
		_mouseLocked = true;
		Player.GetComponent<vp_FPInput>().MouseCursorForced = false;
		Player.GetComponent<vp_FPWeaponHandler>().enabled = true;
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

}

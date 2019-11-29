using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

	private bool _isPaused;
	private bool _mouseLocked = true;
	private bool _mouseLockedInGame;

	public bool CanPause = false;

    FMOD.Studio.EventInstance menuEvent;

	private void Awake()
	{
		Instance = this;
	}

	void Start()
    {
		Menu.SetActive(true);
		BackToTitleScreen();

        menuEvent = FMODUnity.RuntimeManager.CreateInstance(SoundManager.sm.menu);
        menuEvent.start();

        //STOP AND RELEASE IN GAME MUSIC!!!

		Player.SetActive(false);
		Intro.SetActive(false);
		Game.SetActive(false);
		PauseMenu.SetActive(false);
		RenderSettings.fogDensity = 0.002f;
    }

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.U) && CanPause)
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
		RenderSettings.fogDensity = 0.08f;
       
        menuEvent.setParameterByName("Game Start", 1f);
    }

	public void StartGame()
	{
		Menu.SetActive(false);
		Player.transform.position = GameSpawnLocation.transform.position;
		Game.SetActive(true);
		Player.SetActive(true);
		Cursor.lockState = CursorLockMode.Locked;
		CanPause = true;
		RenderSettings.fogDensity = 0.08f;
		Game.GetComponent<WaveManager>().StartGame();
        
        menuEvent.setParameterByName("Game Start", 1f);
    }

	public void ResetGame()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void OpenControls()
	{
		ControlsScreen.SetActive(true);
		TitleScreen.SetActive(false);
	}

	public void OpenCredits()
	{
		CreditsScreen.SetActive(true);
		TitleScreen.SetActive(false);
	}


	public void BackToTitleScreen()
	{
		ControlsScreen.SetActive(false);
		CreditsScreen.SetActive(false);
		TitleScreen.SetActive(true);

        menuEvent.start();
    }

	public void Quit()
	{
		Application.Quit();
	}

	public void Pause()
	{
		_isPaused = true;
		Time.timeScale = 0.0f;
		PauseMenu.SetActive(true);
		_mouseLockedInGame = _mouseLocked;
		FreeMouse();

	}

	public void Unpause()
	{
		_isPaused = false;
		Time.timeScale = 1.0f;
		PauseMenu.SetActive(false);
		if (_mouseLockedInGame) LockMouse();
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

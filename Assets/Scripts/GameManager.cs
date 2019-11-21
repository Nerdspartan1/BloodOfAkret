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

	public Camera CurrentCamera;

	private void Awake()
	{
		Instance = this;
	}

	void Start()
    {
		Menu.SetActive(true);
		Player.SetActive(false);
		Intro.SetActive(false);
		Game.SetActive(false);
    }

	public void StartIntro()
	{
		Menu.SetActive(false);
		Player.transform.position = IntroSpawnLocation.transform.position;
		Intro.SetActive(true);
		Player.SetActive(true);
		Intro.GetComponent<DesertWandering>().PlayIntro();
		Cursor.lockState = CursorLockMode.Locked;
	}

	public void StartGame()
	{
		Menu.SetActive(false);
		Player.transform.position = GameSpawnLocation.transform.position;
		Game.SetActive(true);
		Player.SetActive(true);
		Cursor.lockState = CursorLockMode.Locked;
	}

	public void ResetGame()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

}

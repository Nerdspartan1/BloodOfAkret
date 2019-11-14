using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;

	public GameObject Player;
	public GameObject IntroSpawnLocation;
	public GameObject GameSpawnLocation;

	public GameObject Menu;
	public GameObject Intro;
	public GameObject Game;

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
	}

	public void StartGame()
	{
		Menu.SetActive(false);
		Player.transform.position = GameSpawnLocation.transform.position;
		Game.SetActive(true);
		Player.SetActive(true);
	}
}

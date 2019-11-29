using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
	public Text KillCounter;
	public Text Comment;

	public void OnEnable()
	{
		KillCounter.text = $"Undeads put to rest : {WaveManager.EnemiesKilled}" +
			$"\nGods slain : {WaveManager.GodsSlain}";
		switch (WaveManager.GodsSlain)
		{
			case 0:
				Comment.text = "The gods did not notice you"; break;
			case 1:
				Comment.text = "The gods are amused by your struggle"; break;
			case 2:
				Comment.text = "The gods are impressed by your fights"; break;
			case 3:
				Comment.text = "The gods are concerned about your power"; break;
			case 4:
				Comment.text = "The gods fear you"; break;
		}
	}
}

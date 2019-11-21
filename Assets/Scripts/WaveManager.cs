using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
	public static WaveManager Instance;
	private int _points;
	public int Points {
		get
		{
			return _points;
		}
		set
		{
			_points = value;
			PointsText.text = $"{_points}";
		}
	}

	public List<GameObject> EnemySpawns;
	public GameObject EnemyPrefab;
	public Shop Shop;
	public Text PointsText;

	private int _numberOfEnemiesAlive;
	private int _wave = 0;

	private void Awake()
	{
		Instance = this;
	}

	void Start()
    {
		Shop.gameObject.SetActive(false);
		Points = 0;
		StartNextWave();
    }

	public void Spawn(int number)
	{
		_numberOfEnemiesAlive = number;
		for(int i=0; i<number; ++i)
		{
			Enemy enemy = Instantiate(EnemyPrefab, EnemySpawns[Random.Range(0, EnemySpawns.Count - 1)].transform.position, Quaternion.identity, transform).GetComponent<Enemy>();
			enemy.Target = GameManager.Instance.Player;
		}
	}

	public void EnemyDown()
	{
		_numberOfEnemiesAlive--;

		if(_numberOfEnemiesAlive == 0)
		{
			EndWave();
		}
	}

	public void EndWave()
	{
		OpenShop();
	}

	public void OpenShop()
	{
		Shop.gameObject.SetActive(true);
		Shop.PresentPerks();

		GameManager.Instance.FreeMouse();
	}

	public void CloseShop()
	{
		Shop.gameObject.SetActive(false);

		GameManager.Instance.LockMouse();
	}

	public void StartNextWave()
	{
		switch (++_wave)
		{
			case 1:
				Spawn(1);
				break;
			case 2:
				Spawn(2);
				break;
			default:
				Spawn(3);
				break;
		}
	}

}

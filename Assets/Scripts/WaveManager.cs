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
	public GameObject SkeletonPrefab;
	public GameObject MummyPrefab;
	public GameObject GolemPrefab;
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

	public void Spawn(int skeletons, int mummies = 0, int golems = 0)
	{
		_numberOfEnemiesAlive = skeletons + mummies + golems;
		for(int i=0; i < skeletons; ++i)
		{
			Enemy enemy = Instantiate(SkeletonPrefab, EnemySpawns[Random.Range(0, EnemySpawns.Count - 1)].transform.position, Quaternion.identity, transform).GetComponent<Enemy>();
			enemy.Target = GameManager.Instance.Player;
		}
		for (int i = 0; i < mummies; ++i)
		{
			Enemy enemy = Instantiate(MummyPrefab, EnemySpawns[Random.Range(0, EnemySpawns.Count - 1)].transform.position, Quaternion.identity, transform).GetComponent<Enemy>();
			enemy.Target = GameManager.Instance.Player;
		}
		for (int i = 0; i < golems; ++i)
		{
			Enemy enemy = Instantiate(GolemPrefab, EnemySpawns[Random.Range(0, EnemySpawns.Count - 1)].transform.position, Quaternion.identity, transform).GetComponent<Enemy>();
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
				Spawn(0,0,1);
				break;
			case 2:
				Spawn(2,1);
				break;
			default:
				Spawn(3,2);
				break;
		}
	}

}

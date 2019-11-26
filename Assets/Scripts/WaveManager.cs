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

	public Animator WaveAnnouncer;

	public float WaveEndDelay = 3f;
	public float WaveStartDelay = 4f;

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
    }

	public void StartGame()
	{
		StartCoroutine(StartNextWave());
	}

	public void Spawn(int skeletons, int mummies = 0, int golems = 0)
	{
		Player player = GameManager.Instance.Player.GetComponent<Player>();
		_numberOfEnemiesAlive = skeletons + mummies + golems;
		for(int i=0; i < skeletons; ++i)
		{
			Enemy enemy = Instantiate(SkeletonPrefab, EnemySpawns[Random.Range(0, EnemySpawns.Count - 1)].transform.position, Quaternion.identity, transform).GetComponent<Enemy>();
			enemy.Target = player;
		}
		for (int i = 0; i < mummies; ++i)
		{
			Enemy enemy = Instantiate(MummyPrefab, EnemySpawns[Random.Range(0, EnemySpawns.Count - 1)].transform.position, Quaternion.identity, transform).GetComponent<Enemy>();
			enemy.Target = player;
		}
		for (int i = 0; i < golems; ++i)
		{
			Enemy enemy = Instantiate(GolemPrefab, EnemySpawns[Random.Range(0, EnemySpawns.Count - 1)].transform.position, Quaternion.identity, transform).GetComponent<Enemy>();
			enemy.Target = player;
		}
	}

	public void EnemyDown()
	{
		_numberOfEnemiesAlive--;

		if(_numberOfEnemiesAlive == 0)
		{
			StartCoroutine(EndWave());
		}
	}

	public IEnumerator EndWave()
	{
		yield return new WaitForSeconds(WaveEndDelay);

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

		StartCoroutine(StartNextWave());
	}

	public IEnumerator StartNextWave()
	{
		WaveAnnouncer.SetTrigger("startWave");
		WaveAnnouncer.gameObject.GetComponentInChildren<Text>().text = $"Wave {++_wave}";

		yield return new WaitForSeconds(WaveStartDelay);

		switch (_wave)
		{
			case 1:
				Spawn(3);
				break;
			case 2:
				Spawn(2,1);
				break;
			default:
				Spawn(3,2,1);
				break;
		}
	}

}

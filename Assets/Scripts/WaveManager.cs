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

	private Player _player;

	public Animator WaveAnnouncer;

	public float WaveEndDelay = 3f;
	public float WaveStartDelay = 4f;

	public int MaxSkeletons = 5;
	public int MaxMummies = 1;
	public int MaxGolems = 1;

	private int _currentSkeletons;
	private int _currentMummies;
	private int _currentGolems;

	private int _remainingSkeletons;
	private int _remainingMummies;
	private int _remainingGolems;


	private int _numberOfEnemiesAlive;
	private int _wave = 0;

	private void Awake()
	{
		Instance = this;
	}

	void Start()
    {
		_player = GameManager.Instance.Player.GetComponent<Player>();
		Shop.gameObject.SetActive(false);
		Points = 0;
    }

	public void StartGame()
	{
		StartCoroutine(StartNextWave());
	}

	public void SpawnWave(int skeletons, int mummies, int golems)
	{
		_remainingSkeletons = skeletons;
		_remainingMummies = mummies;
		_remainingGolems = golems;
		for (int i = 0; i < Mathf.Min(skeletons,MaxSkeletons); i++) SpawnSkeleton();
		for (int i = 0; i < Mathf.Min(mummies,MaxMummies); i++) SpawnMummy();
		for (int i = 0; i < Mathf.Min(golems,MaxGolems); i++) SpawnGolem();

	}

	public void SpawnSkeleton()
	{
		Enemy enemy = Instantiate(SkeletonPrefab, EnemySpawns[Random.Range(0, EnemySpawns.Count - 1)].transform.position, Quaternion.identity, transform).GetComponent<Enemy>();
		enemy.Target = _player;
        Debug.Log("Skeleton!");
	}

	public void SpawnMummy()
	{
		Enemy enemy = Instantiate(MummyPrefab, EnemySpawns[Random.Range(0, EnemySpawns.Count - 1)].transform.position, Quaternion.identity, transform).GetComponent<Enemy>();
		enemy.Target = _player;
        Debug.Log("Mummy!");
	}

	public void SpawnGolem()
	{
		Enemy enemy = Instantiate(GolemPrefab, EnemySpawns[Random.Range(0, EnemySpawns.Count - 1)].transform.position, Quaternion.identity, transform).GetComponent<Enemy>();
		enemy.Target = _player;
        Debug.Log("Golem!");
	}


	public void EnemyDown(Enemy who)
	{
		if (who is EnemyHarasser)
		{
			if (_remainingSkeletons-- > MaxSkeletons) SpawnSkeleton();
		}
		else if (who is EnemyCharger)
		{
			if (_remainingMummies-- > MaxMummies) SpawnMummy();
		}
		else if (who is EnemyCaster)
		{
			if (_remainingGolems-- > MaxGolems) SpawnGolem();
		}

		if (_remainingGolems + _remainingMummies + _remainingSkeletons == 0)
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
				SpawnWave(1, 1, 1);
				break;
			case 2:
				SpawnWave(0, 1, 0);
				break;
			default:
				SpawnWave(0, 0, 1);
				break;
		}
	}

}

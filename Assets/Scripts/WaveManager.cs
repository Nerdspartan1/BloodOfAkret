﻿using System.Collections;
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
	public GameObject MagePrefab;
	public GameObject GodPrefab;
	public Shop Shop;
	public Text PointsText;

	public Material ArenaSky;
	public GameObject DirectionalLight;

	private Player _player;

	public Animator WaveAnnouncer;

	public float WaveEndDelay = 3f;
	public float WaveStartDelay = 4f;

	public int MaxSkeletons = 5;
	public int MaxMages = 3;
	public int MaxMummies = 1;
	public int MaxGolems = 1;

	private int _remainingSkeletons;
	private int _remainingMummies;
	private int _remainingGolems;
	private int _remainingMages;
	private bool _remainingBoss;

	private bool _forceEndWave = false;

	public static int EnemiesKilled;
	public static int GodsSlain;

	public int Wave = 0;

	private void Awake()
	{
		EnemiesKilled = 0;
		GodsSlain = 0;
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
		RenderSettings.skybox = ArenaSky;
		_player.FPSCamera.clearFlags = CameraClearFlags.Skybox;
		DirectionalLight.transform.Rotate(Vector3.right, -30);
		StartCoroutine(StartNextWave());
	}

	public void SpawnWave(int skeletons, int mummies, int mages, int golems, bool god = false)
	{
		_forceEndWave = false;
		_remainingSkeletons = skeletons;
		_remainingMummies = mummies;
		_remainingGolems = golems;
		_remainingMages = mages;
		_remainingBoss = god;
		for (int i = 0; i < Mathf.Min(skeletons,MaxSkeletons); i++) SpawnSkeleton();
		for (int i = 0; i < Mathf.Min(mummies,MaxMummies); i++) SpawnMummy();
		for (int i = 0; i < Mathf.Min(golems,MaxGolems); i++) SpawnGolem();
		for (int i = 0; i < Mathf.Min(mages, MaxMages); i++) SpawnMage();
		if (god) SpawnGod();

	}

	public void SpawnSkeleton()
	{
		Enemy enemy = Instantiate(SkeletonPrefab, EnemySpawns[Random.Range(0, EnemySpawns.Count - 1)].transform.position, Quaternion.identity, transform).GetComponent<Enemy>();
		enemy.Target = _player;
	}

	public void SpawnMummy()
	{
		Enemy enemy = Instantiate(MummyPrefab, EnemySpawns[Random.Range(0, EnemySpawns.Count - 1)].transform.position, Quaternion.identity, transform).GetComponent<Enemy>();
		enemy.Target = _player;
	}

	public void SpawnGolem()
	{
		Enemy enemy = Instantiate(GolemPrefab, EnemySpawns[Random.Range(0, EnemySpawns.Count - 1)].transform.position, Quaternion.identity, transform).GetComponent<Enemy>();
		enemy.Target = _player;
	}

	public void SpawnMage()
	{
		EnemyMage enemy = Instantiate(MagePrefab, EnemySpawns[Random.Range(0, EnemySpawns.Count - 1)].transform.position, Quaternion.identity, transform).GetComponent<EnemyMage>();
		enemy.Target = _player;
		enemy.PatrolCenter = _player.gameObject;
	}

	public void SpawnGod()
	{
		EnemyMage enemy = Instantiate(GodPrefab, GameManager.Instance.Game.transform.position, Quaternion.identity, transform).GetComponent<EnemyMage>();
		enemy.Target = _player;
		enemy.PatrolCenter = GameManager.Instance.Game;
	}

	public void EnemyDown(Enemy who)
	{
		if (_forceEndWave) return;

		if (who is EnemyHarasser)
		{
			if (_remainingSkeletons-- > MaxSkeletons) SpawnSkeleton();
			EnemiesKilled++;
		}
		else if (who is EnemyCharger)
		{
			if (_remainingMummies-- > MaxMummies) SpawnMummy();
			EnemiesKilled++;
		}
		else if (who is God)
		{
			_remainingGolems = 0;
			_remainingMages = 0;
			_remainingSkeletons = 0;
			_remainingMummies = 0;
			_remainingBoss = false;
			_forceEndWave = true;
			foreach (var enemy in GameManager.Instance.Game.GetComponentsInChildren<Enemy>())
			{
				enemy.Die();
			}
			GodsSlain++;
		}
		else if (who is EnemyMage)
		{
			if (_remainingMages-- > MaxMages) SpawnMage();
			EnemiesKilled++;
		}
		else if (who is EnemyCaster)
		{
			if (_remainingGolems-- > MaxGolems) SpawnGolem();
			EnemiesKilled++;
		}


		if (!_remainingBoss && _remainingGolems + _remainingMummies + _remainingSkeletons + _remainingMages == 0)
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
		WaveAnnouncer.gameObject.GetComponentInChildren<Text>().text = $"Wave {++Wave}";

		yield return new WaitForSeconds(WaveStartDelay);

		switch (Wave)
		{
			case 1:
				SpawnWave(0, 0, 3, 0,true);
				break;
			case 2:
				SpawnWave(0, 1, 0,0);
				break;
			default:
				SpawnWave(0, 0, 1,0);
				break;
		}
	}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
	public static WaveManager Instance;

	public List<GameObject> EnemySpawns;
	public GameObject EnemyPrefab;
	public GameObject Player;

	private int _numberOfEnemiesAlive;
	private int _wave = 0;

	private void Awake()
	{
		Instance = this;
	}

	void Start()
    {
		StartNextWave();
    }

	public void Spawn(int number)
	{
		_numberOfEnemiesAlive = number;
		for(int i=0; i<number; ++i)
		{
			Enemy enemy = Instantiate(EnemyPrefab, EnemySpawns[Random.Range(0, EnemySpawns.Count - 1)].transform.position, Quaternion.identity, transform).GetComponent<Enemy>();
			enemy.Target = Player;
		}
	}

	public void EnemyDown()
	{
		_numberOfEnemiesAlive--;

		if(_numberOfEnemiesAlive == 0)
		{
			StartNextWave();
		}
	}

	public void StartNextWave()
	{
		switch (++_wave)
		{
			case 1:
				Spawn(5);
				break;
			case 2:
				Spawn(5);
				break;
		}
	}
}

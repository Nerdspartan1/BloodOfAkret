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
	private bool _bossWave = false;

	public static int EnemiesKilled;
	public static int GodsSlain;

	public int Wave = 0;

    public FMOD.Studio.EventInstance ingamemusicEvent;
    FMOD.Studio.Bus MageBus;

	private bool _skyIsSet = false;

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

	public void SetSky()
	{
		if (!_skyIsSet)
		{
			RenderSettings.skybox = ArenaSky;
			_player.FPSCamera.clearFlags = CameraClearFlags.Skybox;
			DirectionalLight.transform.Rotate(Vector3.right, -30);
			_skyIsSet = true;
		}
	}

	public void StartGame()
	{

		SetSky();

		StartCoroutine(StartNextWave());

        GameManager.Instance.menuEvent.setParameterByName("Game Start", 1f);

        ingamemusicEvent = FMODUnity.RuntimeManager.CreateInstance(SoundManager.sm.music);
        ingamemusicEvent.setParameterByName("Wave Prog", Wave);
	}

	public void SpawnWave(int skeletons, int mummies, int mages, int golems, bool god = false)
	{

        ingamemusicEvent.start();

		_forceEndWave = false;
		_remainingSkeletons = skeletons;
		_remainingMummies = mummies;
		_remainingGolems = golems;
		_remainingMages = mages;
		_remainingBoss = god;
		_bossWave = god;
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
        
        FMODUnity.RuntimeManager.PlayOneShotAttached(SoundManager.sm.skelwarrvoice, enemy.gameObject);
	}

	public void SpawnMummy()
	{
		Enemy enemy = Instantiate(MummyPrefab, EnemySpawns[Random.Range(0, EnemySpawns.Count - 1)].transform.position, Quaternion.identity, transform).GetComponent<Enemy>();
		enemy.Target = _player;
        
        FMODUnity.RuntimeManager.PlayOneShotAttached(SoundManager.sm.mummywarrvoice, enemy.gameObject);
    }

	public void SpawnGolem()
	{
		Enemy enemy = Instantiate(GolemPrefab, EnemySpawns[Random.Range(0, EnemySpawns.Count - 1)].transform.position, Quaternion.identity, transform).GetComponent<Enemy>();
		enemy.Target = _player;

        FMODUnity.RuntimeManager.PlayOneShotAttached(SoundManager.sm.golemvoice, enemy.gameObject);
    }

	public void SpawnMage()
	{
		EnemyMage enemy = Instantiate(MagePrefab, EnemySpawns[Random.Range(0, EnemySpawns.Count - 1)].transform.position, Quaternion.identity, transform).GetComponent<EnemyMage>();
		enemy.Target = _player;
		enemy.PatrolCenter = _player.gameObject;

        FMODUnity.RuntimeManager.PlayOneShotAttached(SoundManager.sm.skelmagevoice, enemy.gameObject);
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
			_remainingBoss = false;
			if (Wave <= 15)
			{
				_remainingGolems = 0;
				_remainingMages = 0;
				_remainingSkeletons = 0;
				_remainingMummies = 0;
				
				_forceEndWave = true;
				foreach (var enemy in GameManager.Instance.Game.GetComponentsInChildren<Enemy>())
				{
					enemy.Die();
				}
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

            ingamemusicEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

            //MageBus = FMODUnity.RuntimeManager.GetBus("Bus:/Enemy/Skeleton/Mage Movem");
            //MageBus.stopAllEvents(FMOD.Studio.STOP_MODE.IMMEDIATE);
            
            
            
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
		Shop.PresentPerks(_bossWave);

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
				SpawnWave(3, 0, 0, 0);
				break;
			case 2:
				SpawnWave(3, 1, 0, 0);
				break;
			case 3:
				SpawnWave(4, 0, 3, 0);
				break;
			case 4:
				MaxMummies++;
				SpawnWave(0, 3, 3, 1);
				break;
			case 5:
				SpawnWave(0, 0, 0, 0, true);
                break;
			case 6:
                MaxSkeletons++;
				MaxMages++;
				SpawnWave(5, 4, 5, 0);
				break;
			case 7:
				MaxGolems++;
				SpawnWave(0, 4, 8, 2);
				break;
			case 8:
				SpawnWave(10, 0, 0, 2);
				break;
			case 9:
				SpawnWave(0, 4, 8, 1);
				break;
			case 10:
                MaxMages += 2;
				SpawnWave(0, 0, 15, 0,true);
				break;
			case 11:
                MaxMummies++;
				MaxSkeletons += 2;
				SpawnWave(8, 6, 0, 4);
				break;
			case 12:
				SpawnWave(12, 0, 10, 0);
				break;
			case 13:
				SpawnWave(10, 8, 0, 2);
				break;
			case 14:
				SpawnWave(0, 0, 10, 2);
				break;
			case 15:
                SpawnWave(24, 0, 0, 6,true);
				break;
			default:

                SpawnWave(MaxSkeletons++, MaxMummies++, MaxMages++, MaxGolems++, true);

				break;


		}
        ingamemusicEvent.setParameterByName("Wave Prog", Wave);

        ingamemusicEvent.setParameterByName("Boss Wave", _bossWave ? 1f : 0f);

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMage : EnemyCaster
{
	public float MoveSpeed = 0.05f;
	public float PatrolRadius = 20f;
	public GameObject PatrolCenter;
	public float AltitudeOverGround = 4f;

	protected float _theta;

	protected override void Start()
	{
		_theta = Random.Range(0, 6.28f);
		base.Start();
	}

	protected override void Update()
    {
		Vector3 thisToPlayer = Target.transform.position - transform.position;

		
		transform.position = Damp(
			transform.position, 
			new Vector3(
				PatrolCenter.transform.position.x + PatrolRadius * Mathf.Cos(_theta), 
				PatrolCenter.transform.position.y + AltitudeOverGround, 
				PatrolCenter.transform.position.z + PatrolRadius * Mathf.Sin(_theta)), 
			0.5f, 
			Time.deltaTime);

		transform.LookAt(Target.transform,Vector3.up);

		_theta += Time.deltaTime * MoveSpeed;


		if (_timeBeforeNextCast < 0f)
		{
			_anim.SetTrigger("cast");
			_timeBeforeNextCast = CastingCooldown;
		}
		_timeBeforeNextCast -= Time.deltaTime;
	}

	public override void Cast()
	{
		for (int i = 0; i < NumberOfProjectiles; ++i)
		{
			var proj = Instantiate(CastProjectile, HitCast.transform.position, Quaternion.LookRotation(Target.FPSCamera.transform.position - HitCast.transform.position), GameManager.Instance.Game.transform).GetComponent<Fireball>();
			proj.Target = Target.FPSCamera.gameObject;
		}
		if (_nav) _nav.isStopped = false;
	}

	public static Vector3 Damp(Vector3 source, Vector3 target, float smoothing, float dt)
	{
		return Vector3.Lerp(source, target, 1 - Mathf.Pow(smoothing, dt));
		
	}
}

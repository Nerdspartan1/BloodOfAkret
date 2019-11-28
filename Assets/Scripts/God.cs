using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class God : EnemyMage
{

	public float SwitchPhasePeriod = 10f;

	public float SwitchPhaseProbability = 0.5f;

	public float _nextSwitchPhaseTime;

	public float DeathAnimationLength = 5f;

	public GameObject AoEProjectile;
	public GameObject LongRangeProjectile;

	public GameObject DeathEffect;


	public enum Phase
	{
		Circling,
		Center
	}

	public Phase CurrentPhase = Phase.Center;

	protected override void Update()
	{
		Vector3 thisToPlayer = Target.transform.position - transform.position;

		if(Time.time >= _nextSwitchPhaseTime)
		{
			if(Random.value < SwitchPhaseProbability)
			{
				if (CurrentPhase == Phase.Circling) CurrentPhase = Phase.Center;
				else CurrentPhase = Phase.Circling;
			}
			_nextSwitchPhaseTime = Time.time + SwitchPhasePeriod;
		}

		Vector3 targetPosition = PatrolCenter.transform.position;

		switch (CurrentPhase)
		{
			case Phase.Circling:
				targetPosition = PatrolCenter.transform.position + new Vector3(
					PatrolRadius * Mathf.Cos(_theta),
					AltitudeOverGround,
					PatrolRadius * Mathf.Sin(_theta));
				break;
			case Phase.Center:
				targetPosition = PatrolCenter.transform.position + new Vector3(
					0,
					AltitudeOverGround + AltitudeOverGround / 4f * Mathf.Sin(_theta),
					0);
				break;
		}


		transform.position = Damp(
			transform.position,
			targetPosition,
			0.5f,
			Time.deltaTime);

		transform.LookAt(Target.transform, Vector3.up);

		_theta += Time.deltaTime * MoveSpeed;


		if (_timeBeforeNextCast < 0f)
		{
			float roll = Random.value;
			if(roll < 0.4f)
			{
				_anim.SetTrigger("attackSingle");
			}else if(roll < 0.7f)
			{
				_anim.SetTrigger("attackDouble");
			}
			else
			{
				_anim.SetTrigger("attackArea");
			}
			_timeBeforeNextCast = CastingCooldown;
		}
		_timeBeforeNextCast -= Time.deltaTime;
	}

	public void Blast(int nb)
	{
		Vector3 thisToPlayer = Target.FPSCamera.transform.position - HitCast.transform.position;
		GameObject projectile = CastProjectile;
		if (thisToPlayer.magnitude > CastRange)
		{
			projectile = LongRangeProjectile;
			nb = 1;
		}
			
		for (int i = 0; i < nb; ++i)
		{
			var proj = Instantiate(projectile, HitCast.transform.position, Quaternion.LookRotation((Target.FPSCamera.transform.position + 3f*Random.onUnitSphere) - HitCast.transform.position), GameManager.Instance.Game.transform).GetComponent<Fireball>();
			proj.Target = Target.FPSCamera.gameObject;
		}
	}

	public void AreaAttack(int nb)
	{
		for (int i = 0; i < nb; ++i)
		{
			var proj = Instantiate(AoEProjectile, Target.transform.position + 20f*Vector3.up + 10f*Random.insideUnitSphere, Quaternion.LookRotation(Vector3.down), GameManager.Instance.Game.transform).GetComponent<Fireball>();
			proj.Target = Target.FPSCamera.gameObject;
		}
	}

	public override void Die()
	{

		_anim.SetBool("dead", true);
		this.enabled = false;

		
		if (_disintegrate) _disintegrate.enabled = true;

		var effect = Instantiate(DeathEffect, transform.position, Quaternion.identity);
		StartCoroutine(SpinCoroutine());
		Destroy(effect);

		WaveManager.Instance.EnemyDown(this);
	}

	public IEnumerator SpinCoroutine()
	{
		float _t = 0;
		while (_t < DeathAnimationLength)
		{
			transform.Rotate(Vector3.up, _t);
			yield return null;
			_t += Time.deltaTime;
		}

	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharger : Enemy
{
	public float ChargeRange = 6f;
	public float ChargeSpeed = 7f;
	public float ChargeCooldown = 5f;

	private bool _charging = false;
	
	private float _timeBeforeNextCharge = 0f;
	private float _normalSpeed;
	private float _normalAcceleration;

    protected override void Update()
    {
		if (!_charging)
		{
			_anim.SetFloat("speed", _nav.velocity.magnitude);
			if (Target) _nav.SetDestination(Target.transform.position);

			float distance = Vector3.Distance(Target.transform.position, HitCast.transform.position);

			if (distance < Range)
			{
				_anim.SetTrigger("attack");
			}
			else if (_timeBeforeNextCharge < 0f && distance < ChargeRange)
			{
				_charging = true;
				_nav.isStopped = true;
				_anim.SetTrigger("chargeAttack");
				_timeBeforeNextCharge = ChargeCooldown;
				
			}

			_timeBeforeNextCharge -= Time.deltaTime;
		}
	}

	public void BeginCharge()
	{
		_normalSpeed = _nav.speed;
		_normalAcceleration = _nav.acceleration;
		_nav.isStopped = false;
		_nav.speed = ChargeSpeed;
		_nav.acceleration = 100000f;
		_nav.SetDestination(transform.position + (Target.transform.position - transform.position) * 1.2f);
		
	}

	public void EndCharge()
	{
		_nav.speed = _normalSpeed;
		_nav.acceleration = _normalAcceleration;
		_charging = false;
	}


}

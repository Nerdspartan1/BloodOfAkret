using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHarasser : Enemy
{
	public Vector2 DestinationSwitchRandomRange = new Vector2(2f, 4f);
	public Vector2 HarassDistanceRandomRange = new Vector2(4f, 10f);

	public float MaxDistance = 12f;
	public float AttackCooldown = 5f;

	private float _nextDestinationSwitch = 0;
	private float _nextAttack = 0;

	protected override void Start()
	{
		base.Start();
	}

	protected override void Update()
	{
		_anim.SetFloat("speed", _nav.velocity.magnitude);
		if (Target)
		{
			float distance = Vector3.Distance(Target.transform.position, HitCast.transform.position);
			if (distance > MaxDistance)
			{
				_nav.SetDestination(Target.transform.position);
				_nextDestinationSwitch = Time.time;
			}
			else if (Time.time >= _nextDestinationSwitch)
			{
				_nav.SetDestination(Target.transform.position + Random.Range(HarassDistanceRandomRange.x, HarassDistanceRandomRange.y) * Vector3.ProjectOnPlane(Random.onUnitSphere,Vector3.up));
				_nextDestinationSwitch = Time.time + Random.Range(DestinationSwitchRandomRange.x, DestinationSwitchRandomRange.y); ;
			}
			
			if (distance < Range && Time.time >= _nextAttack)
			{
				_anim.SetTrigger("attack");
				_nextAttack = Time.time + AttackCooldown;
			}

		}
	}

    public override void Die()
    {
        base.Die();
        FMODUnity.RuntimeManager.PlayOneShotAttached(SoundManager.sm.skelcommondeath, this.gameObject);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHarasser : Enemy
{
	public Vector2 DestinationSwitchRandomRange = new Vector2(2f, 4f);
	public Vector2 HarassDistanceRandomRange = new Vector2(4f, 10f);
	private float _nextDestinationSwitch;

	protected override void Start()
	{
		base.Start();
	}

	protected override void Update()
	{
		_anim.SetFloat("speed", _nav.velocity.magnitude);
		if (Target)
		{
			if (Time.time >= _nextDestinationSwitch)
			{
				_nav.SetDestination(Target.transform.position + Random.Range(HarassDistanceRandomRange.x, HarassDistanceRandomRange.y) * Vector3.ProjectOnPlane(Random.onUnitSphere,Vector3.up));
				_nextDestinationSwitch = Time.time + Random.Range(DestinationSwitchRandomRange.x, DestinationSwitchRandomRange.y); ;
			}
			
			if (Vector3.Distance(Target.transform.position, HitCast.transform.position) < Range)
			{
				_anim.SetTrigger("attack");

			}
		}
	}
}

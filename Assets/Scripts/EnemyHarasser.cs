using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHarasser : Enemy
{

	protected override void Update()
	{
		if (Target) _nav.SetDestination(Target.transform.position);
		if (Vector3.Distance(Target.transform.position, HitCast.transform.position) < Range)
		{
			_anim.SetTrigger("attack");

		}
	}
}

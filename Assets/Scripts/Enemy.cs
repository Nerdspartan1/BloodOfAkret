using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : vp_DamageHandler
{
	public GameObject Target;
	public GameObject Ragdoll;

	private NavMeshAgent _nav;
	private Animator _anim;
	private float _lastDamageTaken;

    void Start()
    {
		_nav = GetComponent<NavMeshAgent>();
		_anim = GetComponent<Animator>();
		foreach (Rigidbody rb in Ragdoll.GetComponentsInChildren<Rigidbody>())
		{
			rb.isKinematic = true;
		}
	}

	public override void Damage(vp_DamageInfo damageInfo)
	{
		_lastDamageTaken = damageInfo.Damage;
		base.Damage(damageInfo);
	}

	public override void Die()
	{
		Vector3 pushForce = 10f*(transform.position - Target.transform.position).normalized * _lastDamageTaken;
		foreach (Rigidbody rb in Ragdoll.GetComponentsInChildren<Rigidbody>())
		{
			rb.isKinematic = false;
			rb.AddForce(pushForce,ForceMode.Impulse);
		}
		_nav.enabled = false;
		_anim.enabled = false;
		this.enabled = false;

		WaveManager.Instance.EnemyDown();
	}

	void Update()
    {
		if(Target) _nav.SetDestination(Target.transform.position);
    }
}

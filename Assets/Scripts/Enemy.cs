using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : vp_DamageHandler
{
	[Header("References")]
	public GameObject Target;
	public GameObject Ragdoll;
	public GameObject HitCast;
	public GameObject Projectile;
	public float Range = 1f;

	[Header("Stats")]
	public int Points;

	public float RagdollLifeTime = 10f;

	protected NavMeshAgent _nav;
	protected Animator _anim;
	private Collider _collider;
	private float _lastDamageTaken;
	private Disintegrate _disintegrate;

	protected virtual void Start()
    {
		_collider = GetComponent<Collider>();
		_disintegrate = GetComponent<Disintegrate>();
		if (_disintegrate) _disintegrate.enabled = false;
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
		_collider.enabled = false;
		Vector3 pushForce = 10f*(transform.position - Target.transform.position).normalized * _lastDamageTaken;
		foreach (Rigidbody rb in Ragdoll.GetComponentsInChildren<Rigidbody>())
		{
			rb.isKinematic = false;
			rb.AddForce(pushForce,ForceMode.Impulse);
		}
		if(_nav) _nav.enabled = false;
		_anim.enabled = false;
		this.enabled = false;


		if (_disintegrate) _disintegrate.enabled = true;
		

		WaveManager.Instance.Points += Points;
		WaveManager.Instance.EnemyDown();
	}

	protected virtual void Update()
	{

	}

	public void Hit()
	{
		var proj = Instantiate(Projectile, HitCast.transform.position, Quaternion.LookRotation(HitCast.transform.forward), GameManager.Instance.Game.transform);
		var fb = proj.GetComponent<Fireball>();
		if (fb) fb.Target = Target;
	}

}

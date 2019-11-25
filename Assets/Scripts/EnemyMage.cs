using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMage : EnemyCaster
{
	private Rigidbody _rb;
	public float CloseInDistance = 15f;
	public float MoveAwayDistance = 5f;
	public float MoveSpeed = 5f;
	public float AltitudeOverPlayer = 4f;
	public float VerticalAcceleration = 10f;

	protected override void Start()
	{
		_rb = GetComponent<Rigidbody>();
		base.Start();
	}

	protected override void Update()
    {
		Vector3 thisToPlayer = Target.transform.position - transform.position;
        if(thisToPlayer.magnitude > CloseInDistance)
		{
			_rb.AddForce(MoveSpeed * thisToPlayer.normalized, ForceMode.Acceleration);
		}else if (thisToPlayer.magnitude < MoveAwayDistance)
		{
			_rb.AddForce(MoveSpeed * -thisToPlayer.normalized, ForceMode.Acceleration);
		}

		float targetAltitude = Target.transform.position.y + AltitudeOverPlayer;
		float currentAltitude = transform.position.y;
		_rb.AddForce(VerticalAcceleration* (targetAltitude - currentAltitude) * Vector3.up, ForceMode.Acceleration);

		if(thisToPlayer.magnitude < CastRange)
		{
			_anim.SetTrigger("cast");
		}
    }
}

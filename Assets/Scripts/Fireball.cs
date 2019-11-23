using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
	public GameObject Target;
	public float InitialSpeed = 4f;
	public float HomingAcceleration = 1f;
	public float Damage = 1f;

	private Rigidbody _rb;

    void Start()
    {
		_rb = GetComponent<Rigidbody>();
		_rb.velocity = InitialSpeed * transform.forward;
    }

    void Update()
    {
		_rb.AddForce((Target.transform.position - transform.position).normalized * HomingAcceleration, ForceMode.Acceleration);
    }

	private void OnCollisionEnter(Collision collision)
	{
		Destroy(gameObject);
		var player = collision.gameObject.GetComponent<Player>();
		if (player) player.Damage(Damage);
	}
}

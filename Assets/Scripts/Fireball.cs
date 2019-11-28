using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
	public GameObject Target;
	public Vector2 InitialSpeed = new Vector2(4f, 4f);
	public Vector2 HomingAcceleration = new Vector2(1, 2);
	public float Damage = 1f;

	public float LifeTime = 8f;

	private Rigidbody _rb;
	private float _acceleration;

    void Start()
    {
		_rb = GetComponent<Rigidbody>();
		_rb.velocity = Random.Range(InitialSpeed.x, InitialSpeed.y) * transform.forward;
		_acceleration = Random.Range(HomingAcceleration.x, HomingAcceleration.y);

	}

    void Update()
    {
		_rb.AddForce((Target.transform.position - transform.position).normalized * _acceleration, ForceMode.Acceleration);
		LifeTime -= Time.deltaTime;
		if (LifeTime < 0) Destroy(gameObject);
    }

	private void OnCollisionEnter(Collision collision)
	{
		Destroy(gameObject);

		var player = collision.gameObject.GetComponent<Player>();
		if (player)
		{
			var impactPoint = new GameObject();
			impactPoint.transform.position = transform.position;
			Destroy(impactPoint, 2f);
			player.Damage(new vp_DamageInfo(Damage, impactPoint.transform, vp_DamageInfo.DamageType.Bullet));
		}
	}
}

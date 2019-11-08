using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Rendering.PostProcessing;

public class PlayerController : MonoBehaviour
{
	[Header("Controls")]
	public float CameraSensitivity = 100f;
	public bool Azerty = false;

	[Header("Movement")]
	public float MoveSpeed = 6f;
	public float AirMoveSpeed = 2f;
	public float LandingStabilizationRate = 0.5f;
	public float JumpHeight;
	public float Gravity;
	public LayerMask GroundLayer;

	private float _groundAltitudeAtJumpingLocation;

	private bool _isGrounded;
	private Vector3 _speed;
	private Vector3 _momentum;
	private float _movementControl = 1f;

	private float _rotationY = 0F;


    [HideInInspector]
	public Camera Camera;
	public GameObject GroundCheck;
	
	private CharacterController _controller;

	private void Awake()
	{
		Camera = GetComponentInChildren<Camera>();

		_controller = GetComponent<CharacterController>();
	}

	void Update()
	{
		UpdateCameraRotation();
		UpdateMovement();
	}

	private void UpdateCameraRotation()
	{
		Cursor.lockState = CursorLockMode.Locked;
		float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * CameraSensitivity * Time.deltaTime;

		_rotationY += Input.GetAxis("Mouse Y") * CameraSensitivity * Time.deltaTime;
		_rotationY = Mathf.Clamp(_rotationY, -70f, +70f);

		transform.localEulerAngles = new Vector3(0, rotationX, 0);
		Camera.transform.localEulerAngles = new Vector3(-_rotationY, 0, 0);
	}

	private void UpdateMovement()
	{
		_isGrounded = Physics.CheckSphere(GroundCheck.transform.position, 0.1f,GroundLayer);

		float forwardInput = Azerty ? Input.GetAxis("VerticalAzerty") : Input.GetAxis("Vertical");
		float lateralInput = Azerty ? Input.GetAxis("HorizontalAzerty") : Input.GetAxis("Horizontal");

		Vector3 horizontalMovement = transform.forward * forwardInput + transform.right * lateralInput;

		// cap the max speed so that the player doesn't go faster diagonally
		if (horizontalMovement.sqrMagnitude > 1f) horizontalMovement.Normalize();

		if (_isGrounded)
		{
			_speed.x = (1f - _movementControl)*_momentum.x + _movementControl * (horizontalMovement.x * MoveSpeed);
			_speed.z = (1f - _movementControl)*_momentum.z + _movementControl * (horizontalMovement.z * MoveSpeed);

			if (_movementControl < 1)
				_movementControl += LandingStabilizationRate * Time.deltaTime;
			
			if (_movementControl > 1)
				_movementControl = 1f;

			if (Input.GetButtonDown("Jump"))
			{
				_speed.y = Mathf.Sqrt(2 * -Gravity * JumpHeight);
				_momentum = Vector3.ProjectOnPlane(_speed-_speed.normalized*AirMoveSpeed, Vector3.up);
				_movementControl = 0f;
			}
		}
		else
		{
			_speed.x = _momentum.x + horizontalMovement.x * AirMoveSpeed;
			_speed.z = _momentum.z + horizontalMovement.z * AirMoveSpeed;
			_speed.y += Gravity * Time.deltaTime;
		}
		
		_controller.Move(_speed*Time.deltaTime);

	}

	public void SetAzerty(bool azerty)
	{
		Azerty = azerty;
		PlayerPrefs.SetInt("azerty", azerty ? 1 : 0);
	}

	public void SetSensitivity(float sensitivity)
	{
		CameraSensitivity = sensitivity;
		PlayerPrefs.SetFloat("sensitivity", sensitivity);
	}
}

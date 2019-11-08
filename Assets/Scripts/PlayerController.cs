using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Rendering.PostProcessing;

public class PlayerController : MonoBehaviour
{
	public float cameraSensitivity = 100f;

    public float MoveSpeed = 6f;

	private float rotationY = 0F;

	public bool Azerty = false;

	private float _confusedCameraAngle;
	private float _timeConfused = 0f;

    [HideInInspector]
	public Camera Camera;
	
	private CharacterController _controller;


	private float _height;

	private void Awake()
	{
		Camera = GetComponentInChildren<Camera>();

		_controller = GetComponent<CharacterController>();
		_height = transform.position.y;

	}

	void Update()
	{
		UpdateCameraRotation();
		UpdateMovement();


		if (_timeConfused > 0f) _timeConfused -= Time.deltaTime;
	}

	private void UpdateCameraRotation()
	{
		Cursor.lockState = CursorLockMode.Locked;
		float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * cameraSensitivity * Time.deltaTime;

		rotationY += Input.GetAxis("Mouse Y") * cameraSensitivity * Time.deltaTime;
		rotationY = Mathf.Clamp(rotationY, -70f, +70f);

		transform.localEulerAngles = new Vector3(0, rotationX, 0);
		Camera.transform.localEulerAngles = new Vector3(-rotationY, 0, _timeConfused > 0f ? _confusedCameraAngle : 0f);
	}

	private void UpdateMovement()
	{
		float forwardInput = Azerty ? Input.GetAxis("VerticalAzerty") : Input.GetAxis("Vertical");
		float lateralInput = Azerty ? Input.GetAxis("HorizontalAzerty") : Input.GetAxis("Horizontal");

		Vector3 movement = transform.forward * forwardInput + transform.right * lateralInput;
		// cap the max speed so that the player doesn't go faster diagonally
		if (movement.sqrMagnitude > 1f) movement.Normalize();
		_controller.SimpleMove(movement * MoveSpeed);

	}

	public void SetAzerty(bool azerty)
	{
		Azerty = azerty;
		PlayerPrefs.SetInt("azerty", azerty ? 1 : 0);
	}

	public void SetSensitivity(float sensitivity)
	{
		cameraSensitivity = sensitivity;
		PlayerPrefs.SetFloat("sensitivity", sensitivity);
	}

}

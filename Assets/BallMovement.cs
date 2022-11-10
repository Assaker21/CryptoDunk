using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    private new Rigidbody2D rigidbody;

	[Header("Components")]
	[SerializeField] ParticleSystem jumpParticles;

	[Header("Settings")]
	[SerializeField] float startXVelocity;
	[SerializeField] Vector2 maxVelocity;
    [SerializeField] float jumpVelocity;
	[SerializeField] float maxAngularVelocity;
	[SerializeField] float angularVelocityImpulse;

	[SerializeField] Vector2 drag;

	private void Start()
	{
		rigidbody = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			OnInput();
		}
	}

	private void FixedUpdate()
	{
		rigidbody.velocity *= Vector2.one - drag;
		rigidbody.velocity = new Vector2(Mathf.Clamp(rigidbody.velocity.x, -maxVelocity.x, maxVelocity.x), Mathf.Clamp(rigidbody.velocity.y, -maxVelocity.y, maxVelocity.y));
	}

	public void OnInput()
	{
		rigidbody.velocity = new Vector2(startXVelocity, jumpVelocity);
		jumpParticles.Play();
		//rigidbody.angularVelocity = Mathf.Clamp(rigidbody.angularVelocity + angularVelocityImpulse, -maxAngularVelocity, maxAngularVelocity);
	}

	public void ResetXVelocity()
	{
		rigidbody.velocity = new Vector2(startXVelocity, rigidbody.velocity.y);
	}

	public Vector2 GetVelocity()
	{
		return rigidbody.velocity;
	}
}

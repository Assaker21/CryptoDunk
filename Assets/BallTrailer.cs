using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallTrailer : MonoBehaviour
{
	[Header("Components")]
	[SerializeField] Master master;

	[Header("Settings")]
	[SerializeField] float distance;

	private void FixedUpdate()
	{
		Vector2 targetPos = master.ball.position;

		targetPos -= distance * master.ball.linearVelocity.normalized;

		transform.position = targetPos;
	}
}

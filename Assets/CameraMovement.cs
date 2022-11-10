using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
	[Header("Components")]
	[SerializeField] Transform target;
	[SerializeField] Vector2 offset;

	[Header("Settings")]
	[SerializeField] float sensitivity;

	Vector3 targetPos;

	private void FixedUpdate()
	{
		targetPos = Vector2.Lerp(transform.position, new Vector2(target.position.x + offset.x, offset.y), Time.deltaTime * sensitivity);
		targetPos.z = -10;
		transform.position = targetPos;
	}
}

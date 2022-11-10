using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallShadow : MonoBehaviour
{
	[Header("Components")]
	[SerializeField] Transform ball;

	[Header("Settings")]
	[SerializeField] float minScale;
    [SerializeField] float maxScale;
    [SerializeField] float ballRange;

	private void Update()
	{
		transform.position = new Vector3(ball.position.x, transform.position.y, 0);
		transform.localScale = new Vector3(Mathf.Clamp((1 - ((ball.position.y - transform.position.y) / ballRange)), minScale, maxScale), 0.25f, 1);
	}
}

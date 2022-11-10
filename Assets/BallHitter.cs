using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallHitter : MonoBehaviour
{
	[SerializeField] Transform ball;
	[SerializeField] float animationTime;
	[SerializeField] AnimationCurve curve;

	new Camera camera;

	private void Start()
	{
		camera = Camera.main;
	}

	public void OnJump()
	{
		transform.position = ball.position;
		/*LeanTween.value(transform.position.x, ScreenBorders.GetBottomLeftCorner(camera).x - 2f, animationTime).setEase(curve).setOnUpdate((float value) =>
		{
			transform.position = new Vector2(value, transform.position.y);
		});*/
	}
}

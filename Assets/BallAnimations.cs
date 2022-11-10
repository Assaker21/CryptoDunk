using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallAnimations : MonoBehaviour
{
	[System.Serializable]
	private enum BallAnimationType { 
		none, ball, coin
	}

	[SerializeField] BallAnimationType isA;

	[Header("Components")]
    [SerializeField] Master master;
	[SerializeField] Transform ballScaler;

	[Header("Settings")]
	[Header("Ball")]
	[SerializeField] float deformation;
	[SerializeField] bool xDeformation;
	[SerializeField] bool yDeformation;
	[Header("Coin")]
	[SerializeField] float temp;

	private void Update()
	{
		switch (isA)
		{
			case BallAnimationType.ball:
				BallAnimation();
				break;
			case BallAnimationType.coin:
				CoinAnimation();
				break;
		}
	}

	private void BallAnimation()
	{
		Vector3 scale = Vector3.one;

		if (xDeformation)
			scale.x = Mathf.Clamp(1 - deformation * Mathf.Abs(master.ball.linearVelocity.y / master.minimumVelocity.y), 1 - deformation, 1);
		if (yDeformation)
			scale.y = Mathf.Clamp(1 - deformation * Mathf.Abs(1 - (master.ball.linearVelocity.y / master.minimumVelocity.y)), 1 - deformation, 1);

		if (scale.x > scale.y)
		{
			scale.y = scale.y / scale.x;
			scale.x = 1;
		}
		else
		{
			scale.x = scale.x / scale.y;
			scale.y = 1;
		}

		ballScaler.localScale = scale;
	}

	private void CoinAnimation()
	{
		float rotation = master.ball.rotation;

		Vector3 scale = Vector3.one;

		scale.x = Mathf.Clamp(Mathf.Abs(Mathf.Cos(rotation * temp - temp)), 0.5f, 1f);
		scale.y = Mathf.Clamp(Mathf.Abs(Mathf.Sin(rotation * temp - temp)), 0.5f, 1f);

		ballScaler.localScale = scale;
	}
}

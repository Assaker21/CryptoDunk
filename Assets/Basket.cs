using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basket : MonoBehaviour
{
	[Header("Components")]
	[SerializeField] public Transform leftCollider;
	[SerializeField] public Transform rightCollider;
	[SerializeField] SpriteRenderer backRing;
	[SerializeField] SpriteRenderer frontRing;
	[SerializeField] ParticleSystem ringParticle;
	[SerializeField] ParticleSystem ThreeDotsParticles;

	[Header("Settings")]
	[SerializeField] float basketScaleAnimationTime;
	[SerializeField] AnimationCurve basketScaleAnimationCurve;
	[SerializeField] float basketMaxScale;

	public void OnBasketScoreSwish()
	{
		frontRing.color = Color.black;
		backRing.color = Color.black;
		frontRing.sortingOrder = 40;

		ringParticle.Play();
		ThreeDotsParticles.Play();
	}

	public void OnBasketScoreNormal()
	{
		frontRing.sortingOrder = 40;
		float startScale = transform.localScale.x;

		LeanTween.value(0f, 1f, basketScaleAnimationTime).setEase(basketScaleAnimationCurve).setOnUpdate((float value) =>
		{
			transform.localScale = new Vector3(startScale + (basketMaxScale - 1) * value, startScale + (basketMaxScale - 1) * value, startScale + (basketMaxScale - 1) * value);

			backRing.color = new Color(255, 255, 255, 1 - value);
			frontRing.color = new Color(255, 255, 255, 1 - value);
		});
	}

	public void OnBasketRespawn()
	{
		transform.localScale = Vector3.one;

		backRing.color = Color.white;
		frontRing.color = Color.white;
		frontRing.sortingOrder = 60;
	}
}

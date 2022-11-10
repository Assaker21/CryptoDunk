using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ScoreAnimation : MonoBehaviour
{
    [SerializeField] RectTransform otherRect;
    [SerializeField] RectTransform myRect;

	[SerializeField] float animationTime;
	[SerializeField] AnimationCurve curve;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.E))
		{
			Animate();		
		}
	}

	void Animate()
    {
		LeanTween.moveX(otherRect, 140, animationTime).setEase(curve);
		LeanTween.moveX(myRect, 520, animationTime).setEase(curve).setOnComplete(_Animate);
    }

	void _Animate()
	{
		LeanTween.moveX(myRect, 200, animationTime).setEase(curve);
		LeanTween.moveX(otherRect, -180, animationTime).setEase(curve).setOnComplete(__Animate);
	}

	void __Animate()
	{
		LeanTween.moveX(otherRect, -200, animationTime).setEaseOutSine().setOnComplete(___Animate);
	}

	void ___Animate()
	{
		float aspect = (float)Screen.width / (float)Screen.height;
		if (aspect > 9f / 16f)
		{
			otherRect.anchoredPosition = new Vector2(aspect * 1920 + 141, otherRect.anchoredPosition.y);
		}
		else
		{
			otherRect.anchoredPosition = new Vector2(1080 + 141, otherRect.anchoredPosition.y);
		}

		LeanTween.moveX(otherRect, 580, animationTime).setEase(curve);
	}
}

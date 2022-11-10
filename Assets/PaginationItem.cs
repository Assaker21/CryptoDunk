using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaginationItem : MonoBehaviour
{
	[Header("Components")]
	[SerializeField] Toggle[] toggles;
	[SerializeField] ScrollSnapCanvasManager scrollSnapCanvasManager;

	[Header("Settings")]
	[SerializeField] Color onColor;
	[SerializeField] Color offColor;
	[SerializeField] Vector3 onScale;
	[SerializeField] Vector3 offScale;
	[SerializeField] float animationTime;
	[SerializeField] AnimationCurve animationCurve;
	[SerializeField] int startingToggle;

	int lastToggle;

	private void Start()
	{
		lastToggle = startingToggle;
		scrollSnapCanvasManager.OnChangeToggle(startingToggle);
	}

	public void OnToggleValueChanged(int index)
	{
		RectTransform rect = toggles[index].transform.GetChild(0).GetComponent<RectTransform>();

		if (toggles[index].isOn)
		{
			LeanTween.color(rect, onColor, animationTime).setEase(animationCurve).setIgnoreTimeScale(true);
			LeanTween.scale(rect, onScale, animationTime).setEase(animationCurve).setIgnoreTimeScale(true);
			toggles[lastToggle].isOn = false;
			toggles[index].interactable = false;
			lastToggle = index;
			scrollSnapCanvasManager.OnChangeToggle(index);
		}
		else
		{
			LeanTween.color(rect, offColor, animationTime).setEase(animationCurve).setIgnoreTimeScale(true);
			LeanTween.scale(rect, offScale, animationTime).setEase(animationCurve).setIgnoreTimeScale(true);
			toggles[index].interactable = true;
		}
	}
}

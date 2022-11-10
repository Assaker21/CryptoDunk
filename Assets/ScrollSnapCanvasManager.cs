using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollSnapCanvasManager : MonoBehaviour
{
	[SerializeField] float animationTime;
	[SerializeField] Canvas[] canvases;

	public void OnChangeToggle(int activeToggle)
	{
		for (int i = 0; i < canvases.Length; i++)
		{
			if (activeToggle == i)
				canvases[i].enabled = true;
			else
				StartCoroutine(WaitBeforeSetInactive(animationTime, i));
		}
	}

	IEnumerator WaitBeforeSetInactive(float animationTime, int i)
	{
		yield return new WaitForSeconds(animationTime);

		canvases[i].enabled = false;
	}
}

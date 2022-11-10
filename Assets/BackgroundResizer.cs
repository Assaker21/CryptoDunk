using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundResizer : MonoBehaviour
{
	private void Start()
	{
		float width = Screen.width;
		float height = Screen.height;
		float verticalSize = Camera.main.orthographicSize * 2;

		transform.localScale = new Vector3((width / height) * verticalSize, verticalSize, 1);
	}

}

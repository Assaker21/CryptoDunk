using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PatternMovement : MonoBehaviour
{
    [SerializeField] RawImage image;
    [SerializeField] Vector2 velocity;

	private void Update()
	{
		image.uvRect = new Rect(image.uvRect.position + velocity * Time.deltaTime, image.uvRect.size);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartParticles : MonoBehaviour
{
	[Header("Components")]
	[SerializeField] GameObject particlePrefab;
	[SerializeField] Transform target;

	[Header("Settings")]
	[SerializeField] Vector2 gridUnitDimensions;

	private List<Transform> particles;
	private new Camera camera;

	private void Start()
	{
		camera = Camera.main;

		particles = new List<Transform>();

		Vector2 avg = Vector2.zero;
		int xCount = (int)(GetScreenWidth() / gridUnitDimensions.x + 2);
		int yCount = (int)(GetScreenHeight() / gridUnitDimensions.y + 2);

		for (int i = 0; i < xCount; i++)
		{
			for (int j = 0; j < yCount; j++)
			{
				Vector3 position = new Vector3(ScreenBorders.GetBottomLeftCorner(camera).x + i * gridUnitDimensions.x, ScreenBorders.GetBottomLeftCorner(camera).y + j * gridUnitDimensions.y, 0);

				particles.Add(Instantiate(particlePrefab, position, Quaternion.identity, transform).transform);

				avg.x += position.x;
				avg.y += position.y;
			}
		}

		avg.x /= xCount;
		avg.y /= yCount;

		Debug.Log(avg);

		for (int i = 0; i < particles.Count; i++)
		{
			//particles[i].position += (Vector3)avg / 2f;
		}
	}

	private void Update()
	{
		transform.position = new Vector3(target.position.x, transform.position.y, 0);
	}

	float GetScreenWidth()
	{
		float aspect = (float)Screen.width / (float)Screen.height;
		return GetScreenHeight() * aspect;
	}

	float GetScreenHeight()
	{
		return 10f;
	}
}

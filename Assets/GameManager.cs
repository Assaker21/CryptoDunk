using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	[SerializeField] GameObject[] gameGameObjects;
	[SerializeField] GameObject[] mainmenuGameObjects;
	public void StartGame()
	{
		for (int i = 0; i < gameGameObjects.Length; i++)
		{
			gameGameObjects[i].SetActive(true);
		}
		for (int i = 0; i < mainmenuGameObjects.Length; i++)
		{
			mainmenuGameObjects[i].SetActive(false);
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceivedOnFrameOne
{
	public ParticularGameSettings particularGameSettings;
	public GameSettings generalGameSettings;
}

[System.Serializable]
public struct GameSettings
{
	public bool in_use;
	public float distanceBetweenBaskets;
	public float distanceToFirstBasket;
	public float basketVerticalSpawnRange;
	public float basketWidth;
	public float basketCollidersRadius;
	public SimpleVector2 rotationRange;
	public int firstRotatedBasket;
	public float basketWidthMaxRegression;
	public SimpleVector2 linearAcceleration;
	public SimpleVector2 linearDrag;
	public float angularDrag;
	public SimpleVector2 maximumVelocity;
	public SimpleVector2 minimumVelocity;
	public float friction;
	public float bounciness;
	public float jumpPower;
	public float radius;
	public float jumpErrorRange;
	public int concurrentBasketsAmount;
}

[System.Serializable]
public struct ParticularGameSettings {
	public string token;
	public int randomA;
	public int randomB;
}

[System.Serializable]
public struct SimpleVector2 {
	public float x;
	public float y;
}

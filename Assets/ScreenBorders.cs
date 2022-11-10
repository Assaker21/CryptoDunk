using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScreenBorders
{
	public static Vector2[] GetScreenBorders(Camera camera) 
    {
        Vector2 upperLeft = camera.ScreenToWorldPoint(new Vector2(0, Screen.height));
        Vector2 upperRight = camera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        Vector2 lowerLeft = camera.ScreenToWorldPoint(new Vector2(0, 0));
        Vector2 lowerRight = camera.ScreenToWorldPoint(new Vector2(Screen.width, 0));

        return new Vector2[] { upperLeft, upperRight, lowerLeft, lowerRight };
    }

    public static Vector2 GetBottomLeftCorner(Camera camera)
    { 
        return camera.ScreenToWorldPoint(new Vector2(0, 0));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceivedOnFramePacket
{
    public SimpleBall ball;
    public SimpleBasket[] baskets;
    public int score;
    public int swishScore;
    public Message message;
}

[System.Serializable]
public struct SimpleBall {
    public SimpleVector2 position;
}

[System.Serializable]
public struct SimpleBasket {
    public SimpleVector2 position;
    public float rotation;
    public float width;
}

[System.Serializable]
public struct Message {
    public string message;
    public string messageType;
}

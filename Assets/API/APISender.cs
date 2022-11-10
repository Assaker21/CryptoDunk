using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class APISender : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText;
    [SerializeField] Transform ball;
    [SerializeField] Transform[] baskets;

    List<List<PacketFrame>> queue;
    List<List<PacketFrame>> sentPackets;

    public int maxPacketsPerSend;
    public string connectionToken;
    bool isRunning;

    public void StartSending()
    {
        isRunning = false;
        StartCoroutine(Update60FramesWWW());
    }

    public void AddToQueue(string _connectionToken, List<PacketFrame> packet)
    {
        if (queue == null)
            queue = new List<List<PacketFrame>>();

        queue.Add(packet);

        connectionToken = _connectionToken;
    }

    IEnumerator Update60FramesWWW()
    {
        while (true)
        {
            if (queue != null && queue.Count != 0 && !isRunning)
            {
                isRunning = true;

                WWWForm form = new WWWForm();
                form.AddField("token", connectionToken);

                int frameCount = 0;
                sentPackets = new List<List<PacketFrame>>();

                for (int i = 0; i < Mathf.Min(maxPacketsPerSend, queue.Count); i++)
                {
                    sentPackets.Add(queue[0]);
                    queue.RemoveAt(0);
                }


                for (int i = 0; i < sentPackets.Count; i++)
                {
                    for (int j = 0; j < sentPackets[i].Count; j++)
                    {
                        form.AddField("frames", sentPackets[i][j].frame.ToString());
                        form.AddField("deltaTimes", sentPackets[i][j].delta.ToString());
                        form.AddField("times", sentPackets[i][j].time.ToString());
                        form.AddField("jumpErrors", sentPackets[i][j].jumpError.ToString());
                        Debug.Log("--------------" + sentPackets[i][j].jumpError.ToString());
                        form.AddField("screenWidth", Mathf.Abs(2 * sentPackets[i][j].screenLeft).ToString());
                        frameCount++;
                    }
                }

                sentPackets = null;

                using (UnityWebRequest www = UnityWebRequest.Post("http://localhost:3000/UpdateFrames", form))
                {
                    www.downloadHandler = new DownloadHandlerBuffer();

                    yield return www.SendWebRequest();

                    if (www.result != UnityWebRequest.Result.Success)
                    {
                        Debug.Log(www.error);
                    }
                    else
                    {
                        ReceivedOnFramePacket rec = JsonUtility.FromJson<ReceivedOnFramePacket>(www.downloadHandler.text);
                        Debug.Log("Response: " + rec.message.message + "  Frames sent: " + frameCount);
                        SetAPIModelsPositions(new Vector2(rec.ball.position.x, rec.ball.position.y), rec.baskets, rec.score);

                    }
                    isRunning = false;
                }
            }
            else
            {
                yield return null;
            }
        }
    }

    public void SetAPIModelsPositions(Vector2 ballPosition, SimpleBasket[] _baskets, int score)
    {
        ball.position = ballPosition;
        scoreText.text = "Score: " + score.ToString();

        for (int i = 0; i < baskets.Length; i++)
        {
            baskets[i].position = new Vector2(_baskets[i].position.x, _baskets[i].position.y);
            baskets[i].rotation = Quaternion.Euler(0, 0, _baskets[i].rotation);
            baskets[i].localScale = new Vector3(_baskets[i].width, baskets[i].localScale.y, 1);

        }
    }
}

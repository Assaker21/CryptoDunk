using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Networking;
using TMPro;
using System;

public class Master : MonoBehaviour
{
    [SerializeField] Transform ballPositionModel;
    [SerializeField] Transform ballRotationModel;
    [SerializeField] BasketManager basketManager;
    [SerializeField] EventManager eventManager;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] APISender APISender; 

    private int frame = 0;
    public Ball ball = new Ball();
    private Basket[] baskets;
    private new Camera camera;
    private bool jump;
    private int score;
    private int swishScore;
    public int[] randoms;
    private bool wasOverBasket;

    private string connectionToken;
    private int counter;
    private bool startRunningUpdates;
    private bool isRunning;

    [Header("Basket Management")]
    [SerializeField] float distanceBetweenBaskets;
    [SerializeField] float distanceToFirstBasket;
    [SerializeField] float basketVerticalSpawnRange;
    [SerializeField] float basketWidth;
    [SerializeField] float basketCollidersRadius;
    [SerializeField] Vector2 rotationRange;
    [SerializeField] int firstRotatedBasket;
    [SerializeField] float basketWidthMaxRegression;
    //[SerializeField] float basketAngularDeformation;

    [Header("Player")]
    [SerializeField] Vector2 linearAcceleration;
    [SerializeField] Vector2 linearDrag;
    [SerializeField] float angularDrag;
    [SerializeField] public Vector2 maximumVelocity;
    [SerializeField] public Vector2 minimumVelocity;
    [SerializeField] float friction;
    [SerializeField] float bounciness;
    [SerializeField] float jumpPower;
    [SerializeField] float radius;
    [SerializeField] float jumpErrorRange;


    List<PacketFrame> nextPacket;

    public class Ball
    {
        public Vector2 position;
        public float rotation;

        public Vector2 linearAcceleration;
        public float angularAcceleration;

        public Vector2 linearVelocity;
        public float angularVelocity;

        public CircleCollider collider = new CircleCollider();

        public bool isColliding;
    }

    public class CircleCollider
    {
        public Vector2 position;
        public float radius;
        public CircleCollider()
        {

        }

        public CircleCollider(Vector2 _position, float _radius)
        {
            position = _position;
            radius = _radius;
        }
    }

    public class Basket
    {
        public Vector2 position;
        public float rotation;

        public CircleCollider leftCollider;
        public CircleCollider rightCollider;

        public float width;

        public bool hitCollider;
        public bool wentThrough;

        public Basket()
        {
            position = new Vector2(0, 0);
            width = 0;
            leftCollider = new CircleCollider(Vector2.zero, 0);
            rightCollider = new CircleCollider(Vector2.zero, 0);
        }

        public void RecalculateColliderPositions()
        {
            leftCollider.position.x = -(width / 2f) * Mathf.Cos(rotation * 3.1415f / 180f);
            leftCollider.position.y = -(width / 2f) * Mathf.Sin(rotation * 3.1415f / 180f);

            rightCollider.position = -leftCollider.position;
        }
    }



    private void FrameZero(float screenRight)
    {
        randoms = new int[123];
        for (int i = 0; i < randoms.Length; i++)
        {
            randoms[i] = GenerateNextRandom();
        }

        baskets = new Basket[4];
        for (int i = 0; i < baskets.Length; i++)
        {
            float randomFormulaValue = RandomFormula();

            baskets[i] = new Basket();
            baskets[i].position = new Vector2(screenRight + i * distanceBetweenBaskets + distanceToFirstBasket, randomFormulaValue * basketVerticalSpawnRange - basketVerticalSpawnRange / 2f);
            baskets[i].width = basketWidth - BasketWidthRegressionAmount();
            baskets[i].leftCollider = new CircleCollider(new Vector2(-basketWidth / 2, 0), basketCollidersRadius);
            baskets[i].rightCollider = new CircleCollider(new Vector2(basketWidth / 2, 0), basketCollidersRadius);

            if (n >= firstRotatedBasket && n % 3 == 0)
            {
                float rotationFill = randomFormulaValue + 0.5f;
                if (rotationFill > 1)
                    rotationFill = rotationFill - 1;

                baskets[i].rotation = Mathf.Abs(rotationRange.y - rotationRange.x) * rotationFill + rotationRange.x;
            }
            else baskets[i].rotation = 0;

            baskets[i].RecalculateColliderPositions();

            baskets[i].hitCollider = false;
            baskets[i].wentThrough = false;

            eventManager.OnBasketRespawn(i);
        }

        ball.collider.radius = radius;

        nextPacket = new List<PacketFrame>();
        startRunningUpdates = true;
    }

    private void UpdateOneFrame(bool jump, float screenLeft)
    {
        // Updating ball velocity
        ball.linearVelocity.y += linearAcceleration.y * Time.fixedDeltaTime;
        ball.linearVelocity.x += linearAcceleration.x * Time.fixedDeltaTime;

        float jumpError = 0;

        if (jump)
        {
            jumpError = UnityEngine.Random.Range(-jumpErrorRange / 2f, jumpErrorRange / 2f);
            ball.linearVelocity.y = jumpPower * (1 + jumpError);
            eventManager.OnJump();
        }

        // Applying ball linear velocity constraints
        if (ball.linearVelocity.y > maximumVelocity.y)
        {
            ball.linearVelocity.y = maximumVelocity.y;
        }
        else if (ball.linearVelocity.y < minimumVelocity.y)
        {
            ball.linearVelocity.y = minimumVelocity.y;
        }

        if (ball.linearVelocity.x > maximumVelocity.x)
        {
            ball.linearVelocity.x = maximumVelocity.x;
        }
        else if (ball.linearVelocity.x < minimumVelocity.x)
        {
            ball.linearVelocity.x = minimumVelocity.x;
        }

        // Applying ball linear drag
        ball.linearVelocity.y = ball.linearVelocity.y * (1 - Time.fixedDeltaTime * linearDrag.y);
        ball.linearVelocity.x = ball.linearVelocity.x * (1 - Time.fixedDeltaTime * linearDrag.x);

        // Updating ball position
        ball.position.x += ball.linearVelocity.x * Time.fixedDeltaTime;
        ball.position.y += ball.linearVelocity.y * Time.fixedDeltaTime;

        // Applying ball angular drag
        ball.angularVelocity = ball.angularVelocity * (1 - Time.fixedDeltaTime * angularDrag);

        // Updating ball rotation
        ball.rotation += ball.angularVelocity * Time.fixedDeltaTime;

        // Updating basket states
        for (int i = 0; i < baskets.Length; i++)
        {
            if (baskets[i].position.x <= screenLeft - 2f)
            {
                int k = 0;
                for (int j = 1; j < baskets.Length; j++)
                {
                    if (baskets[j].position.x > baskets[k].position.x)
                    {
                        k = j;
                    }
                }

                float randomFormulaValue = RandomFormula();

                baskets[i].position = new Vector2(baskets[k].position.x + distanceBetweenBaskets, randomFormulaValue * basketVerticalSpawnRange - basketVerticalSpawnRange / 2f);

                if (n >= firstRotatedBasket && n % 3 == 0)
                {
                    float rotationFill = randomFormulaValue + 0.5f;
                    if (rotationFill > 1)
                        rotationFill = rotationFill - 1;

                    baskets[i].rotation = Mathf.Abs(rotationRange.y - rotationRange.x) * rotationFill + rotationRange.x;
                }
                else baskets[i].rotation = 0;

                baskets[i].width = basketWidth - BasketWidthRegressionAmount();

                baskets[i].RecalculateColliderPositions();

                baskets[i].hitCollider = false;
                baskets[i].wentThrough = false;

                eventManager.OnBasketRespawn(i);
            }
        }

        // Collision Handling
        Vector2[] sqrDistances = new Vector2[baskets.Length];
        for (int i = 0; i < baskets.Length; i++)
        {
            if (baskets[i].wentThrough) continue;

            Vector2 ldirection = ball.position - (baskets[i].position + baskets[i].leftCollider.position);
            //Debug.Log(ldirection + ", Frame: " + frame);
            Vector2 rdirection = ball.position - (baskets[i].position + baskets[i].rightCollider.position);

            sqrDistances[i] = new Vector2(ldirection.x * ldirection.x + ldirection.y * ldirection.y, rdirection.x * rdirection.x + rdirection.y * rdirection.y);


            //Debug.Log("sqrDistances[i].x: " + sqrDistances[i].x + ", otherSide: " + Square(ball.collider.radius + baskets[i].leftCollider.radius) + ", Frame: " + frame);
            //Debug.Log(ball.collider.radius + ", " + baskets[i].leftCollider.radius);
            if (sqrDistances[i].x < Square(ball.collider.radius + baskets[i].leftCollider.radius))
            {
                //Debug.Log("BREAKTHROUGH---------------------------------------<<<<<");
                //float verticalFallPowerFactor = ball.linearVelocity.y / minimumVelocity.y;

                eventManager.OnHit();
                if (!baskets[i].wentThrough) eventManager.OnHitUnscoredBasket();

                float _distance = Mathf.Sqrt(sqrDistances[i].x);
                ldirection.x /= _distance;
                ldirection.y /= _distance;

                //Debug.Log("--------" + (ldirection.x * ldirection.x + ldirection.y * ldirection.y));

                ball.position = ldirection * (ball.collider.radius + baskets[i].leftCollider.radius) + baskets[i].position + baskets[i].leftCollider.position;

                if (ball.position.y < baskets[i].position.y)
                {
                    ball.linearVelocity += ldirection * bounciness * Dot(-ball.linearVelocity, ldirection);
                }
                else
                {
                    ball.linearVelocity += ldirection * bounciness * Dot(-ball.linearVelocity, ldirection);
                }

                ball.angularVelocity += Dot(ball.linearVelocity, new Vector2(-ldirection.y, ldirection.x)) * friction;

                baskets[i].hitCollider = true;

                /*if (verticalFallPowerFactor > 0 && ball.position.x < baskets[i].leftCollider.position.x + baskets[i].position.x + baskets[i].leftCollider.radius && ball.position.x > baskets[i].leftCollider.position.x + baskets[i].position.x - baskets[i].leftCollider.radius)
                {
                    baskets[i].rotation += verticalFallPowerFactor * basketAngularDeformation;
                    baskets[i].RecalculateColliderPositions();
                }*/
            }
            else if (sqrDistances[i].y < Square(ball.collider.radius + baskets[i].rightCollider.radius))
            {
                //float verticalFallPowerFactor = ball.linearVelocity.y / minimumVelocity.y;

                eventManager.OnHit();
                if (!baskets[i].wentThrough) eventManager.OnHitUnscoredBasket();

                float _distance = Mathf.Sqrt(sqrDistances[i].y);
                rdirection.x /= _distance;
                rdirection.y /= _distance;

                ball.position = rdirection * (ball.collider.radius + baskets[i].rightCollider.radius) + baskets[i].position + baskets[i].rightCollider.position;

                if (ball.position.y < baskets[i].position.y)
                {
                    ball.linearVelocity += rdirection * bounciness * Dot(-ball.linearVelocity, rdirection);
                }
                else
                {
                    ball.linearVelocity += rdirection * bounciness * Dot(-ball.linearVelocity, rdirection);
                }

                ball.angularVelocity += Dot(ball.linearVelocity, new Vector2(-rdirection.y, rdirection.x)) * friction;

                baskets[i].hitCollider = true;

                /*if (verticalFallPowerFactor > 0 && ball.position.x < baskets[i].rightCollider.position.x + baskets[i].position.x + baskets[i].rightCollider.radius && ball.position.x > baskets[i].rightCollider.position.x + baskets[i].position.x - baskets[i].rightCollider.radius)
                {
                    baskets[i].rotation -= verticalFallPowerFactor * basketAngularDeformation;
                    baskets[i].RecalculateColliderPositions();
                }*/
            }
        }

        // Scoring Handling
        for (int i = 0; i < baskets.Length; i++)
        {
            if (!baskets[i].wentThrough)
            {
                if ((baskets[i].position.x + baskets[i].width / 2f) < (ball.position.x - ball.collider.radius))
                {
                    eventManager.OnMiss();
                }
                else if (ball.position.x < baskets[i].position.x + basketWidth / 2f && ball.position.x > baskets[i].position.x - baskets[i].width / 2f)
                {
                    if (ball.position.y > baskets[i].leftCollider.position.y + baskets[i].position.y)
                        wasOverBasket = true;
                    else if (ball.position.y < baskets[i].leftCollider.position.y + baskets[i].position.y - 0.5f)
                        wasOverBasket = false;
                    else
                    {
                        if (wasOverBasket)
                        {
                            baskets[i].wentThrough = true;
                            wasOverBasket = false;
                            if (baskets[i].hitCollider)
                            {
                                // Normal score
                                eventManager.OnScore();
                                swishScore = 0;
                                eventManager.OnBasketScoreNormal(i);
                                score++;
                            }
                            else
                            {
                                // Swish score
                                swishScore++;
                                score += Mathf.Clamp(swishScore, int.MinValue, 5);
                                eventManager.OnBasketScoreSwish(i);
                                switch (swishScore)
                                {
                                    case 1:
                                        eventManager.OnSwishScore();
                                        break;
                                    case 2:
                                        eventManager.OnSwishScoreTwice();
                                        break;
                                    case 3:
                                        eventManager.OnSwishScoreThrice();
                                        break;
                                    default:
                                        eventManager.OnSwishScoreEndless();
                                        break;
                                }
                            }
                        }
                    }
                }
            }
        }

        // Packet creation - not on server
        PacketFrame packetFrame = new PacketFrame();

        packetFrame.delta = Time.fixedDeltaTime;
        if (jump) packetFrame.jumpError = jumpError;
        else packetFrame.jumpError = 0;
        packetFrame.frame = frame;
        packetFrame.time = DateTime.Now.ToString();
        packetFrame.screenLeft = screenLeft;

        nextPacket.Add(packetFrame);
    }

    public void JumpInput()
    {
        jump = true;
    }


    IEnumerator FrameZeroWWW()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", "charbel");
        form.AddField("password", "assaker");
        form.AddField("screenWidth", Mathf.Abs(ScreenBorders.GetBottomLeftCorner(camera).x * 2).ToString());

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost:3000/StartGame", form))
        {
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                ReceivedOnFrameOne rec = JsonUtility.FromJson<ReceivedOnFrameOne>(www.downloadHandler.text);
                Debug.Log("Response:" + rec.particularGameSettings.token);

                connectionToken = rec.particularGameSettings.token;
                randomA = rec.particularGameSettings.randomA;
                randomB = rec.particularGameSettings.randomB;
                score = 0;

                distanceBetweenBaskets = rec.generalGameSettings.distanceBetweenBaskets;
                distanceToFirstBasket = rec.generalGameSettings.distanceToFirstBasket;
                basketVerticalSpawnRange = rec.generalGameSettings.basketVerticalSpawnRange;
                basketWidth = rec.generalGameSettings.basketWidth;
                basketCollidersRadius = rec.generalGameSettings.basketCollidersRadius;
                rotationRange = new Vector2(rec.generalGameSettings.rotationRange.x, rec.generalGameSettings.rotationRange.y);
                firstRotatedBasket = rec.generalGameSettings.firstRotatedBasket;
                basketWidthMaxRegression = rec.generalGameSettings.basketWidthMaxRegression;
                linearAcceleration = new Vector2(rec.generalGameSettings.linearAcceleration.x, rec.generalGameSettings.linearAcceleration.y);
                linearDrag = new Vector2(rec.generalGameSettings.linearDrag.x, rec.generalGameSettings.linearDrag.y);
                angularDrag = rec.generalGameSettings.angularDrag;
                maximumVelocity = new Vector2(rec.generalGameSettings.maximumVelocity.x, rec.generalGameSettings.maximumVelocity.y);
                minimumVelocity = new Vector2(rec.generalGameSettings.minimumVelocity.x, rec.generalGameSettings.minimumVelocity.y);
                friction = rec.generalGameSettings.friction;
                bounciness = rec.generalGameSettings.bounciness;
                jumpPower = rec.generalGameSettings.jumpPower;
                radius = rec.generalGameSettings.radius;
                jumpErrorRange = rec.generalGameSettings.jumpErrorRange;
                Debug.Log(rec.generalGameSettings.jumpErrorRange);

                FrameZero(-ScreenBorders.GetBottomLeftCorner(camera).x);

                APISender.StartSending();
            }
        }
    }

    private void Awake()
    {
        camera = Camera.main;
        startRunningUpdates = false;
        StartCoroutine(FrameZeroWWW());
    }

	
    void FixedUpdate()
    {
        if (!startRunningUpdates) return;

        UpdateOneFrame(jump, ScreenBorders.GetBottomLeftCorner(camera).x);

        if (jump) jump = false;

        frame++;
        counter++;

        if (counter >= 5)
        {
            APISender.AddToQueue(connectionToken, nextPacket);
            nextPacket = new List<PacketFrame>();
            counter = 0;
        }

        ballPositionModel.position = ball.position;
        ballRotationModel.rotation = Quaternion.Euler(0, 0, ball.rotation);

        for (int i = 0; i < baskets.Length; i++)
        {
            basketManager.baskets[i].transform.position = baskets[i].position;
            basketManager.baskets[i].transform.rotation = Quaternion.Euler(0, 0, baskets[i].rotation);
            if(!baskets[i].wentThrough)
                basketManager.baskets[i].transform.localScale = Vector3.one * (1 - basketWidth + baskets[i].width);
        }

        scoreText.text = score.ToString();
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            JumpInput();
        }
    }
    

	// Random
	private int randomSeed = 1, randomA = 11, randomB = 6;
	int GenerateNextRandom()
    {
		randomSeed = (randomSeed * randomA + randomB) % 100;
        return randomSeed;
    }

    int n = 0;
    float RandomFormula()
    {
        n++;

        if (n - 1 >= randoms.Length) n = 1;

        return (float)randoms[n - 1] / 100f;
    }

    float Square(float value)
    {
        return value * value;
    }

    float Dot(Vector2 vector1, Vector2 vector2) 
    {
        return vector1.x * vector2.x + vector1.y * vector2.y;
    }

    float BasketWidthRegressionAmount()
    {
        if (n <= 0 || (-7f / n) + 1 < 0)
            return 0;

        return ((-7f / n) + 1) * basketWidthMaxRegression;
    }

    float RunFrames(int frameCount)
    {
        DateTime before = DateTime.Now;

        for (int i = 0; i < frameCount; i++)
        {
            UpdateOneFrame(false, ScreenBorders.GetBottomLeftCorner(camera).x);
        }

        DateTime after = DateTime.Now;
        TimeSpan duration = after.Subtract(before);
        Debug.Log("Duration in milliseconds: " + duration.Milliseconds);
        return duration.Milliseconds;
    }
}

public struct PacketFrame
{
    public int frame;
    public float delta;
    public float jumpError;
    public string time;
    public float screenLeft;
}
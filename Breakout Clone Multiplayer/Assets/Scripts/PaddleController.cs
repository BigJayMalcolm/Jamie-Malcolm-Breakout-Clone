using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;

public class PaddleController : NetworkBehaviour
{
    [Header("References")]
    [Tooltip("The GameObject of the temporary ball to display before it's launched.")] public GameObject FakeBall;
    [Tooltip("The ball prefab that is launched.")] public GameObject BallPrefab;

    [Header("Data")]
    [HideInInspector] [SyncVar] public int Player = 0;
    [Tooltip("The force at which to launch the ball.")] public float Force = 1.5f;

    private Vector2 _rawMouseInput;
    private float fakeBallScale = 0.1538461f;

    #region NetworkBehaviour Methods

    private void Update()
    {
        if (!isLocalPlayer) return;

        HandlePaddleMovement();
    }

    private void LateUpdate()
    {
        if (BallActive())
        {
            FakeBall.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
            Cursor.visible = false;
        }
        else
        {
            FakeBall.transform.localScale = new Vector3(fakeBallScale, 1.0f, 1.0f);
            Cursor.visible = true;
        }
    }

    #endregion

    #region Private Methods

    void HandlePaddleMovement()
    {
        float movement = Camera.main.ScreenToWorldPoint(_rawMouseInput).x;
        movement = Mathf.Clamp(movement, -16, 16);
        transform.position = new Vector3(movement, transform.position.y);
    }

    void LaunchBall()
    {
        if (!BallActive())
        {
            FakeBall.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
            Launch(Player);
        }
    }

    bool BallActive()
    {
        bool returnValue = false;

        if (Player == 1 && GameObject.FindWithTag("GameController").GetComponent<GameController>().BallOneActive == 1)
        {
            returnValue = true;
        }
        else if (Player == 2 && GameObject.FindWithTag("GameController").GetComponent<GameController>().BallTwoActive == 1)
        {
            returnValue = true;
        }

        return returnValue;
    }

    [Command]
    void Launch(int player)
    {
        GameObject ball = Instantiate(BallPrefab);
        ball.transform.position = FakeBall.transform.position;
        ball.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        ball.GetComponent<Ball>().Owner += player;

        NetworkServer.Spawn(ball);

        ball.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-Force, Force), Force, 0.0f), ForceMode.Impulse);

        if (player == 1)
        {
            GameObject.FindWithTag("GameController").GetComponent<GameController>().BallOneActive++;
        }
        else if (player == 2)
        {
            GameObject.FindWithTag("GameController").GetComponent<GameController>().BallTwoActive++;
        }
    }

    #endregion

    #region Input Handlers

    void OnMove(InputValue value)
    {
        if (!isLocalPlayer) return;

        _rawMouseInput = value.Get<Vector2>();
    }

    void OnLaunch()
    {
        if (!isLocalPlayer) return;

        LaunchBall();
    }

    #endregion
}
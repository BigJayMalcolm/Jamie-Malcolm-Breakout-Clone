using UnityEngine;
using UnityEngine.InputSystem;

public class PaddleController : MonoBehaviour
{
    [Header("References")]
    public GameObject Ball;

    [Header("Data")]
    public float Force = 1.5f;

    [Header("Sounds")]
    public AudioSource LaunchSound;

    [HideInInspector] public bool BallAttached = true;

    private GameObject _currentBall;
    private Vector2 _rawMouseInput;

    #region MonoBehaviour Methods

    private void LateUpdate()
    {
        HandlePaddleMovement();
    }

    #endregion

    #region Private Methods

    void HandlePaddleMovement()
    {
        float movement = Camera.main.ScreenToWorldPoint(_rawMouseInput).x;
        movement = Mathf.Clamp(movement, -16, 16);
        transform.position = new Vector3(movement, -5.0f);
    }

    void LaunchBall()
    {
        if (BallAttached)
        {
            Ball.SetActive(false);

            _currentBall = Instantiate(Ball);
            _currentBall.SetActive(true);
            _currentBall.transform.position = Ball.transform.position;
            _currentBall.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            _currentBall.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-Force, Force), Force, 0.0f), ForceMode.Impulse);

            LaunchSound.Play();

            BallAttached = false;

            Cursor.visible = false;
        }
    }

    #endregion

    #region Public Methods

    public void OutOfBounds()
    {
        Destroy(_currentBall);
        _currentBall = null;
        Ball.SetActive(true);
        BallAttached = true;

        Cursor.visible = true;
    }

    #endregion

    #region Input Handlers

    void OnMove(InputValue value)
    {
        _rawMouseInput = value.Get<Vector2>();
    }

    void OnLaunch()
    {
         LaunchBall();
    }

    #endregion
}
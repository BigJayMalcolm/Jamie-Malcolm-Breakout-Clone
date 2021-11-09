using UnityEngine;

public class Ball : MonoBehaviour
{
    [Header("Sounds")]
    public AudioSource BrickDestroyedSound;
    public AudioSource DeathSound;

    void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Brick":
            {
                collision.gameObject.SetActive(false);

                GameObject.FindWithTag("GameController").GetComponent<GameController>().Score += 100;

                BrickDestroyedSound.Play();

                break;
            }
            case "DeadZone":
            {
                GameObject.FindWithTag("Paddle").GetComponent<PaddleController>().OutOfBounds();

                DeathSound.Play();

                break;
            }
        }
    }
}
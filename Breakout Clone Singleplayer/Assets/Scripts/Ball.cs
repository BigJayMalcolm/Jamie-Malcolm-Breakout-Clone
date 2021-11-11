using UnityEngine;

public class Ball : MonoBehaviour
{
    [Header("Sounds")]
    [Tooltip("The sound to play when a brick is destroyed.")] public AudioSource BrickDestroyedSound;
    [Tooltip("The sound to play when a ball is lost.")] public AudioSource DeathSound;

    #region NetworkBehaviour Callbacks

    void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Brick":
            {
                // Disable the brick when it is hit
                collision.gameObject.SetActive(false);

                // Increase the palyer's score
                GameObject.FindWithTag("GameController").GetComponent<GameController>().Score += 100;

                // Firstly, check that the sound isn't null and then play the sound
                if (BrickDestroyedSound != null) BrickDestroyedSound.Play();

                break;
            }
            case "DeadZone":
            {
                // Run a function which resets the player's ball
                GameObject.FindWithTag("Paddle").GetComponent<PaddleController>().OutOfBounds();

                // Firstly, check that the sound isn't null and then play the sound
                if (DeathSound != null) DeathSound.Play();

                break;
            }
        }
    }

    #endregion
}
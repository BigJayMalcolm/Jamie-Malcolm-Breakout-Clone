using UnityEngine;
using Mirror;

public class Ball : NetworkBehaviour
{
    [Header("Sounds")]
    [Tooltip("The sound to play when a brick is destroyed.")] public AudioSource BrickDestroyedSound;

    [HideInInspector] [SyncVar] public int Owner = 0;

    #region NetworkBehaviour Methods

    private void Update()
    {
        // If the ball ends up bouncing along an axis, add a bit of force to it so it doesn't get stuck
        if ((GetComponent<Rigidbody>().velocity.x == 0.0f && GetComponent<Rigidbody>().velocity.y != 0.0f) || (GetComponent<Rigidbody>().velocity.x != 0.0f && GetComponent<Rigidbody>().velocity.y == 0.0f))
        {
            GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0.0f), ForceMode.Impulse);
        }
    }

    #endregion

    #region NetworkBehaviour Callbacks

    void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Brick":
            {
                // Destroy the brick for all clients
                NetworkServer.Destroy(collision.gameObject);

                // Update the score which is a SyncVar in the GameController
                GameObject.FindWithTag("GameController").GetComponent<GameController>().Score += 100;

                // Firstly, check that the sound isn't null and then play the sound
                // Sounds are not synched over clients currently, but this is something that could be done
                if (BrickDestroyedSound != null) BrickDestroyedSound.Play();
                
                break;
            }
            case "DeadZone":
            {
                // Depending on which player owns the ball, a SyncVar is changed so that the correct paddle is notified
                if (Owner == 1) { GameObject.FindWithTag("GameController").GetComponent<GameController>().BallOneActive--; }
                else if (Owner == 2) { GameObject.FindWithTag("GameController").GetComponent<GameController>().BallTwoActive--; }

                // Destroy the ball for all clients
                NetworkServer.Destroy(gameObject);

                break;
            }
        }
    }

    #endregion
}
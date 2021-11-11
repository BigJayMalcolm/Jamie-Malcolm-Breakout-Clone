using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [Tooltip("The TMP_Text from the UI that represents the score value.")] public TMP_Text ScoreValue;
    [HideInInspector] public int Score = 0;

    #region NetworkBehaviour Methods

    void Update()
    {
        // Keep the displayed score value equal to that of the actual value
        ScoreValue.text = Score.ToString();

        // Reload the scene if all of the bricks have been collected
        if (Score == 5000)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    #endregion
}
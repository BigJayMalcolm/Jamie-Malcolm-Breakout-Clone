using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public TMP_Text ScoreValue;
    [HideInInspector] public int Score = 0;

    void Update()
    {
        ScoreValue.text = Score.ToString();

        // Reload the scene if all of the bricks have been collected
        if (Score == 5000)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
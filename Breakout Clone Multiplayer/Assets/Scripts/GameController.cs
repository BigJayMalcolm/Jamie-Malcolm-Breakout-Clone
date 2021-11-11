using TMPro;
using Mirror;
using UnityEngine;

public class GameController : NetworkBehaviour
{
    [Tooltip("The TMP_Text from the UI that represents the score value.")] public TMP_Text ScoreValue;
    [Tooltip("The TMP_Text from the UI that represents the level completed text.")] public TMP_Text LevelCompleted;
    [HideInInspector] [SyncVar] public int Score;
    [HideInInspector] [SyncVar] public int BallOneActive = 0;
    [HideInInspector] [SyncVar] public int BallTwoActive = 0;

    #region NetworkBehaviour Methods

    void Update()
    {
        // Keep the displayed score value equal to that of the actual value
        ScoreValue.text = Score.ToString();

        // Display a message instructing the players how to play again
        if (Score == 5000) { LevelCompleted.text = "Level Completed!\n\nRestart the server to play again."; }
    }

    #endregion
}
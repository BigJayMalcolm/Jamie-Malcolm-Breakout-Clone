using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkManagerBreakout : NetworkManager
{
    [Tooltip("The spawn point of player one.")] public Transform PlayerOneSpawn;
    [Tooltip("The spawn point of player two.")] public Transform PlayerTwoSpawn;

    #region NetworkManager Overrides

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        Transform start = numPlayers == 0 ? PlayerOneSpawn : PlayerTwoSpawn;
        GameObject player = Instantiate(playerPrefab, start.position, start.rotation);
        player.GetComponent<PaddleController>().Player = numPlayers == 0 ? 1 : 2;
        NetworkServer.AddPlayerForConnection(conn, player);
    }

    public override void OnStopHost()
    {
        base.OnStopHost();

        // When a host ends the game, restart the scene
        singleton.ServerChangeScene(SceneManager.GetActiveScene().name);
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);

        // When a client leaves the game, restart their scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    #endregion
}
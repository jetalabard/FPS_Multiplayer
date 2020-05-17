using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public MatchSettings MatchSettings;

    [SerializeField]
    private GameObject _sceneCamera;

    public delegate void PlayerKilledCallback(string player);
    public PlayerKilledCallback OnPlayerKilledCallback;

    public delegate void PlayerConnectedCallback(string player);
    public PlayerConnectedCallback OnPlayerConnectedCallback;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More one object GameManager in scene");
        }
        else
        {
            Instance = this;
        }
    }

    public void SetSceneCameraActive(bool isActive)
    {
        if (_sceneCamera == null)
        {
            return;
        }
        _sceneCamera.SetActive(isActive);
    }

    #region Player Tracking
    private const string PlayerIdPrefix = "Player ";

    private static Dictionary<string, Player> _players = new Dictionary<string, Player>();

    private static Dictionary<string, string> _playersName = new Dictionary<string, string>();

    public static string LocalPlayerName { get; set; }

    public static void RegisterPlayer(string netId, Player player, string name)
    {
        string playerId = PlayerIdPrefix + netId;
        _players.Add(playerId, player);
        _playersName.Add(playerId, name);
        player.UserName = name;
        player.transform.name = playerId;
    }

    internal static void UnRegisterPlayer(string id)
    {
        _playersName.Remove(id);
        _players.Remove(id);
    }

    public static Player GetPlayer(string playerId)
    {
        return _players[playerId];
    }
    public static string GetPlayerName(string playerId)
    {
        return _playersName[playerId];
    }
    #endregion
}

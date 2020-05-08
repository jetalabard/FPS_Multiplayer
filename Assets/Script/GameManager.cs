using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public MatchSettings MatchSettings;

    [SerializeField]
    private GameObject _sceneCamera;

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

    public static void RegisterPlayer(string netId, Player player)
    {
        string playerId = PlayerIdPrefix + netId;
        _players.Add(playerId, player);
        player.transform.name = playerId;
    }

    internal static void UnRegisterPlayer(string name)
    {
        _players.Remove(name);
    }

    //private void OnGUI()
    //{
    //    GUILayout.BeginArea(new Rect(200,200,200,500));
    //    GUILayout.BeginVertical();

    //    foreach (var player in _players)
    //    {
    //        GUILayout.Label(player.Key + " - " + player.Value.transform.name);
    //    }

    //    GUILayout.EndVertical();
     
    //    GUILayout.EndArea();
    //}

    public static Player GetPlayer(string playerId)
    {
        return _players[playerId];
    }

#endregion
}

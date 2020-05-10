using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class PauseMenu : MonoBehaviour
{
    public static bool isOn = false;

    private NetworkManager _networkManager;

    public void Start()
    {
        _networkManager = NetworkManager.singleton;
    }

    public void LeaveRoomButton()
    {
        MatchInfo matchInfo = _networkManager.matchInfo;
        _networkManager.matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0,
            _networkManager.OnDropConnection);
        _networkManager.StopHost();
    }
}

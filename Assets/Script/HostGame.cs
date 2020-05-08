using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HostGame : MonoBehaviour
{
    [SerializeField]
    private uint _roomSize = 10;

    [SerializeField]
    private Text _roomName;

    private NetworkManager _networkManager;

    private void Start()
    {
        _networkManager = NetworkManager.singleton;
        if (_networkManager.matchMaker == null)
        {
            _networkManager.StartMatchMaker();
        }
    }

    public void CreateRoom()
    {
        if (!string.IsNullOrEmpty(_roomName.text))
        {
            Debug.Log("Create : " + _roomName + " with " + _roomSize + " slots");
            _networkManager.matchMaker.CreateMatch(
                _roomName.text, 
                _roomSize, 
                true, 
                string.Empty, 
                string.Empty, 
                string.Empty,
                0,
                0,_networkManager.OnMatchCreate);
        }
    }
}

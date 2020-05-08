using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.UI;

public class JoinGame : MonoBehaviour
{
    List<GameObject> roomList = new List<GameObject>();

    [SerializeField] private Text _status;

    [SerializeField] private GameObject _roomListItemPrefab;

    [SerializeField] private Transform _roomListParent;

    private NetworkManager _networkManager;
    // Start is called before the first frame update
    void Start()
    {
        _networkManager = NetworkManager.singleton;
        if (_networkManager.matchMaker == null)
        {
            _networkManager.StartMatchMaker();
        }

        RefreshRoomList();
    }

    public void RefreshRoomList()
    {
        ClearRoomList();

        _networkManager.matchMaker.ListMatches(0, 20, string.Empty, false, 0, 0, OnMatchList);
        _status.text = "Refreshing...";
    }

    private void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> responseData)
    {
        _status.text = string.Empty;
        if (responseData == null)
        {
            _status.text = "Couldn't get match list";
            return;
        }

        foreach (var match in responseData)
        {
            GameObject roomListItemGO = Instantiate(_roomListItemPrefab);
            roomListItemGO.transform.SetParent(_roomListParent);
            RoomListItem roomListItem = roomListItemGO.GetComponent<RoomListItem>();
            if (roomListItem != null)
            {
                roomListItem.Setup(match, JoinRoom);
            }
            roomList.Add(roomListItemGO);
        }

        if (!roomList.Any())
        {
            _status.text = "No server available";
        }
    }

    private void JoinRoom(MatchInfoSnapshot match)
    {
        _networkManager.matchMaker.JoinMatch(match.networkId, string.Empty, string.Empty, string.Empty, 0, 0,
            _networkManager.OnMatchJoined);
        ClearRoomList();
        _status.text = "Joining...";
    }

    private void ClearRoomList()
    {
        foreach (var obj in roomList)
        {
            Destroy(obj);
        }

        roomList.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public string UserName { get; set; }

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

    void Update()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            _status.text = "Check internet connection!";
            _status.color = Color.red;
        }
        else if (_status.color == Color.red && Application.internetReachability != NetworkReachability.NotReachable)
        {
            _status.color = Color.white;
            RefreshRoomList();
        }
    }

    public void RefreshRoomList()
    {
        ClearRoomList();
        if (_networkManager.matchMaker == null)
        {
            _networkManager.StartMatchMaker();
        }
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
        StartCoroutine(WaitForJoin());
    }

    IEnumerator WaitForJoin()
    {
        ClearRoomList();
       
        int countDown = 10;
        while (countDown > 0)
        {
            _status.text = "Joining... ("+ countDown + ")";
            yield return new WaitForSeconds(1);
            countDown--;
        }

        _status.text = "Failed to connect";
        yield return new WaitForSeconds(2);

        MatchInfo matchInfo = _networkManager.matchInfo;
        if (matchInfo != null)
        {
            _networkManager.matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0,
                _networkManager.OnDropConnection);
            _networkManager.StopHost();
        }
        

        RefreshRoomList();
    }

    private void ClearRoomList()
    {
        foreach (var obj in roomList)
        {
            Destroy(obj);
        }

        roomList.Clear();
    }
}

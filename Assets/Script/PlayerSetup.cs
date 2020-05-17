using System;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(PlayerController))]
public class PlayerSetup : NetworkBehaviour
{
    [SerializeField]
    private Behaviour[] _componentToDisable;

    [SerializeField]
    private string _remoteLayerName = "RemotePlayer";

    [SerializeField] private string _dontDrawLayerName = "DontDraw";

    [SerializeField] private GameObject _playerGraphics;

    [SerializeField]
    private GameObject _playerUiPrefab;

    [HideInInspector]
    public GameObject PlayerUiInstance;

    // Start is called before the first frame update
    void Start()
    {
        if (!isLocalPlayer)
        {
            DisableComponents();
            AssignRemoteLayer();
        }
        else
        {
            SetLayerRecursively(_playerGraphics, LayerMask.NameToLayer(_dontDrawLayerName));

            PlayerUiInstance = Instantiate(_playerUiPrefab);
            PlayerUiInstance.name = _playerUiPrefab.name;

            PlayerUi ui = PlayerUiInstance.GetComponent<PlayerUi>();
            if (ui == null)
            {
                Debug.LogError("Problem no compoent playerUi in PlayerUiInstance");
            }
            else
            {
                ui.SetPlayer(GetComponent<Player>());
            }
            GetComponent<Player>().SetupPlayer();

            CmdSetUsername(transform.name, GameManager.LocalPlayerName);
        }
    }

    private void SetLayerRecursively(GameObject playerGraphics, int nameLayer)
    {
        playerGraphics.layer = nameLayer;

        foreach (Transform child in playerGraphics.transform)
        {
            SetLayerRecursively(child.gameObject, nameLayer);
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        string netId = Convert.ToString(GetComponent<NetworkIdentity>().netId);
        Player player = GetComponent<Player>();
        GameManager.RegisterPlayer(netId, player,
            !string.IsNullOrEmpty(player.UserName) ? player.UserName : GameManager.LocalPlayerName);
        GameManager.Instance.OnPlayerConnectedCallback.Invoke(!string.IsNullOrEmpty(player.UserName) ? player.UserName : GameManager.LocalPlayerName);
    }

    [Command]
    void CmdSetUsername(string playerId, string username)
    {
        Player player = GameManager.GetPlayer(playerId);
        if (player != null)
        {
            Debug.Log(username + " has joined !");
            player.UserName = username;
        }
    }

    private void AssignRemoteLayer()
    {
        gameObject.layer = LayerMask.NameToLayer(_remoteLayerName);
    }

    private void DisableComponents()
    {
        foreach (Behaviour behavior in _componentToDisable)
        {
            behavior.enabled = false;
        }
    }

    private void OnDisable()
    {
        Destroy(PlayerUiInstance);

        if (isLocalPlayer)
        {
            GameManager.Instance.SetSceneCameraActive(true);
        }
        GameManager.UnRegisterPlayer(transform.name);
    }
}

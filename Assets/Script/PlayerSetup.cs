using System;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
public class PlayerSetup : NetworkBehaviour
{
    [SerializeField]
    private Behaviour[] _componentToDisable;

    private Camera _sceneCamera;

    [SerializeField]
    private string _remoteLayerName = "RemotePlayer";

    [SerializeField] private string _dontDrawLayerName = "DontDraw";

    [SerializeField] private GameObject _playerGraphics;

    [SerializeField] private GameObject _playerUiPrefab;
    private GameObject _playerUiInstance;

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
            _sceneCamera = Camera.main;
            if (_sceneCamera != null)
            {
                _sceneCamera.transform.gameObject.SetActive(false);
            }

            SetLayerRecursively(_playerGraphics, LayerMask.NameToLayer(_dontDrawLayerName));

            _playerUiInstance = Instantiate(_playerUiPrefab);
            _playerUiInstance.name = _playerUiPrefab.name;
        }

        GetComponent<Player>().Setup();
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
        GameManager.RegisterPlayer(netId, player);
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
        Destroy(_playerUiInstance);
        if (_sceneCamera != null)
        {
            _sceneCamera.transform.gameObject.SetActive(true);
        }

        GameManager.UnRegisterPlayer(transform.name);
    }
}

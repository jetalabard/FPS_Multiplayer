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
        }

        GetComponent<Player>().Setup();
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
        if (_sceneCamera != null)
        {
            _sceneCamera.transform.gameObject.SetActive(true);
        }

        GameManager.UnRegisterPlayer(transform.name);
    }
}

using UnityEngine;
using UnityEngine.Networking;

public class PlayerSetup : NetworkBehaviour
{
    [SerializeField]
    private Behaviour[] _componentToDisable;

    private Camera _sceneCamera;

    // Start is called before the first frame update
    void Start()
    {
        if (!isLocalPlayer)
        {
            foreach (Behaviour behavior in _componentToDisable)
            {
                behavior.enabled = false;
            }
        }
        else
        {
            _sceneCamera = Camera.main;
            if (_sceneCamera != null)
            {
                _sceneCamera.transform.gameObject.SetActive(false);
            }
        }
    }

    private void OnDisable()
    {
        if (_sceneCamera != null)
        {
            _sceneCamera.transform.gameObject.SetActive(true);
        }
    }
}

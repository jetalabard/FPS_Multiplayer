using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(PlayerSetup))]
public class Player : NetworkBehaviour
{
    public bool IsDead { get; private set; }

    public string UserName { get; set; }

    [SerializeField] private float _maxHealth = 100f;

    [SyncVar]
    private float _currentHealth;

    public float GetHealthPourcentage()
    {
        return _currentHealth / _maxHealth;
    }

    [SerializeField] private Behaviour[] _disableOnDeath;

    [SerializeField] private GameObject[] _disableGameObjectOnDeath;

    [SerializeField] private GameObject _deathEffect;

    [SerializeField] private GameObject _spawnEffect;

    private bool[] _wasEnabled;

    private bool _firstSetup = true;

    // Start is called before the first frame update
    public void SetupPlayer()
    {
        if (isLocalPlayer)
        {
            GameManager.Instance.SetSceneCameraActive(false);
            GetComponent<PlayerSetup>().PlayerUiInstance.SetActive(true);
        }

        CmdBroadcastNewPlayerSetup();
    }

    [Command]
    private void CmdBroadcastNewPlayerSetup()
    {
        RpcSetupPlayerOnAllClients();
    }

    [ClientRpc]
    private void RpcSetupPlayerOnAllClients()
    {
        if (_firstSetup)
        {
            _wasEnabled = new bool[_disableOnDeath.Length];

            for (int i = 0; i < _disableOnDeath.Length; i++)
            {
                _wasEnabled[i] = _disableOnDeath[i].enabled;
            }

            _firstSetup = false;
        }
       
        SetDefaults();
    }

    public void SetDefaults()
    {
        IsDead = false;
        _currentHealth = _maxHealth;

        for (int i = 0; i < _disableOnDeath.Length; i++)
        {
            _disableOnDeath[i].enabled = _wasEnabled[i];
        }

        foreach (var t in _disableGameObjectOnDeath)
        {
            t.SetActive(true);
        }

        if (isLocalPlayer)
        {
            GameManager.Instance.SetSceneCameraActive(false);
            GetComponent<PlayerSetup>().PlayerUiInstance.SetActive(true);
        }

        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = true;
        }

        GameObject _gfxInstantiate = Instantiate(_spawnEffect, transform.position, Quaternion.identity);
        Destroy(_gfxInstantiate, 3f);
    }

    [ClientRpc]
    internal void RpcTakeDamage(float weaponDamage)
    {
        if (!IsDead)
        {
            _currentHealth -= weaponDamage;
            Debug.Log(transform.name + " : " + _currentHealth + " pv");
            if (_currentHealth <= 0)
            {
                Die();
            }
        }
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(GameManager.Instance.MatchSettings.RespawnTime);
       
        Transform spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;

        yield return new WaitForSeconds(0.2f);

        SetupPlayer();

        Debug.Log(transform.name + " has respawn");
    }
    
    private void Die()
    {
        IsDead = true;

        GameManager.Instance.OnPlayerKilledCallback.Invoke(UserName);


        Debug.Log(transform.name + " is dead");

        foreach (var t in _disableOnDeath)
        {
            t.enabled = false;
        }

        foreach (var t in _disableGameObjectOnDeath)
        {
            t.SetActive(false);
        }

        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }

        GameObject _gfxInstantiate = Instantiate(_deathEffect, transform.position, Quaternion.identity);
        Destroy(_gfxInstantiate, 3f);

        StartCoroutine(Respawn());
    }
}

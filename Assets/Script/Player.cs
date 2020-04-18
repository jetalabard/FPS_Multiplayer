using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{

    public bool IsDead { get; private set; }

    [SerializeField] private float _maxHealth = 100f;

    [SyncVar]
    private float _currentHealth;

    [SerializeField] private Behaviour[] _disableOnDeath;

    private bool[] _wasEnabled;

    // Start is called before the first frame update
    public void Setup()
    {
        _wasEnabled = new bool[_disableOnDeath.Length];

        for(int i =0; i < _disableOnDeath.Length; i++)
        {
            _wasEnabled[i] = _disableOnDeath[i].enabled;
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

        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = true;
        }
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
        SetDefaults();
        Transform spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;

        Debug.Log(transform.name + " has respawn");
    }

    //private void Update()
    //{
    //    if (!isLocalPlayer)
    //    {
    //        return;
    //    }

    //    if (Input.GetKeyDown(KeyCode.K))
    //    {
    //        RpcTakeDamage(999);
    //    }
    //}

    private void Die()
    {
        IsDead = true;

        Debug.Log(transform.name + " is dead");

        foreach (var t in _disableOnDeath)
        {
            t.enabled = false;
        }

        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }

        StartCoroutine(Respawn());
    }
}

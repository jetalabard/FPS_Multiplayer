using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(WeaponManager))]
public class PlayerShoot : NetworkBehaviour
{
    [SerializeField]
    private Camera _camera;

    [SerializeField]
    private LayerMask _mask;

    [SerializeField]
    private PlayerWeapon _currentWeapon;
    private WeaponManager _weaponManager;

    void Start()
    {
        if (_camera == null)
        {
            Debug.LogError("No camera added");
            this.enabled = false;
        }

        _weaponManager = GetComponent<WeaponManager>();
    }

    private void Update()
    {
        _currentWeapon = _weaponManager.CurrentWeapon;

        if (_currentWeapon.FireRate <= 0)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1"))
            {
                InvokeRepeating(nameof(Shoot), 0f,1f/_currentWeapon.FireRate);
            }
            else if (Input.GetButtonUp("Fire1"))
            {
                CancelInvoke(nameof(Shoot));
            }
        }
       
    }

    [Command]
    void CmdOnShoot()
    {
        RpcDoShootEffects();
    }


    [Command]
    void CmdOnHit(Vector3 position, Vector3 normal)
    {
        RpcDoHitEffects(position, normal);
    }

    [ClientRpc]
    void RpcDoHitEffects(Vector3 position, Vector3 normal)
    {
        GameObject hitEffect = Instantiate(_weaponManager.CurrentGraphics.HitEffectPrefab, position, Quaternion.LookRotation(normal));
        Destroy(hitEffect, 2f);
    }

    [ClientRpc]
    void RpcDoShootEffects()
    {
        _weaponManager.CurrentGraphics.MuzzleFlash.Play();
    }

    [Client]
    private void Shoot()
    {
        if (isLocalPlayer)
        {
            CmdOnShoot();
            RaycastHit hit;
            if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit, _currentWeapon.Range, _mask))
            {
                Debug.Log("Touched object : " + hit.collider.name);
                if (hit.collider.tag == "Player")
                {
                    CmdPlayerShot(hit.collider.name, _currentWeapon.Damage);
                }
            }

            CmdOnHit(hit.point, hit.normal);
        }
    }

    [Command]
    private void CmdPlayerShot(string playerId, float weaponDamage)
    {
        Debug.Log(playerId + " was hit");

        Player player = GameManager.GetPlayer(playerId);
        player.RpcTakeDamage(weaponDamage);
    }
}

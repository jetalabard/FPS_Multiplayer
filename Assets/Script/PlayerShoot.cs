using UnityEngine;
using UnityEngine.Networking;

public class PlayerShoot : NetworkBehaviour
{
    public PlayerWeapon Weapon;

    [SerializeField]
    private Camera _camera;

    [SerializeField]
    private LayerMask _mask;

    void Start()
    {
        if (_camera == null)
        {
            Debug.LogError("No camera added");
            this.enabled = false;
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    [Client]
    private void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit, Weapon.Range, _mask))
        {
            Debug.Log("Touched object : " + hit.collider.name);
            if (hit.collider.tag == "Player")
            {
                CmdPlayerShot(hit.collider.name, Weapon.Damage);
            }
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

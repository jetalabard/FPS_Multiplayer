using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class WeaponManager : NetworkBehaviour
{
    [SerializeField] private PlayerWeapon _primaryWeapon;

    [SerializeField] private Transform _weaponHolder;

    [SerializeField] private string _weaponLayerName = "Weapon";

    public WeaponGraphics CurrentGraphics { get; private set; }


    public PlayerWeapon CurrentWeapon { get; private set; }

    void Start()
    {
        EquipWeapon(_primaryWeapon);
    }

    void EquipWeapon(PlayerWeapon weapon)
    {
        CurrentWeapon = weapon;
       GameObject weaponInstance = Instantiate(weapon.Graphics, _weaponHolder.position, _weaponHolder.rotation);
       weaponInstance.transform.SetParent(_weaponHolder);

       CurrentGraphics = weaponInstance.GetComponent<WeaponGraphics>();
       if (CurrentGraphics == null)
       {
           Debug.LogError("No WeaponGraphics script on weapon : " + weapon.Name);
       }

       if (isLocalPlayer)
       {
           Util.SetLayerRecursively(weaponInstance, LayerMask.NameToLayer(_weaponLayerName));
       }
    }
}

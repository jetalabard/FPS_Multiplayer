using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNamePlate : MonoBehaviour
{

    [SerializeField] private Text _userNameText;

    [SerializeField] private Player _player;

    [SerializeField] private RectTransform _healthBarFill;
    
    void Update()
    {
        _userNameText.text = _player.UserName;
        _healthBarFill.localScale = new Vector3(_player.GetHealthPourcentage(), 1,1);
    }
}

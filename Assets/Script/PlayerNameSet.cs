using UnityEngine;
using UnityEngine.UI;

public class PlayerNameSet : MonoBehaviour
{
    [SerializeField] private Text _textUserName;

    void Start()
    {
        _textUserName.text = "Welcome ";
        if (!string.IsNullOrEmpty(GameManager.LocalPlayerName))
        {
            _textUserName.text += GameManager.LocalPlayerName;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerNameDefinition : MonoBehaviour
{
    [SerializeField] private Text _textUserName;

    [SerializeField] private Text _errorText;

    public void GoToLobbyScene()
    {
        if (!string.IsNullOrEmpty(_textUserName.text))
        {
            UserAccountManager.LoggedIn_Username = _textUserName.text;
            SceneManager.LoadScene("Lobby");
        }
        else
        {
            _errorText.text = "Please fill username";
        }
    }
}

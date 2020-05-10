using UnityEngine;

public class UserAccountManager : MonoBehaviour
{
    public static UserAccountManager instance;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this);
    }

    public static string LoggedIn_Username { get; set; } //stores username once logged in
}

using System;
using UnityEngine;

namespace Assets.Script
{
    public class KillFeed : MonoBehaviour
    {

        [SerializeField]
        private GameObject _killFeedItemPrefab;

        // Use this for initialization
        void Start()
        {
            GameManager.Instance.OnPlayerKilledCallback += OnKill;
            GameManager.Instance.OnPlayerConnectedCallback += OnConnected;
        }

        private void OnConnected(string player)
        {
            GameObject go = Instantiate(_killFeedItemPrefab, this.transform);
            go.GetComponent<KillFeedItem>().SetupConnected(player);
            Destroy(go, 4f);
        }

        public void OnKill(string player)
        {
            GameObject go = Instantiate(_killFeedItemPrefab, this.transform);
            go.GetComponent<KillFeedItem>().SetupKilled(player);
            Destroy(go, 4f);
        }

    }
}

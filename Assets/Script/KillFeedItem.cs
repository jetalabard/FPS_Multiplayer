using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script
{
    public class KillFeedItem : MonoBehaviour
    {

        [SerializeField]
        private Text _text;

        public void SetupKilled(string player)
        {
            _text.text = "<i>" + player + "</i> is dead";
        }

        public void SetupConnected(string player)
        {
            _text.text = "<i>" + player + "</i> is connected";
        }

    }
}

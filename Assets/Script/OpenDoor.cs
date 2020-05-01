using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    private Animator _animator;

    private bool _isOpened;

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
    }

    private void OnTriggerEnter(Collider hit)
    {
        if (hit.gameObject.tag == "Player")
        {
            if (!_isOpened)
            {
                _animator.Play("door_3_open");
                _isOpened = true;
            }
            
        }
    }

    private void OnTriggerExit(Collider hit)
    {
        if (hit.gameObject.tag == "Player" && _isOpened)
        {
            _animator.Play("door_3_close");
            _isOpened = false;
        }
    }
}

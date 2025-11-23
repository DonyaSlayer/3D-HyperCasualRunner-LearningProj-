using UnityEngine;

public class ButtonSFX : MonoBehaviour
{
    [SerializeField] private AudioClip _clickClip;
    private AudioSource _audioSource;

    void Awake()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.playOnAwake = false;
        _audioSource.clip = _clickClip;
    }

    public void PlayClickSound()
    {
        if (_audioSource != null && _clickClip != null)
        {
            _audioSource.PlayOneShot(_clickClip);
        }
    }
}

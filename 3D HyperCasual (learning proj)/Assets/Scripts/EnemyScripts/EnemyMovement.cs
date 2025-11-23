using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    [SerializeField] private float _runningSpeed;
    private bool _canMove = true;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource _stepAudioSource;
    [SerializeField] private float _maxVolumeDistance = 5f;


    private Transform _playerTransform;
    void Start()
    {
        transform.Rotate(0, 180, 0);
        transform.position = new Vector3(Random.Range(-1.8f, 1.8f), transform.position.y, transform.position.z);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            _playerTransform = player.transform;
        }
        if (_stepAudioSource != null && _stepAudioSource.clip != null && !_stepAudioSource.isPlaying)
        {
            _stepAudioSource.loop = true;
            _stepAudioSource.Play();
        }
    }

    private void FixedUpdate()
    {
        if (!_canMove) return;
        transform.position += transform.forward * _runningSpeed;
        UpdateStepVolume();
    }

    private void UpdateStepVolume()
    {
        if (_playerTransform == null || _stepAudioSource == null) return;
        float distance = Vector3.Distance(transform.position, _playerTransform.position);
        float volumeFactor = 1f - Mathf.Clamp01(distance / _maxVolumeDistance);
        _stepAudioSource.volume = volumeFactor;
    }

    public void StopMoving()
    {
        _canMove = false;
        if (_stepAudioSource != null && _stepAudioSource.isPlaying)
        {
            _stepAudioSource.Stop();
        }
    }
}

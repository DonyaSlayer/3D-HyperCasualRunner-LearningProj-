using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _speed;
    [SerializeField] private Vector3 _offset;

    private void Update()
    {
        Vector3 _targetPosition = new Vector3(_offset.x, _offset.y, _target.position.z + _offset.z);
        transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _speed);
    }
}

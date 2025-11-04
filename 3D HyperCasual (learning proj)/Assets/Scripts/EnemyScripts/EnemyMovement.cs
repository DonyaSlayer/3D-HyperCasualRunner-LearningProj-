using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    [SerializeField] private float _runningSpeed;
    private bool _canMove = true;
    void Start()
    {
        transform.Rotate(0, 180, 0);
        transform.position = new Vector3(Random.Range(-1.8f, 1.8f), transform.position.y, transform.position.z);
    }

    private void FixedUpdate()
    {
        if (!_canMove) return;
        transform.position += transform.forward * _runningSpeed;
    }
    public void StopMoving()
    {
        _canMove = false;
    }
}

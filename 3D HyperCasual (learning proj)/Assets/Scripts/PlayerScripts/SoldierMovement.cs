using UnityEngine;

public class SoldierMovement : MonoBehaviour
{
    [Header("Soldier Movement Variables")]

    public Transform targetPoint;
    [SerializeField] private float _movingSpeed;
    [SerializeField] private float _horizontalLimit;
    private void Update()
    {
        Vector3 targetPosition = targetPoint.position;
        targetPosition.x = Mathf.Clamp(targetPosition.x, -_horizontalLimit, _horizontalLimit);
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, _movingSpeed);
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, -_horizontalLimit, _horizontalLimit),
            transform.position.y,
            transform.position.z);
    }
}

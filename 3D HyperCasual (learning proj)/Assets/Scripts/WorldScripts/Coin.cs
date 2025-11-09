using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Coin : MonoBehaviour
{
    [Header("Magnet Settings")]
    public float magnetRadius;
    public float minSpeed;
    public float maxSpeed;
    public float accelerationDistance;

    private Transform target;

    private void Update()
    {
        FindClosestTarget();

        if (target == null) return;

        float distance = Vector3.Distance(transform.position, target.position);
        if (distance <= magnetRadius)
        {
            float speed = Mathf.Lerp(maxSpeed, minSpeed, distance / accelerationDistance);
            speed = Mathf.Clamp(speed, minSpeed, maxSpeed);

            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
        }
    }

    private void FindClosestTarget()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject[] soldiers = GameObject.FindGameObjectsWithTag("Soldier");

        Transform closest = null;
        float minDist = float.MaxValue;

        foreach (var obj in players)
        {
            float dist = Vector3.Distance(transform.position, obj.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = obj.transform;
            }
        }

        foreach (var obj in soldiers)
        {
            float dist = Vector3.Distance(transform.position, obj.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = obj.transform;
            }
        }

        target = closest;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Soldier"))
        {
            Destroy(gameObject);
            if (TeamController.instance != null)
            {
                TeamController.instance.coinCount += 1;
                int currentCoinCount = TeamController.instance.coinCount;
                if (UIManager.Instance != null)
                {
                    UIManager.Instance.UpdateCoinCount(currentCoinCount);
                }
            }
            else
            {
                Debug.LogWarning("TeamController.instance == null");
            }
        }
    }
}

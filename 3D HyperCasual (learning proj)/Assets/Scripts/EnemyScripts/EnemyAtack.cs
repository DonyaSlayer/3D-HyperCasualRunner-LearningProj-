using UnityEngine;

public class EnemyAtack : MonoBehaviour
{
    [SerializeField] private float _damage;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player")|| collision.gameObject.CompareTag("Soldier"))
        {
            if (TeamController.instance.soldiers.Count > 0)
            {
                TeamController.instance.RemoveSoldier();
            }
            else
            {
                collision.gameObject.GetComponent<PlayerHealth>().Damage(_damage);
            }
            Destroy(gameObject);
        }
    }
}

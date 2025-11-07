using System.Collections.Generic;
using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    [SerializeField] private EnemyShootingController _enemyShootingController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Soldier"))
        {
            _enemyShootingController.StartShooting();
        }
    }
}

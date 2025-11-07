using System.Collections.Generic;
using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    private EnemyShootingController _enemyShootingController;
    public List<Collider> collidersInZone = new List<Collider>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Soldier"))
        {
            CheckShooting();
            collidersInZone.Add(other);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player") || !other.CompareTag("Soldier"))
        {
            CheckShooting();
            collidersInZone.Remove(other);
        }
    }

    public void CheckShooting()
    {
        if (collidersInZone.Count > 0)
        {
            _enemyShootingController.StartShooting();
        }
        else
        {
            _enemyShootingController.StopShooting();
        }
    }
}

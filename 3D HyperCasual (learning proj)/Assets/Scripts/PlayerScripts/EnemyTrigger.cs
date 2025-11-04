using UnityEngine;
using System.Collections.Generic;

public class EnemyTrigger : MonoBehaviour
{
    public List<Collider> collidersInZone = new List<Collider>();
    [SerializeField] private TeamController _teamController;

    private void OnTriggerEnter(Collider other)
    {
        if(!collidersInZone.Contains(other))
        {
            collidersInZone.Add(other);
            _teamController?.CheckShooting();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (collidersInZone.Contains(other))
        {
            collidersInZone.Remove(other);
            _teamController?.CheckShooting();
        }
    }
    private void HandleEnemiesInZone()
    {
        // 1. ¬идал€Їмо null (мертв≥ або знищен≥ об'Їкти)
        collidersInZone.RemoveAll(c => c == null);

        // 2. якщо ворог≥в б≥льше немаЇ Ч оновлюЇмо стан стр≥льби
        if (_teamController != null && collidersInZone.Count == 0 && _teamController.collidersInZone)
        {
            _teamController.CheckShooting();
        }
    }
    private void Update()
    {
        HandleEnemiesInZone();
    }
}

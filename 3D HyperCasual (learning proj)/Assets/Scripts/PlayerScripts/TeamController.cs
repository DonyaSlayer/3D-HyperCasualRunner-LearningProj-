using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class TeamController : MonoBehaviour
{
    [SerializeField] private GameObject _soldierPrefab;

    [Header("Other components")]
    public bool collidersInZone;
    [SerializeField] private EnemyTrigger _enemyTrigger;
    [SerializeField] private ShootingController _playersShootingController;
    public GameObject bulletCurrentPrefab;
    public GameObject bulletDefaultPrefab;
    public static TeamController instance;
    private Coroutine _bulletsTimer;


    [Header("Team Variables")]
    [SerializeField] private Transform _pointsParent;
    [SerializeField] private Transform[] _teamPoints;
    public List<SoldierManager> soldiers;
    [SerializeField] private int _maxSoldiersCount;

    [Header("Collectable Variables")]
    public int coinCount;


    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        for (int i = 0; i < _teamPoints.Length; i++)
        {
            _teamPoints[i] = _pointsParent.GetChild(i);
        }
    }

    public void ChangeSoldiersCount(int count)
    {
        if(count > 0)
        {
            for(int i = 0; i <count; i++)
            {
                AddSoldier();
            }
        }
        else if (count < 0)
        {
            count *= -1;
            for (int i = 0; i < count; i++)
            {
               RemoveSoldier();
            }
        }
    }

    public void AddSoldier()
    {
        if (soldiers.Count == _maxSoldiersCount) return;
        Transform targetForSoldier = _teamPoints[soldiers.Count];
        GameObject newSoldier = Instantiate(_soldierPrefab, targetForSoldier.position, Quaternion.identity, null);
        newSoldier.GetComponent<SoldierManager>().Initialize(targetForSoldier);
        newSoldier.GetComponent<ShootingController>().bulletPrefab = bulletCurrentPrefab;
        soldiers.Add(newSoldier.GetComponent<SoldierManager>());
    }

    public void RemoveSoldier()
    {
        if (soldiers == null || soldiers.Count == 0)
        {
            Debug.LogWarning("RemoveSoldier() called, but no soldiers left in the list!");
            return;
        }
        Destroy(soldiers[soldiers.Count - 1].gameObject); 
        soldiers.RemoveAt(soldiers.Count - 1);
    }

    public void CheckShooting()
    {
        if (_enemyTrigger.collidersInZone.Count > 0)
        {
            collidersInZone = true;
            foreach (var soldier in soldiers)
                soldier.shootingController.StartShooting();
            _playersShootingController.StartShooting();
        }
        else
        {
            collidersInZone = false;
            foreach (var soldier in soldiers)
                soldier.shootingController.StopShooting();
            _playersShootingController.StopShooting();
        }
    }
    public void SetBullets (GameObject newBullet, float timer, string bulletType)
    {
        bulletCurrentPrefab = newBullet;
        SetBulletToTeam();
        if (_bulletsTimer != null)
        {
            StopCoroutine(_bulletsTimer);
        }
        _bulletsTimer = StartCoroutine(BulletTimer(timer));
        if (UIManager.Instance != null)
        {
            UIManager.Instance.StartBoxBuffTimer(timer, bulletType);
        }
    }
    IEnumerator BulletTimer(float timer) 
    {
        yield return new WaitForSeconds(timer);
        bulletCurrentPrefab = bulletDefaultPrefab;
        SetBulletToTeam();
    }

    private void SetBulletToTeam()
    {
        for (int i = 0; i < soldiers.Count; i++)
        {
            soldiers[i].shootingController.bulletPrefab = bulletCurrentPrefab;
        }
        _playersShootingController.bulletPrefab = bulletCurrentPrefab;
    }
}

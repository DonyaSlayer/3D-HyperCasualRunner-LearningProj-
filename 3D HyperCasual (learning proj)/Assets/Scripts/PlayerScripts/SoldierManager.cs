using UnityEngine;

public class SoldierManager : MonoBehaviour
{
    [Header("Soldier Variables")]
    public SoldierMovement soldierMovement;
    public ShootingController shootingController;
    public Animator animator;

    public void Initialize(Transform targetPoint)
    {
        soldierMovement.targetPoint = targetPoint;
    }
}

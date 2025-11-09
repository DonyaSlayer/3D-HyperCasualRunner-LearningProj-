using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ArchController : MonoBehaviour
{

    private List <GameObject> arches = new List <GameObject>();
    [SerializeField] private GameObject _archPrefab;
    [SerializeField] private Transform _leftArchPoint;
    [SerializeField] private Transform _rightArchPoint;
    private Coroutine _archActivatedDelay;
    private Arch _activatedArch;

    public void ArchTargetChoosing()
    {
        int random = Random.Range(0, 3);
        switch (random)
        {
            case 0:
                SpawnArch(_leftArchPoint);
                SpawnArch(_rightArchPoint);
                break;
            case 1:
                SpawnArch(_leftArchPoint);
                break;

            case 2:
                SpawnArch(_rightArchPoint);
                break;
        }
    }


    public void SpawnArch(Transform archPoint)
    {
        GameObject newArch = Instantiate(_archPrefab, archPoint.position + Vector3.up * 2f, Quaternion.identity,transform);
        newArch.GetComponent<Arch>().archController = this;
        arches.Add(newArch);
    }

    public void ArchActivated(Arch arch)
    {
        if (_archActivatedDelay == null)
        {
            _archActivatedDelay = StartCoroutine(ArchActivatorDelay());
            _activatedArch = arch;
        }
    }

    public IEnumerator ArchActivatorDelay()
    {
        yield return new WaitForSeconds(0.1f);

        TeamController.instance.ChangeSoldiersCount(_activatedArch.soldiers);


        foreach (GameObject arch in arches)
        {
            Destroy(arch);
        }

        Destroy(gameObject);
    }
}

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Arch: MonoBehaviour
{
    public bool activated = false;
    [HideInInspector] public ArchController archController;

    [Header("Soldier Settings")]
    [HideInInspector] public int soldiers;
    public int minSoldiers;
    public int maxSoldiers;

    [Header("Visual")]
    [SerializeField] public TMP_Text _soldiersText;
    [SerializeField] public MeshRenderer _meshRenderer;
    [SerializeField] public Material _redMaterial;
    [SerializeField] public Material _greenMaterial;

    private void Start()
    {
        soldiers = GetRandomSoldiers();
        RefreshVisual();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            activated = true;
            archController.ArchActivated(this);
        }
    }

    private void RefreshVisual()
    {
        string symbol;
        if (soldiers > 0)
        {
            _meshRenderer.material = _greenMaterial;
            symbol = "+";
        }
        else
        {
            _meshRenderer.material = _redMaterial;
            symbol = "";
        }
        _soldiersText.text = symbol + soldiers;
    }

    public int GetRandomSoldiers()
    {
        int count = 0;

        while (count == 0)
        {
            count = Random.Range(minSoldiers, maxSoldiers + 1);
        }
        return count;
    }
}

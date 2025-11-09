using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI CoinCountText;
    public TextMeshProUGUI WaveCountText;
    public static UIManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    public void UpdateCoinCount(int newCount)
    {
        CoinCountText.text = $"Coins: {newCount}";
    }

    public void UpdateWaveCount(int newWave)
    {
        WaveCountText.text = $"Wawe: {newWave}";
    }
}

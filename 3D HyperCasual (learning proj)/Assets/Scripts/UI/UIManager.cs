using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI CoinCountText;
    public TextMeshProUGUI WaveCountText;
    public static UIManager Instance { get; private set; }

    [Header("Buff Indicator")]
    [SerializeField] private GameObject _boxBuffIndicatorParent;
    [SerializeField] private Image _boxBuffFillImage;
    [SerializeField] private Image _bulletIconImage;
    [SerializeField] private Sprite _blueBulletSprite;
    [SerializeField] private Sprite _greenBulletSprite;

    [Header("Game Over Panel")]
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private TextMeshProUGUI _finalWaveText;
    [SerializeField] private TextMeshProUGUI _finalCoinText;

    private const int _gameSceneIndex = 1;
    private const int MainMenuSceneIndex = 0;

    [Header("Game Stats")]
    public int ñurrentCoins { get; private set; } = 0;
    public int ñurrentWaves { get; private set; } = 0;

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
        ñurrentCoins = newCount;
        CoinCountText.text = $"Coins: {newCount}";
    }

    public void UpdateWaveCount(int newWave)
    {
        ñurrentWaves = newWave;
        WaveCountText.text = $"Wawe: {newWave}";
    }



    public void StartBoxBuffTimer(float duration, string bulletType)
    {
        if (_boxBuffIndicatorParent == null || _boxBuffFillImage == null || _bulletIconImage == null) return;
        _boxBuffIndicatorParent.SetActive(true);
        if (bulletType == "Blue")
        {
            _bulletIconImage.sprite = _blueBulletSprite;
        }
        else if (bulletType == "Green")
        {
            _bulletIconImage.sprite = _greenBulletSprite;
        }
        StopAllCoroutines();
        StartCoroutine(UpdateBuffTimer(duration));
    }

    private System.Collections.IEnumerator UpdateBuffTimer(float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float fillAmount = 1f - (elapsedTime / duration);
            _boxBuffFillImage.fillAmount = fillAmount;
            yield return null;
        }
        _boxBuffIndicatorParent.SetActive(false);
    }

    public void ShowGameOver()
    {
        Time.timeScale = 0f;
        _finalWaveText.text = $": {ñurrentWaves}";
        _finalCoinText.text = $": {ñurrentCoins}";
        _gameOverPanel.SetActive(true);
    }

    public void RetryGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(_gameSceneIndex);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(MainMenuSceneIndex);
    }
}

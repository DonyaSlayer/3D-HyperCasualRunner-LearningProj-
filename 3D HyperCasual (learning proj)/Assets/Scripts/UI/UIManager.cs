using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

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
    [SerializeField] private CanvasGroup _gameOverCanvasGroup;

    private const int _gameSceneIndex = 1;
    private const int MainMenuSceneIndex = 0;

    [Header("Game Stats")]
    public int ñurrentCoins { get; private set; } = 0;
    public int ñurrentWaves { get; private set; } = 0;

    public const string BestWaveKey = "BestWaveScore";
    public const string BestCoinKey = "BestCoinScore";

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

    private void Start()
    {
        if (_gameOverCanvasGroup != null)
        {
            _gameOverCanvasGroup.alpha = 0f;
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


    public void ShowGameOverPanelSmoothly()
    {
        UpdateGameOverStats();
        StopAllCoroutines();
        StartCoroutine(FadeInGameOverPanel(3f));
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

    private IEnumerator FadeInGameOverPanel(float duration)
    {
        if (_gameOverCanvasGroup == null)
        {
            _gameOverPanel.SetActive(true); 
            Time.timeScale = 0f;
            yield break;
        }
        _gameOverPanel.SetActive(true);
        float startTime = Time.unscaledTime; 
        while (_gameOverCanvasGroup.alpha < 1f)
        {
            float t = (Time.unscaledTime - startTime) / duration;
            _gameOverCanvasGroup.alpha = Mathf.Lerp(0f, 1f, t);
            yield return null;
        }
        _gameOverCanvasGroup.alpha = 1f;
        Time.timeScale = 0f;
    }
    private void UpdateGameOverStats()
    {
        int savedBestWave = PlayerPrefs.GetInt(BestWaveKey, 0);
        int savedBestCoin = PlayerPrefs.GetInt(BestCoinKey, 0);

        if (ñurrentWaves > savedBestWave)
        {
            savedBestWave = ñurrentWaves;
            PlayerPrefs.SetInt(BestWaveKey, savedBestWave);
        }
        if (ñurrentCoins > savedBestCoin)
        {
            savedBestCoin = ñurrentCoins;
            PlayerPrefs.SetInt(BestCoinKey, savedBestCoin);
        }
        PlayerPrefs.Save();
        _finalWaveText.text = $": {ñurrentWaves}";
        _finalCoinText.text = $": {ñurrentCoins}";
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

    public int GetBestWaveScore()
    {
        return PlayerPrefs.GetInt(BestWaveKey, 0);
    }

    public int GetBestCoinScore()
    {
        return PlayerPrefs.GetInt(BestCoinKey, 0);
    }
}

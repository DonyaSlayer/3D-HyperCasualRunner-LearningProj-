using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI CoinCountText;
    public TextMeshProUGUI WaveCountText;
    public static UIManager Instance { get; private set; }

    
    [SerializeField] private Sprite _blueBulletSprite;    
    [SerializeField] private Sprite _greenBulletSprite;

    [Header("Buff Indicator")]
    [SerializeField] private GameObject _boxBuffIndicatorParent;
    [SerializeField] private Image _boxBuffFillImage;
    [SerializeField] private Image _bulletIconImage;

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
        /* (_bulletIconImage != null)
        {
            _bulletIconImage.gameObject.SetActive(false);
        }*/
    }

    public void UpdateCoinCount(int newCount)
    {
        CoinCountText.text = $"Coins: {newCount}";
    }

    public void UpdateWaveCount(int newWave)
    {
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
}

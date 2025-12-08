using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


public class UIMainMenu : MonoBehaviour
{

    [SerializeField] private GameObject _mainMenuPanel; 
    [SerializeField] private GameObject _recordsPanel;  

    [SerializeField] private TextMeshProUGUI _bestWaveDisplay;
    [SerializeField] private TextMeshProUGUI _bestCoinDisplay;

    [SerializeField] private AudioSource _menuMusicSource;
    [SerializeField] private AudioSource _sfxSource;
    [SerializeField] private AudioClip _clickClip;
    private void Start()
    {
        if (_mainMenuPanel != null) _mainMenuPanel.SetActive(true);
        if (_recordsPanel != null) _recordsPanel.SetActive(false);
        if (_menuMusicSource != null) _menuMusicSource.Play();
    }
        

    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit is succsesfull");
    }

    private void PlayClickSound()
    {
        if (_sfxSource != null && _clickClip != null)
        {
            _sfxSource.PlayOneShot(_clickClip, 1.0f);
        }
    }

    public void OpenRecordsMenu()
    {
        if (_recordsPanel != null) _recordsPanel.SetActive(true);
        DisplayBestScores();
    }
    public void CloseRecordsMenu()
    {
        PlayClickSound();
        if (_recordsPanel != null) _recordsPanel.SetActive(false);
    }
    private void DisplayBestScores()
    {
        int bestWave = PlayerPrefs.GetInt(UIManager.BestWaveKey, 0);
        int bestCoin = PlayerPrefs.GetInt(UIManager.BestCoinKey, 0); 

        if (_bestWaveDisplay != null)
        {
            _bestWaveDisplay.text = $"BEST WAVE: {bestWave}";
        }
        if (_bestCoinDisplay != null)
        {
            _bestCoinDisplay.text = $"BEST COINS: {bestCoin}";
        }
    }
}

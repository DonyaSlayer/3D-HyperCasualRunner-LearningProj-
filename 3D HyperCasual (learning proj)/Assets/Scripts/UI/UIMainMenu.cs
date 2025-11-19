using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class UIMainMenu : MonoBehaviour
{

    [SerializeField] private GameObject _mainMenuPanel; 
    [SerializeField] private GameObject _recordsPanel;  

    [SerializeField] private TextMeshProUGUI _bestWaveDisplay;
    [SerializeField] private TextMeshProUGUI _bestCoinDisplay;

    private void Start()
    {
        if (_mainMenuPanel != null) _mainMenuPanel.SetActive(true);
        if (_recordsPanel != null) _recordsPanel.SetActive(false);
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

    public void OpenRecordsMenu()
    {
        if (_recordsPanel != null) _recordsPanel.SetActive(true);
        DisplayBestScores();
    }
    public void CloseRecordsMenu()
    {
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

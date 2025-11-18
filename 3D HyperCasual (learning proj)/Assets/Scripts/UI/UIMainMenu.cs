using UnityEngine;
using UnityEngine.SceneManagement;


public class UIMainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit is succsesfull");
    }
}

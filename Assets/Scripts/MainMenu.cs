using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject confirmationPanel;
    public GameObject skiFreeMenu;

    void Start()
    {
        confirmationPanel.SetActive(false);
        skiFreeMenu.SetActive(true);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Level 1 (Snow)");
    }

    public void QuitConformation()
    {
        confirmationPanel.SetActive(true);
        skiFreeMenu.SetActive(false);
    }

    public void CancelQuit()
    {
        confirmationPanel.SetActive(false);
        skiFreeMenu.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Quit");
    }
}
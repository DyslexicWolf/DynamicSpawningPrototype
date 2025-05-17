using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public void OnStartButtonPressed()
    {
        SceneManager.LoadScene("Game");
    }

    public void OnControlsButtonPressed()
    {
        SceneManager.LoadScene("Controls");
    }

    public void OnExitButtonPressed()
    {
        Application.Quit();
    }
}

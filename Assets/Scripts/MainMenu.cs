using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Button startButton;

    void Awake()
    {
        if (startButton != null)
            startButton.onClick.AddListener(() => SceneManager.LoadScene("Game"));
    }
}

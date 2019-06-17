using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuBehavior : MonoBehaviour
{
    public Button exitButton, levelButton;

    // Start is called before the first frame update
    void Start()
    {
        exitButton.onClick.AddListener(ExitPress);
        levelButton.onClick.AddListener(LevelPress);
    }


    public void ExitPress()
    {
        Application.Quit();
    }

    public void LevelPress()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1); // SpaceLevel
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuBehavior : MonoBehaviour
{
    public CanvasGroup main, load;

    public Button exitButton, levelButton;

    public Text loadText;

    // Start is called before the first frame update
    void Start()
    {
        exitButton.onClick.AddListener(ExitPress);
        levelButton.onClick.AddListener(LevelPress);

        HideCanvas(load);
    }


    public void ExitPress()
    {
        Application.Quit();
    }

    public void LevelPress()
    {
        StartCoroutine(AsyncLoadScene("SlimeLevel"));
    }

    IEnumerator AsyncLoadScene(string name)
    {
        HideCanvas(main);
        ShowCanvas(load);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(name);
        int x = 0;
        while (!asyncLoad.isDone)
        {
            x++;
            loadText.text += x + " ";
            yield return null;
        }
    }

    public static void HideCanvas(CanvasGroup can)
    {
        can.alpha = 0f;
        can.blocksRaycasts = false;
    }

    public static void ShowCanvas(CanvasGroup can)
    {
        can.alpha = 1f;
        can.blocksRaycasts = true;
    }
}

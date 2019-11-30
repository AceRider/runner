using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Runner.Utils;
using Runner.Common;
public class MainMenuController : MonoBehaviour
{
    public GameObject[] panels;
    public GameObject[] mmButons;
    int maxLives = 3;

    private void Start()
    {
        panels = GameObject.FindGameObjectsWithTag("subpanel");
        mmButons = GameObject.FindGameObjectsWithTag("mmbutton");

        foreach (GameObject p in panels)
            p.SetActive(false);
    }

    public void ClosePanel(Button button)
    {
        button.gameObject.transform.parent.gameObject.SetActive(false);
        foreach (GameObject b in mmButons)
            b.SetActive(true);
    }
    public void OpenPanel(Button button)
    {
        button.gameObject.transform.GetChild(1).gameObject.SetActive(true);
        foreach (GameObject b in mmButons)
            if(b != button.gameObject)
                b.SetActive(false);
    }

    public void LoadGameScene()
    {
        PlayerPrefs.SetInt("lives", maxLives);
        RunnerUtils.OpenScene(RunnerSceneType.Game);
    }

    public void QuitGame()
    {
        RunnerUtils.Quit();
    }

}

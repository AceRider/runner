using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Runner.Common;

namespace Runner.Menu
{
    public class MainMenuController : MonoBehaviour
    {
        #region Public properties
        public GameObject[] menuPanelsArray;
        public GameObject[] menuButtonsArray;
        #endregion
        #region Private properties
        private int maxLives = 3;
        #endregion

        private void Start()
        {
            menuPanelsArray = GameObject.FindGameObjectsWithTag("subpanel");
            menuButtonsArray = GameObject.FindGameObjectsWithTag("mmbutton");
            //disable all panels in the main menu in the start
            foreach (GameObject panel in menuPanelsArray)
                panel.SetActive(false);
        }
        #region Panels methods
        public void OpenPanel(Button button)
        {
            //because the panels are child in the main button, we can set using this
            button.gameObject.transform.GetChild(1).gameObject.SetActive(true);
            foreach (GameObject buttonMenu in menuButtonsArray)
                if (buttonMenu != button.gameObject)
                    buttonMenu.SetActive(false);
        }
        public void ClosePanel(Button button)
        {
            button.gameObject.transform.parent.gameObject.SetActive(false);
            foreach (GameObject buttonMenu in menuButtonsArray)
                buttonMenu.SetActive(true);
        }
        #endregion
        #region Game methods
        public void LoadGameScene()
        {
            PlayerPrefs.SetInt("lives", maxLives);
            RunnerUtils.OpenScene(RunnerSceneType.Game);
        }
        public void QuitGame()
        {
            RunnerUtils.Quit();
        }
        #endregion
    }
}

using Runner.Common;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Runner.Utils
{
    public class RunnerUtils
    {

        //------------------------------------
        //         Utils Methods
        //------------------------------------

        public static void Quit()
        {
            Application.Quit();
        }

        public static void OpenScene(RunnerSceneType scene)
        {
            SceneManager.LoadScene((int)scene, LoadSceneMode.Single);
        }
    }
}
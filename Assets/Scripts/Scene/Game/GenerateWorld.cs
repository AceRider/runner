using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Runner.Common;

namespace Runner.Scene.Game
{
    public class GenerateWorld : MonoBehaviour
    {
        #region Public static properties
        public static GameObject worldGenerator;
        public static GameObject recentPlatform;
        #endregion
        void Awake()
        {
            worldGenerator = new GameObject("dummy");
        }
        #region Generator methods
        //Run the generator to create the world
        public static void RunGenerator()
        {
            //get a random platform from the pool
            GameObject pool = Pool.singleton.GetRandom();
            if (pool == null) return;

            if (recentPlatform != null)
            {
                //if is TSection, push futher
                if (recentPlatform.gameObject.tag == "platformTSection")
                    SetWorldGenPosition(20);
                else
                    SetWorldGenPosition(10);

                //if is StairSection, generate the world to up
                if (recentPlatform.tag == "stairsUp")
                    worldGenerator.transform.Translate(0, 5, 0);
            }
            recentPlatform = pool;
            pool.SetActive(true);
            pool.transform.position = worldGenerator.transform.position;
            pool.transform.rotation = worldGenerator.transform.rotation;

            //if stairsdown, generate the world to down and rotate the stairs to match others platforms
            if (pool.tag == "stairsDown")
            {
                worldGenerator.transform.Translate(0, -5, 0);
                pool.transform.Rotate(0, 180, 0);
                pool.transform.position = worldGenerator.transform.position;
            }
        }
        private static void SetWorldGenPosition(int fwd)
        {
            worldGenerator.transform.position = recentPlatform.transform.position + PlayerController.player.transform.forward * fwd;
        }
        #endregion
        public void QuitToMenu()
        {
            RunnerUtils.OpenScene(RunnerSceneType.Menu);
        }
    }
}

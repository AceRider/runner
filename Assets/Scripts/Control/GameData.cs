using Runner.Menu;
using UnityEngine;
using UnityEngine.UI;

namespace Runner.Control
{
    public class GameData : MonoBehaviour
    {
        #region Public static properties
        public static GameData singleton;
        #endregion
        #region Public properties
        public Text scoreText = null;
        public int score = 0;
        public GameObject musicSlider;
        public GameObject soundSlider;
        #endregion

        private void Awake()
        {
            #region Singleton check
            GameObject[] gd = GameObject.FindGameObjectsWithTag("gamedata");
            if (gd.Length > 1)
            {
                Destroy(this.gameObject);
            }
            DontDestroyOnLoad(this.gameObject);
            singleton = this;
            #endregion

            PlayerPrefs.SetInt("score", 0);
            musicSlider.GetComponent<UpdateMusic>().Start();
            soundSlider.GetComponent<UpdateSound>().Start();
        }

        public void UpdateScore(int score_new)
        {
            score += score_new;
            PlayerPrefs.SetInt("score", score);
            if (scoreText != null)
                scoreText.text = "Score: " + score;
        }
    }
}

using Runner.Menu;
using UnityEngine;
using UnityEngine.UI;

namespace Runner.Control
{
    public class GameDataManager : MonoBehaviour
    {
        #region Public static properties
        public static GameDataManager singleton;
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
            if (musicSlider)
                musicSlider.GetComponent<MenuMusicDisplay>().Start();
            if (soundSlider)
                soundSlider.GetComponent<MenuSoundDisplay>().Start();
        }

        public void UpdateScore(int score_new)
        {
            score += score_new;
            PlayerPrefs.SetInt("score", score);
            if (scoreText != null)
                scoreText.text = "Score: " + score;
        }

        public int GetLastScore() 
        {
            //Get the PlayerPrefs scores
            if (PlayerPrefs.HasKey("lastscore"))
                //lastScore.text = "Last Score: " + PlayerPrefs.GetInt("lastscore");
                return PlayerPrefs.GetInt("lastscore");
            else
                return 0;
            // lastScore.text = "Last Score: 0";          
        }

        public int GetHighestScore()
        {
            if (PlayerPrefs.HasKey("highscore"))
                //highestScore.text = "Highest Score: " + PlayerPrefs.GetInt("highscore");
                return PlayerPrefs.GetInt("highscore");
            else
                //highestScore.text = "Highest Score: 0";
                return 0;
        }
    }
}

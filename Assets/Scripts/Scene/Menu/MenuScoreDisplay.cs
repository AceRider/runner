using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Runner.Control;

namespace Runner.Menu
{
    public class DisplMenuScoreDisplayayStats : MonoBehaviour
    {
        #region Public properties
        public Text lastScore;
        public Text highestScore;
        #endregion

        private void OnEnable()
        {
            lastScore.text = "Last Score: " + GameDataManager.singleton.GetLastScore();
            highestScore.text = "Highest Score: " + GameDataManager.singleton.GetHighestScore();

        }
    }
}

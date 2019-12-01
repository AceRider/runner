using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DisplayStats : MonoBehaviour
{
    #region Public properties
    public Text lastScore;
    public Text highestScore;
    #endregion

    private void OnEnable()
    {
        //Get the PlayerPrefs scores
        if (PlayerPrefs.HasKey("lastscore"))
            lastScore.text = "Last Score: " + PlayerPrefs.GetInt("lastscore");
        else
            lastScore.text = "Last Score: 0";

        if (PlayerPrefs.HasKey("highscore"))
            highestScore.text = "Highest Score: " + PlayerPrefs.GetInt("highscore");
        else
            highestScore.text = "Highest Score: 0";
        
    }
}

using Runner.Control;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Runner.Control
{
    public class ScoreTextUpdater : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            GameDataManager.singleton.scoreText = this.GetComponent<Text>();
            GameDataManager.singleton.UpdateScore(0);
        }
    }
}

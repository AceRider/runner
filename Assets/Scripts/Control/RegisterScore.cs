using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace World
{
    public class RegisterScore : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            MainMenu.GameData.singleton.scoreText = this.GetComponent<Text>();
            MainMenu.GameData.singleton.UpdateScore(0);
        }
    }
}

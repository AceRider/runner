using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace World
{
    public class Scroll : MonoBehaviour
    {
        //This is to occur in the same time of the game
        private void FixedUpdate()
        {
            if (PlayerManager.PlayerController.isDead) return;

            this.transform.position += PlayerManager.PlayerController.player.transform.forward * -0.1f;

            if (PlayerManager.PlayerController.currentPlatform == null) return;
            //the starcase is 60 degree
            if (PlayerManager.PlayerController.currentPlatform.tag == "stairsUp")
                this.transform.Translate(0, -0.06f, 0);
            if (PlayerManager.PlayerController.currentPlatform.tag == "stairsDown")
                this.transform.Translate(0, 0.06f, 0);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour
{
    //This is to occur in the same time of the game
    private void FixedUpdate()
    {
        if (PlayerController.isDead) return;

        this.transform.position += PlayerController.player.transform.forward * -0.1f;

        if (PlayerController.currentPlatform == null) return;
        //the starcase is 60 degree
        if (PlayerController.currentPlatform.tag == "stairsUp")
            this.transform.Translate(0, -0.06f, 0);
        if (PlayerController.currentPlatform.tag == "stairsDown")
            this.transform.Translate(0, 0.06f, 0);
    }
}

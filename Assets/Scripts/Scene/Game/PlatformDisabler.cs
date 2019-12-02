using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.Scene.Game
{
    public class PlatformDisabler : MonoBehaviour
    {
        bool isDeactivate = false;

        private void OnCollisionEnter(Collision collision)
        {
            //destroy magic ball
            if (collision.gameObject.tag == "Hadouken")
            {
                collision.gameObject.SetActive(false);
            }
        }

        //remove platform after the player run
        private void OnCollisionExit(Collision player)
        {
            if (PlayerController.isDead) return;
            if (player.gameObject.tag == "Player" && !isDeactivate)
            {
                Invoke("SetInactive", 4.0f);
                isDeactivate = true;
            }
        }

        void SetInactive()
        {
            this.gameObject.SetActive(false);
            isDeactivate = false;
        }
    }
}

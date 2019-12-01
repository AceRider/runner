﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.Scene.Game
{
    public class Deactivate : MonoBehaviour
    {
        bool isDeactivate = false;

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

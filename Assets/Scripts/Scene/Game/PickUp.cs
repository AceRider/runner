using Runner.Control;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.Scene.Game
{
    public class PickUp : MonoBehaviour
    {
        #region Private properties
        private MeshRenderer[] meshArray;
        #endregion

        private void Start()
        {
            meshArray = this.GetComponentsInChildren<MeshRenderer>();
        }
        private void OnTriggerEnter(Collider other)
        {
            //check if the player picks the coin
            if (other.gameObject.tag == "Player")
            {
                GameData.singleton.UpdateScore(10);
                PlayerController.soundEffect[1].Play();
                foreach (MeshRenderer meshObj in meshArray)
                    meshObj.enabled = false;
            }
        }
        private void OnEnable()
        {
            //enable the coin after activate the platforms
            if (meshArray != null)
            {
                foreach (MeshRenderer m in meshArray)
                    m.enabled = true;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace World
{
    //Check the wall is triggered and explode it
    public class DestroyWall : MonoBehaviour
    {
        #region Public properties
        public GameObject[] blocks;
        public GameObject explosion;
        #endregion
        #region Private properties
        private List<Rigidbody> blockRigidbodyList = new List<Rigidbody>();
        private List<Vector3> positionList = new List<Vector3>();
        private List<Quaternion> rotationList = new List<Quaternion>();
        private Collider blockCollider;
        #endregion
        private void Awake()
        {
            //get the list of blocks in the wall
            blockCollider = this.GetComponent<Collider>();
            foreach (GameObject block in blocks)
            {
                blockRigidbodyList.Add(block.GetComponent<Rigidbody>());
                positionList.Add(block.transform.localPosition);
                rotationList.Add(block.transform.localRotation);
            }
        }
        private void OnEnable()
        {
            blockCollider.enabled = true;
            for (int i = 0; i < blocks.Length; i++)
            {
                blocks[i].transform.localPosition = positionList[i];
                blocks[i].transform.localRotation = rotationList[i];
                blockRigidbodyList[i].isKinematic = true;
            }
        }
        private void OnCollisionEnter(Collision collision)
        {
            //check if the player attacked the wall
            if (collision.gameObject.tag == "Hadouken")
            {
                GameObject explotionObj = Instantiate(explosion, collision.contacts[0].point, Quaternion.identity);
                Destroy(explotionObj, 2.5f);
                //using the list of blocks to deactivate kinematic and apply physics
                blockCollider.enabled = false;
                foreach (Rigidbody rigidbody in blockRigidbodyList)
                    rigidbody.isKinematic = false;
            }
        }
    }
}

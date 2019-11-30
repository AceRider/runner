using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWall : MonoBehaviour
{
    public GameObject[] bricks;
    List<Rigidbody> bricksRBs = new List<Rigidbody>();
    List<Vector3> positions = new List<Vector3>();
    List<Quaternion> rotations = new List<Quaternion>();
    Collider col;
    
    private void Awake()
    {
        //get the list of blocks
        col = this.GetComponent<Collider>();
        foreach(GameObject b in bricks)
        {
            bricksRBs.Add(b.GetComponent<Rigidbody>());
            positions.Add(b.transform.localPosition);
            rotations.Add(b.transform.localRotation);
        }
    }
    private void OnEnable()
    {
        col.enabled = true;
        for(int i=0;i< bricks.Length;i++)
        {
            bricks[i].transform.localPosition = positions[i];
            bricks[i].transform.localRotation = rotations[i];
            bricksRBs[i].isKinematic = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Hadouken")
        {
            //using the list of blocks to deactivate kinematic and apply physics
            col.enabled = false;
            foreach (Rigidbody r in bricksRBs)
                r.isKinematic = false;
        }
    }
}

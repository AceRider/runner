using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    MeshRenderer[] meshRenderers;

    private void Start()
    {
        meshRenderers = this.GetComponentsInChildren<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            GameData.singleton.UpdateScore(10);
            PlayerController.soundEffect[1].Play();
            foreach (MeshRenderer m in meshRenderers)
                m.enabled = false;
        }

    }

    private void OnEnable()
    {
        if (meshRenderers != null)
        {
            foreach (MeshRenderer m in meshRenderers)
                m.enabled = true;
        }
    }
}

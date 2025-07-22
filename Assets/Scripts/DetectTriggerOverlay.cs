using System;
using UnityEditor;
using UnityEngine;

public class DetectTriggerOverlay : MonoBehaviour
{ 
    public bool isInRange;

    public GameObject Dummy;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Dummy"))
        {
            isInRange = true;
            Dummy = col.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Dummy"))
        {
            isInRange = false;
            Dummy = null;
        }
    }
}

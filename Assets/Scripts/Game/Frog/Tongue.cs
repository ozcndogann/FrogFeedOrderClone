using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tongue : MonoBehaviour
{
    private Frog parentFrog;

    private void Start()
    {
        
        parentFrog = GetComponentInParent<Frog>();
        if (parentFrog == null)
        {
            Debug.LogError("Parent object does not have a Frog script.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (parentFrog != null)
        {
            parentFrog.OnTongueTriggerEnter(other);
        }
    }
}

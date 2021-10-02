using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatorActivator : MonoBehaviour
{
    public Rotator rotator;
    public bool triggerState;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            rotator.SetDragable(triggerState);

            Debug.Log("trigger " + triggerState);
        }
    }
}

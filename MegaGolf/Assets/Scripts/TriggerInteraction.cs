using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerInteraction : MonoBehaviour
{
    [SerializeField] private UnityEvent interacted;

    private void OnTriggerEnter(Collider other)
    {
        interacted.Invoke();
    }
}

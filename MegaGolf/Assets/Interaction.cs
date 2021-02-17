using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interaction : MonoBehaviour
{
    [SerializeField] private UnityEvent interacted;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        interacted.Invoke();
    }
}

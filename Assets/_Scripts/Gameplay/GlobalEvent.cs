using System;
using Horror.Events;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class GlobalEvent: MonoBehaviour
{
    [SerializeField] private Collider triggerCollider;
    [SerializeField] private GlobalEventMetadata globalEventMetadata;

    private void OnTriggerEnter(Collider other)
    {
        gameObject.SetActive(false);
    }
}
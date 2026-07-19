using System;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private bool needKey;
    [SerializeField] private Animator _animator;
    private bool isToggleDoor = false;
    private static int OpenHash = Animator.StringToHash("Open");
    private static int CloseHash = Animator.StringToHash("Close");
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        InvokeRepeating("ResetDoor", 5, 1);
    }

    public Transform Transform => transform;
    public bool CanPerform() => !needKey || GlobalSettings.Gameplay.HasKey;
    
    public void Interact(InteractContext context)
    {
        if (!CanPerform()) return;
        if (!isToggleDoor)
        {
            isToggleDoor = true;
            _animator.Play(OpenHash);
        }
    }

    private void ResetDoor()
    {
        isToggleDoor = true;
        _animator.Play(CloseHash);
    }
    public void FinishDoorAnim()
    {
        isToggleDoor = false;
    }
}
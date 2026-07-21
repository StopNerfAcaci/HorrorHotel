using System;
using UnityEngine;

public class Door : MonoBehaviour, IEnvironment
{
    [SerializeField] private bool needKey;
    [SerializeField] private Animator _animator;
    private bool isToggleDoor = false;
    private static int OpenHash = Animator.StringToHash("Open");
    private static int CloseHash = Animator.StringToHash("Close");

    public float Delay => GetClipLength(_animator, "Open");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        // InvokeRepeating("ResetDoor", 5, 1);
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
        if(!isToggleDoor) return;
        isToggleDoor = true;
        _animator.Play(CloseHash);
    }

    public void FinishDoorAnim()
    {
        isToggleDoor = false;
    }

    static float GetClipLength(Animator animator, string clipName)
    {
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;

        foreach (var clip in clips)
        {
            if (clip.name == clipName)
            {
                return clip.length;
            }
        }

        return .7f;
    }
}
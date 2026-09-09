using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private bool needKey;
    [SerializeField] private Animator _animator;
    [SerializeField] private string sceneAnimName;
    private bool isToggleDoor = false;
    private static int OpenHash = Animator.StringToHash("Open");
    private static int CloseHash = Animator.StringToHash("Close");

    public float Delay => GetClipLength(_animator, "Open");
    public bool IsCutScene => false;
    public string PlayerAnimName => null;
    public string SceneAnimName => sceneAnimName;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        // InvokeRepeating("ResetDoor", 5, 1);
    }

    public ItemSO Item { get; }
    public Transform Transform => transform;
    public bool CanInteract() => !needKey;

    public void Interact(InteractContext context)
    {
        if (!isToggleDoor)
        {
            isToggleDoor = true;
            _animator.Play(OpenHash);
        }
    }
    public void SetHighlighted(bool on)
    {
        
    }

    public UniTask Confirm()
    {
        return UniTask.CompletedTask;
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
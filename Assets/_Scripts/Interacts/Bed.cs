using Cysharp.Threading.Tasks;
using UnityEngine;
using VitalRouter;

public class Bed : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemSO item;
    [SerializeField] private float delay = 2f;
    [SerializeField] private string playerAnimName;
    private Collider _col;
    public bool IsCutScene => false;
    public string PlayerAnimName => playerAnimName;

    public ItemSO Item => item;
    public Transform Transform => transform;
    private Router _publisher;

    private void Awake()
    {
        _col = GetComponent<Collider>();
        _col.enabled = true;
    }
    

    public UniTask Confirm()
    {
        GameplayManager.Instance?.HandleNextPhase();
        _col.enabled = false;
        return UniTask.CompletedTask;
    }

    public bool CanInteract() => GameplayManager.Instance.CanMoveNextPhase();

    public void Interact(InteractContext ctx)
    {
        if (!CanInteract())
        {
            Debug.Log("Require all current progress done");
            // _publisher.PublishAsync(new PopupCommand(PopupType.NotDoneProgress));
            return;
        }

    }

    public void SetHighlighted(bool on)
    {
    }
}
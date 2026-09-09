using GameCore.MVP;
using TMPro;
using UnityEngine;
using Utils.Extensions;
using VitalRouter;

public class MaingameScreen : UIView
{
    [SerializeField] private DialogueController dialogueController;
    [SerializeField] private ObjectPreviewMenu objectPreviewMenu;
    [SerializeField] private RectTransform crosshair;
    [SerializeField] private float followSpeed = 15f;
    private Camera mainCam;
    private RectTransform canvasRect;
    private Router router;

    private bool _isHovering;
    private Vector3 _hoverWorldPos;

    public void Setup(UIManager manager)
    {
        objectPreviewMenu?.Setup(manager);
        mainCam = Camera.main;
        canvasRect = crosshair.GetComponentInParent<Canvas>().transform as RectTransform;
        Interaction.OnHover += BeginHover;
        Interaction.OnInspect += InspectBegin;
        Interaction.OnFinishInspect += Hide;
    }

    protected override void OnDestroy()
    {
        Interaction.OnHover -= BeginHover;
        Interaction.OnInspect -= InspectBegin;
        Interaction.OnFinishInspect -= Hide;
    }

    private void InspectBegin(IItem item)
    {
        _isHovering = false;
        objectPreviewMenu.Inspect(item.Item);
        Show();
    }

    private void BeginHover(Vector3 pos, bool hovering)
    {
        _isHovering = hovering;
        _hoverWorldPos = pos;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (crosshair == null || canvasRect == null) return;

        if (_isHovering)
        {
            crosshair.SetActive(true);
            Vector3 screenPoint = mainCam.WorldToScreenPoint(_hoverWorldPos);
            if (screenPoint.z > 0f)
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPoint, null,
                    out Vector2 targetAnchoredPos);
                crosshair.anchoredPosition = targetAnchoredPos;
            }
        }
        else if (crosshair.gameObject.activeInHierarchy)
        {
            crosshair.anchoredPosition = Vector2.Lerp(
                crosshair.anchoredPosition, Vector2.zero, Time.deltaTime * followSpeed);
            crosshair.SetActive(false);
        }
    }
}
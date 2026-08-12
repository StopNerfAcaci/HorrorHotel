using GameCore.MVP;
using TMPro;
using UnityEngine;
using UnityServiceLocator;
using VitalRouter;

[Routes]
public partial class MaingameScreen : UIView, IMenu<UIContainer>
{
    [SerializeField] TMP_Text dialogueText;
    [SerializeField] private RectTransform crosshair;
    [SerializeField] private float followSpeed = 15f;
    private Camera mainCam;
    private RectTransform canvasRect;
    private Router router;

    private bool isHovering;
    private Vector3 hoverWorldPos;

    public void Setup(UIContainer container)
    {
        dialogueText.gameObject.SetActive(false);
        mainCam = Camera.main;
        canvasRect = crosshair.GetComponentInParent<Canvas>().transform as RectTransform;
        ServiceLocator.Global.Get<Router>(out router);
        MapTo(router);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy() => UnmapRoutes();

    [Route]
    private void On(InspectCommand command)
    {
        isHovering = command.isHovering;
        hoverWorldPos = command.position;
    }

    [Route]
    private void On(EndInspectCommand command)
    {
        isHovering = false;
    }

    [Route]
    private void On(DialogueDisplayCommand command)
    {
        dialogueText.gameObject.SetActive(false);
        var id = command.id;
        // _dialogueController.Play(id);
        // dialogueText.text = data.Text;
        dialogueText.gameObject.SetActive(true);
    }
    private void Update()
    {
        if (crosshair == null || canvasRect == null) return;

        if (isHovering)
        {
            Vector3 screenPoint = mainCam.WorldToScreenPoint(hoverWorldPos);
            if (screenPoint.z > 0f)
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPoint, null, out Vector2 targetAnchoredPos);
                crosshair.anchoredPosition = targetAnchoredPos;
            }
        }
        else
        {
            crosshair.anchoredPosition = Vector2.Lerp(
                crosshair.anchoredPosition, Vector2.zero, Time.deltaTime * followSpeed);
        }
    }
}
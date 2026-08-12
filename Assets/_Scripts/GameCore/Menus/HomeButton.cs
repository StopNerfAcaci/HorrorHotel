using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HomeButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TMP_Text content;
    [SerializeField] private Image buttonImage;

    [SerializeField] private Color hoverTxtColor = Color.black;
    [SerializeField] private Color defaultTxtColor = Color.white;
    [SerializeField] private Color disabledColor = new Color(0.5f, 0.5f, 0.5f);

    private Button _button;
    private bool isHovering;
    Coroutine imageTransitionCoroutine;

    private void OnValidate()
    {
        if (buttonImage == null) buttonImage = GetComponent<Image>();
        if (content == null)
        {
            content = GetComponentInChildren<TMP_Text>();
        }

        if (_button == null)
            _button = GetComponent<Button>();
    }

    public void Setup(Action clickAction)
    {
        OnValidate();
        buttonImage.fillAmount = 0;
        buttonImage.type = Image.Type.Filled;
        buttonImage.fillMethod = Image.FillMethod.Horizontal;
        buttonImage.fillOrigin = 0;
        content.fontStyle = FontStyles.Normal;
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(() =>
        {
            isHovering = false;
            clickAction?.Invoke();
        });
    }

    private void OnEnable()
    {
        isHovering = false;
        ApplyColor();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
        ApplyColor();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
        ApplyColor();
    }

    private void ApplyColor()
    {
        if (_button != null && !_button.interactable)
        {
            content.color = disabledColor;
            return;
        }

        content.color = isHovering ? hoverTxtColor : defaultTxtColor;
        if (imageTransitionCoroutine != null)
        {
            StopCoroutine(imageTransitionCoroutine);
        }

        imageTransitionCoroutine = StartCoroutine(FillImage());
    }

    public IEnumerator FillImage()
    {
        content.fontStyle = isHovering? FontStyles.Bold :  FontStyles.Normal;
        buttonImage.DOFillAmount(isHovering ? 1 : 0, 0.15f);
        yield return new WaitForSeconds(.15f);
    }
}
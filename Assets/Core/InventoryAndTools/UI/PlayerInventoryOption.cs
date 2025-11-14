using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerInventoryOption : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [field: SerializeField]
    private TMP_Text toolNameText;
    [field: SerializeField]
    private Image toolImage;
    internal Action<bool> onHover;
    internal void Init(ATool tool, Action<bool> onhoverEvent)
    {
        toolNameText.text = tool.ToolName;
        toolImage.sprite = tool.ToolSprite;
        onHover = onhoverEvent;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        onHover.Invoke(false); 
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        onHover.Invoke(true);
    }
}

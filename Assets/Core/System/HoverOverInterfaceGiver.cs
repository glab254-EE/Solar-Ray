using TMPro;
using UnityEngine;

public class HoverOverInterfaceGiver : MonoBehaviour
{
    [field:SerializeField]
    private Transform CanvasTransform;
    [field:SerializeField]
    private GameObject GivenInterface;
    [field:SerializeField]
    internal string LabelText;
    internal bool IsHoveringOver {get;private set;} = false;
    private GameObject currentUI;
    void OnMouseEnter()
    {
        if (CanvasTransform == null)
        {
            CanvasTransform = FindAnyObjectByType<Canvas>().transform;
        }
        if (currentUI == null && GivenInterface != null)
        {
            currentUI = Instantiate(GivenInterface,CanvasTransform);
            if (LabelText != "" && currentUI.TryGetComponent(out TMP_Text text))
            {
                text.text = LabelText;
            }
        }
        IsHoveringOver = true;
    }
    void OnMouseExit()
    {
        if (currentUI != null)
        {
            Destroy(currentUI);
        }
        IsHoveringOver = false;
    }
}


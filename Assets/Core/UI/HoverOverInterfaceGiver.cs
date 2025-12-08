using TMPro;
using UnityEngine;

public class HoverOverInterfaceGiver : MonoBehaviour
{
    [field:SerializeField]
    private float MaximumGivenDistance = 10f;
    [field:SerializeField]
    private Transform CanvasTransform;
    [field:SerializeField]
    private GameObject GivenInterface;
    [field:SerializeField]
    internal string LabelText;
    internal bool IsHoveringOver {get;private set;} = false;
    private GameObject currentUI;
    private Transform cameraTransform;
    void Start()
    {
        cameraTransform = Camera.main.transform;
    }
    void FixedUpdate()
    {
        if (Vector3.Distance(transform.position,cameraTransform.position) <= MaximumGivenDistance
        && Physics.Raycast(new Ray(cameraTransform.position,cameraTransform.forward),out RaycastHit hit,MaximumGivenDistance)
        && hit.transform == transform)
        {
            OnMouseOver();
        } else
        {
            OnMouseExit();
        }
    }
    void OnMouseOver()
    {
        if (CanvasTransform == null)
        {
            CanvasTransform = GameObject.FindGameObjectWithTag("MainCanvas").transform;
        }
        if (currentUI == null && GivenInterface != null)
        {
            currentUI = Instantiate(GivenInterface,Vector3.zero,Quaternion.identity,CanvasTransform);
            if (LabelText != "" && currentUI.TryGetComponent(out TMP_Text text))
            {
                text.text = LabelText;
            }
        }
        IsHoveringOver = true;  
    }
    void OnMouseEnter()
    {
        if (CanvasTransform == null)
        {
            CanvasTransform = GameObject.FindGameObjectWithTag("MainCanvas").transform;
        }
        if (currentUI == null && GivenInterface != null)
        {
            currentUI = Instantiate(GivenInterface,Vector3.zero,Quaternion.identity,CanvasTransform);
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


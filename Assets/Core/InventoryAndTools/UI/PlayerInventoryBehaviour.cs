using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerInventoryBehaviour : MonoBehaviour
{

    [field: SerializeField]
    private GameObject InventoryMainFrame;
    [field: SerializeField]
    private Transform PrefabsParent;
    [field: SerializeField]
    private GameObject UIPrefab;

    [field: SerializeField]
    private PlayerCameraController CameraControler;
    [field: SerializeField]
    private PlayerHotbarCoreBehaviour PlayerHotbar;
    [field: SerializeField]
    private GameObject ToolDisplayPort;
    [field: SerializeField]
    private Vector3 ToolDisplayPosition;
    [field: SerializeField]
    private TMP_Text ToolDisplayDescription;
    private GameObject ViewedObject;
    private bool IsVisible = false;
    private InputSystem_Actions inputActions;
    void Start()
    {
        inputActions = new();
        inputActions.Player.OpenInventory.performed += ToggleInventory;
        inputActions.Player.OpenInventory.Enable();
        UpdateList();
    }
    void Update()
    {
        if (InventoryMainFrame.activeInHierarchy != IsVisible)
        {
            InventoryMainFrame.SetActive(IsVisible);
        }
    }
    void OnDestroy()
    {
        inputActions.Player.OpenInventory.performed -= ToggleInventory;        
    }
    private void ToggleInventory(InputAction.CallbackContext _)
    {
        IsVisible = !IsVisible;
        CameraControler.ToggleLock();
    }
    internal void UpdateList()
    {
        List<ATool> tools = BackpackStaticClass.Instance.backpack;
        if (tools == null) return;

        if (PrefabsParent.childCount > 0)
        {
            foreach (Transform ch in PrefabsParent)
            {
                Destroy(ch);
            }
        }

        for (int i = 0; i < tools.Count; i++)
        {
            int index = i+1;
            GameObject newbutton = Instantiate(UIPrefab, PrefabsParent);
            if (newbutton.TryGetComponent(out Button button))
            {
                button.onClick.AddListener(() =>
                {
                    PlayerHotbar.EquipTool(index);
                    CameraControler.ToggleLock();
                    if (ViewedObject != null)
                    {
                        Destroy(ViewedObject);
                    }
                    IsVisible = false;
                }
                );
            }
            if (newbutton.TryGetComponent(out PlayerInventoryOption option))
            {
                ATool tool = tools[i];
                option.Init(tool,bl =>
                {
                    if (!bl)
                    {
                        if (ViewedObject != null)
                        {
                            Destroy(ViewedObject);
                        }
                        ToolDisplayPort.SetActive(false);
                    } else
                    {
                        ToolDisplayDescription.text = tool.ToolDescription;
                        ViewedObject = Instantiate(tool.ViewModel, ToolDisplayPosition, Quaternion.identity);
                        ToolDisplayPort.SetActive(true);
                    }
                });
            }
        }
    }
}

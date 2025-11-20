using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHotbarCoreBehaviour : MonoBehaviour
{
    [field: SerializeField]
    private List<ATool> startingPack;
    [field: SerializeField]
    private PlayerToolBehaviour playerToolBehaviour;
    private int currentSelection = -1;
    private BackpackStaticClass backpack;
    private InputSystem_Actions inputActions;
    void Awake()
    {
        backpack = BackpackStaticClass.Instance;
        backpack.ResetBackpack(startingPack);
    }
    void Start()
    {
        Debug.Log(backpack.backpack);
        inputActions = new();
        ConnectInputs();
        inputActions.PlayerInventorySelection.Enable();
    }
    private void OnDestroy()
    {
        inputActions.PlayerInventorySelection.Disable();        
    }
    private void ConnectInputs()
    {
        InputActionMap map = inputActions.PlayerInventorySelection.Get();
        foreach(InputAction action in map.actions)
        {
            if (int.TryParse(action.name,out int index))
            {
                action.performed += _ =>
                {
                    EquipTool(index);
                };
            }
        }
    }
    internal void EquipTool(int toolIndex)
    {
        toolIndex -= 1;
        if (toolIndex < 0 || toolIndex >= backpack.backpack.Count || currentSelection == toolIndex)
        {
            currentSelection = -1;
            playerToolBehaviour.EquipTool(null);
            return;
        }
        ATool tool = backpack.backpack.ElementAt(toolIndex);
        if (backpack.backpack.Contains(tool) && tool != null)
        {
            playerToolBehaviour.EquipTool(tool);
            currentSelection = toolIndex;
        }
        Debug.Log(toolIndex);
    }
}

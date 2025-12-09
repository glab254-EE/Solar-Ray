using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class PlayerToolBehaviour : MonoBehaviour
{
    [field: SerializeField]
    private PlayerHealthBehaviour playerHP;
    [field: SerializeField]
    private ATool currentTool;
    [field: SerializeField]
    private GameObject currentToolObject = null;
    private Dictionary<FirearmSO,bool> Cooldowns = new();
    private InputSystem_Actions inputActions;
    private bool equiped;
    private bool lefftMouseButtonHeld = false;
    void Start()
    {
        inputActions = new();
        ConnectInputs();
        inputActions.Player.Attack.Enable();
    }
    void Update()
    {
        if (playerHP != null && playerHP.Health <= 0)
        {
            return;
        }
        if (currentTool != null && currentTool.ViewModel != null && currentToolObject == null)
        {
            OnEquip();
        }
        if (currentTool == null && currentToolObject == null)
        {
            equiped = false;
            lefftMouseButtonHeld = false;
        }
        if (equiped && lefftMouseButtonHeld)
        {
            if (currentTool is FirearmSO firearm)
            {
                if (!Cooldowns.ContainsKey(firearm) || !Cooldowns[firearm])
                {
                    StartCoroutine(firearm.FirearmShoot(currentToolObject));       
                    StartCoroutine(CooldownEnumerator(firearm));             
                }
            }
        }
    }
    void OnDestroy()
    {
        inputActions.Player.Attack.Disable();
    }
    private IEnumerator CooldownEnumerator(FirearmSO tool)
    {
        if (!Cooldowns.ContainsKey(tool))
        {
            Cooldowns.Add(tool,true);
        } else
        {
            Cooldowns[tool] = true;
        }
        yield return new WaitForSeconds(tool.FireCooldown);
        Cooldowns[tool] = false;
    }
    private void ConnectInputs()
    {
        inputActions.Player.Attack.performed += OnLeftMouseButtonPress;
        inputActions.Player.Attack.canceled += OnLeftMouseButtonPress;
    }
    private void OnLeftMouseButtonPress(InputAction.CallbackContext callbackContext)
    {
        if (!equiped) return;
        lefftMouseButtonHeld = callbackContext.ReadValueAsButton();
        if (lefftMouseButtonHeld)
        {
            currentTool.OnLeftMouseButtonPresss(gameObject, currentToolObject);            
        }
    }
    private void OnEquip()
    {
        if (currentTool != null && currentTool.ViewModel != null && currentToolObject == null)
        {
            currentToolObject = Instantiate(currentTool.ViewModel,transform);
            currentTool.OnEquip(gameObject, currentToolObject);
            equiped = true;
        }
    }
    internal void EquipTool(ATool tool)
    {
        currentTool = null;
        if (currentToolObject != null)
        {
            Destroy(currentToolObject);
            currentToolObject = null;
        }
        if (tool != null) 
        {
            currentTool = tool;
            OnEquip();
        }
    }
}

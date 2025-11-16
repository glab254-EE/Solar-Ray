using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerToolBehaviour : MonoBehaviour
{
    [field: SerializeField]
    private PlayerHealthBehaviour playerHP;
    private ATool currentTool;
    private GameObject currentToolObject;
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
            if (currentTool is FirearmSO firearm && !firearm.OnCooldown)
            {
                StartCoroutine(firearm.FirearmShoot(currentToolObject.transform));
                if (currentToolObject != null&&currentToolObject.TryGetComponent(out Animator animator))
                {
                    animator.SetTrigger("Shoot");
                } 
            }
        }
    }
    void OnDestroy()
    {
        inputActions.Player.Attack.Disable();
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
        }
        if (tool != null) 
        {
            currentTool = tool;
            OnEquip();
        }
    }
}

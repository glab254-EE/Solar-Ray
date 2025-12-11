using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(HoverOverInterfaceGiver))]
public class AttributeGivingButton : MonoBehaviour
{
    [field:SerializeField]
    private float DefaultCost = 4f;
    [field:SerializeField]
    private string CostAttribute;
    [field:SerializeField]
    private string AddedAttribute;
    [field:SerializeField]
    private float BuyCooldown = 2f;
    [field:SerializeField]
    private ATool givenTool;
    private HoverOverInterfaceGiver hoverOverInterfaceGiver;
    private bool OnCoodown = false;
    [field:SerializeField]
    private string DisplayText = "";
    private float lastCost;
    void Start()
    {
        hoverOverInterfaceGiver = GetComponent<HoverOverInterfaceGiver>();
        hoverOverInterfaceGiver.LabelText = DisplayText+DefaultCost.ToString();
    }
    void Update()
    {
        if (hoverOverInterfaceGiver.IsHoveringOver && Mouse.current.leftButton.isPressed && !OnCoodown)
        {
            TryBuy();
            hoverOverInterfaceGiver.LabelText = DisplayText+lastCost.ToString();
        }
    }
    private void TryBuy()
    {
        if (GlobalAttributesManager.Instance.TryGetAttribute(out float cost, CostAttribute, DefaultCost))
        {
            lastCost = cost;
            OnCoodown = true;
            if (BuildingManager.instance.TryBuyUpgrade(cost))
            {
                GlobalAttributesManager.Instance.TryAddAttribute(AddedAttribute);
                GlobalAttributesManager.Instance.TryAddAttribute(CostAttribute);
                if (givenTool != null)
                {
                    List<ATool> newBackpack = BackpackStaticClass.Instance.backpack;
                    newBackpack.Insert(0,givenTool);
                    BackpackStaticClass.Instance.ResetBackpack(newBackpack);

                    PlayerInventoryBehaviour playerInventoryBehaviour = GameObject.FindAnyObjectByType<PlayerInventoryBehaviour>();
                    if (playerInventoryBehaviour != null)
                    {
                        playerInventoryBehaviour.UpdateList();
                    }
                    GlobalAttributesManager.Instance.TryGetAttribute(out float lastCost, CostAttribute, DefaultCost);
                }
            }
            StartCoroutine(CooldownEnumerator());
        }
    }
    private IEnumerator CooldownEnumerator()
    {
        OnCoodown = true;
        yield return new WaitForSeconds(BuyCooldown);
        OnCoodown = false;
    }
}

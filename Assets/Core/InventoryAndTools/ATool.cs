using UnityEngine;

public abstract class ATool : ScriptableObject
{
    internal abstract string ToolName { get; set; }
    internal abstract string ToolDescription { get; set; }
    internal abstract GameObject ViewModel { get; set; }
    internal abstract Sprite ToolSprite { get; set; }
    internal abstract void OnEquip(GameObject PlayerFirearmsOriginObject, GameObject toolobject);
    internal abstract void OnLeftMouseButtonPresss(GameObject PlayerFirearmsOriginObject, GameObject toolObject);
}

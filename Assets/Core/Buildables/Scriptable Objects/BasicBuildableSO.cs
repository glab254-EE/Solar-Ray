using UnityEngine;

[CreateAssetMenu(fileName = "BasicBuildableSO", menuName = "Scriptable Objects/Buildables/Basic Buildable")]
public class BasicBuildable : ABuildableSO
{
    [field:SerializeField]
    public override GameObject Model { get; protected set; }
    [field:SerializeField]
    public override float BuildCost { get; protected set; }
    public override void OnPlace(GameObject gameObject, ABuildableSO built)
    {

    }
}

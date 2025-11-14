using UnityEngine;

//[CreateAssetMenu(fileName = "BuildableSO", menuName = "Scriptable Objects/BuildableSO")]
public abstract class ABuildableSO : ScriptableObject
{
    public abstract GameObject Model { get; protected set; }
    public abstract int MaxBuildCount { get; protected set; }
    public abstract void OnPlace(GameObject gameObject);
}

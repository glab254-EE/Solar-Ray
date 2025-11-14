using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Building Tool",menuName ="Scriptable Objects/Player Tools/Building Tool")]
public class BuilderSO : ATool
{
    [field: SerializeField]
    internal override GameObject ViewModel { get; set; }
    [field: SerializeField]
    internal override Sprite ToolSprite { get; set; }
    [field: SerializeField]
    internal ABuildableSO builtObject { get; private set; }
    [field: SerializeField]
    internal float BuildMaxDistance { get; private set; }
    [field: SerializeField]
    internal string BuildAreasTag { get; set; } = "Build Areas";
    [field: SerializeField]
    internal override string ToolName { get; set; }
    [field: SerializeField]
    internal override string ToolDescription { get; set; }

    internal override void OnEquip(GameObject PlayerFirearmsOriginObject, GameObject toolobject)
    {
        Debug.Log("Equiped!");
    }

    internal override void OnLeftMouseButtonPresss(GameObject PlayerFirearmsOriginObject, GameObject toolObject)
    {
        bool CanBuild = true;
        if (CanBuild)
        {
            Transform camera = Camera.main.transform;
            if (Physics.Raycast(new Ray(camera.position, camera.forward), out RaycastHit hit, BuildMaxDistance))
            {
                if (hit.collider.gameObject != null && hit.collider.gameObject.CompareTag(BuildAreasTag))
                {
                    Vector3 forward = camera.forward;
                    Vector3 directionWithoutYAxis = new Vector3(forward.x, 0, forward.z).normalized;
                    BuildingManager.instance.TryPlaceObject(builtObject, hit.point, Quaternion.LookRotation(directionWithoutYAxis));
                }
            }        
        }
    }
}

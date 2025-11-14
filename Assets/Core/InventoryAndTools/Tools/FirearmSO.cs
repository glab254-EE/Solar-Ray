using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Firearm",menuName ="Scriptable Objects/Player Tools/Firearm")]
public class FirearmSO : ATool
{
    [field: SerializeField]
    internal override GameObject ViewModel { get; set; }
    [field: SerializeField]
    internal override Sprite ToolSprite { get; set; }
    [field: SerializeField]
    internal float FireCooldown { get; private set; }
    [field: SerializeField]
    private string shootingOriginName;
    [field: SerializeField]
    private float MaxSpread = 0f;
    [field: SerializeField]
    private AProjectileSO projectileSO;
    [field: SerializeField]
    private GameObject ProjectilePrefab;
    [field: SerializeField]
    private Vector3 ProjectileVelocity;
    [field: SerializeField]
    internal override string ToolName { get; set; }
    [field: SerializeField]
    internal override string ToolDescription { get; set; }
    internal bool OnCooldown { get; private set; } = false;
    internal override void OnEquip(GameObject PlayerFirearmsOriginObject, GameObject toolobject)
    {
        Debug.Log("Equiped!");
    }

    internal override void OnLeftMouseButtonPresss(GameObject PlayerFirearmsOriginObject, GameObject toolObject)
    {
        Debug.Log("Pew start!");
    }
    internal IEnumerator FirearmShoot(Transform toolTransform)
    {
        if (toolTransform == null || OnCooldown) yield break;
        Transform neworigin = toolTransform.Find(shootingOriginName) ?? toolTransform;
        Quaternion rotation = neworigin.rotation;
        rotation.eulerAngles += new Vector3(Random.Range(-MaxSpread, MaxSpread), Random.Range(-MaxSpread, MaxSpread), Random.Range(-MaxSpread, MaxSpread));
        if (ProjectilePrefab != null)
        {
            GameObject projectile = Instantiate(ProjectilePrefab,neworigin.position,rotation);
            if (projectile.TryGetComponent(out Rigidbody rb))
            {
                rb.linearVelocity = projectile.transform.forward * ProjectileVelocity.z + projectile.transform.right * ProjectileVelocity.x + neworigin.up * ProjectileVelocity.y;
            }
            if (projectile.TryGetComponent(out ProjectileBehaviour behaviour))
            {
                behaviour.Initialize(projectileSO,"Player");
            }
        }
        OnCooldown = true;
        yield return new WaitForSeconds(FireCooldown);
        OnCooldown = false;
    }
}

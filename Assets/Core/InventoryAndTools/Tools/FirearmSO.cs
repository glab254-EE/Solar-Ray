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
    private Vector3 ProjectileVelocity;
    [field: SerializeField]
    internal override string ToolName { get; set; }
    [field: SerializeField]
    internal override string ToolDescription { get; set; }
    private bool OnCooldown = false;
    private void OnEnable()
    {
        OnCooldown = false;
    }
    internal override void OnEquip(GameObject PlayerFirearmsOriginObject, GameObject toolobject)
    {
    }

    internal override void OnLeftMouseButtonPresss(GameObject PlayerFirearmsOriginObject, GameObject toolObject)
    {
    }
    internal IEnumerator FirearmShoot(GameObject toolGO)
    {
        Transform toolTransform= toolGO.transform;
        Debug.Log(OnCooldown);
        if (toolTransform == null || OnCooldown == true) yield break;
        Transform neworigin = toolTransform.Find(shootingOriginName) ?? toolTransform;
        Debug.Log("Shooting loop start, finding origin");
        OnCooldown = true;

        if (toolTransform.TryGetComponent(out Animator animator))
        {
            animator.SetTrigger("Shoot");
        } 
        if (toolTransform.TryGetComponent(out AudioSource source))
        {
            source.Play();
        }
        ParticleSystem system = toolTransform.GetComponentInChildren<ParticleSystem>();
        if (system != null)
        {
            system.Emit(10);
        }

        Vector3 vector = neworigin.forward;
        Vector3 newSpeed = 
        (vector+Vector3.one
        *Random.Range(
            Mathf.Deg2Rad*-MaxSpread,
            Mathf.Deg2Rad*MaxSpread
            )
        ).normalized
        *ProjectileVelocity.z;


        ProjectileManager.Instance.ShootProjectile(neworigin.position,newSpeed,projectileSO);


        yield return new WaitForSeconds(FireCooldown);

        OnCooldown = false;
    }
}

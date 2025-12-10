using UnityEngine;

[RequireComponent(typeof(PlayerHealthBehaviour))]
public class PlayerDeathGUIGiver : MonoBehaviour
{
    [field:SerializeField]
    private Transform UIParent;
    [field:SerializeField]
    private GameObject givenUI;
    void Start()
    {
        if (UIParent == null)
        {
            UIParent = GameObject.FindWithTag("MainCanvas")?.transform;
        }
        PlayerHealthBehaviour behaviour = GetComponent<PlayerHealthBehaviour>();
        behaviour.OnPlayerDeath += OnDeath;
    }
    private void OnDeath()
    {
        Instantiate(givenUI,UIParent);
    }
}

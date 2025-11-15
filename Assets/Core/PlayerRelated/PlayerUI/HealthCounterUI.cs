using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class HealthCounterUI : MonoBehaviour
{
    [field:SerializeField]
    private PlayerHealthBehaviour playerHealth;
    private TMP_Text textLabel;
    void Start()
    {
        textLabel = GetComponent<TMP_Text>();
    }
    void Update()
    {
        textLabel.text = playerHealth.Health.ToString()+"/"+playerHealth.MaxHealth.ToString();
    }
}

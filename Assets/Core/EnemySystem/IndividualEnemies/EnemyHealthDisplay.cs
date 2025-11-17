using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class EnemyHealthDisplay : MonoBehaviour
{
    [field:SerializeField]
    private EnemyHealthHandler handler;
    private TMP_Text textLabel;
    void Start()
    {
        textLabel = GetComponent<TMP_Text>();
    }
    void Update()
    {
        textLabel.text = handler.Health.ToString()+" HP";
    }
}

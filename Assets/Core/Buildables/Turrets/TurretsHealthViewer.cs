using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class TurretsHealthViewer : MonoBehaviour
{
    [field:SerializeField]
    private TurretBehaviour tower;
    private TMP_Text textLabel;
    void Start()
    {
        textLabel = GetComponent<TMP_Text>();
    }
    void Update()
    {
        textLabel.text = tower.Health.ToString()+" HP";
    }
}

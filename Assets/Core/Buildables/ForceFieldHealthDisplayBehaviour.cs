using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class ForceFieldHealthDisplayBehaviour : MonoBehaviour
{
    [field:SerializeField]
    private BasicDestructable handler;
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

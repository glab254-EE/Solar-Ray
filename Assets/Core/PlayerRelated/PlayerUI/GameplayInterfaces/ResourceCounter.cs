using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class ResourceCounter : MonoBehaviour
{
    private BuildingManager manager;
    private TMP_Text textLabel;
    void Start()
    {
        textLabel = GetComponent<TMP_Text>();
        manager = BuildingManager.instance;
    }
    void Update()
    {
        textLabel.text = manager.Resources.ToString();
    }
}

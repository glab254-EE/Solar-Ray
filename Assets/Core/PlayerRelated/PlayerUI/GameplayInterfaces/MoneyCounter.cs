using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class MoneyCounter : MonoBehaviour
{
    private TMP_Text textLabel;
    void Start()
    {
        textLabel = GetComponent<TMP_Text>();
    }
    void Update()
    {
        textLabel.text = BuildingManager.Money.ToString();
    }
}

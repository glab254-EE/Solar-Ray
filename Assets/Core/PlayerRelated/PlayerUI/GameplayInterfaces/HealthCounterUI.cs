using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class HealthCounterUI : MonoBehaviour
{
    [field:SerializeField]
    private PlayerHealthBehaviour playerHealth;
    [field:SerializeField]
    private List<Sprite> HealthSprites = new();
    private Image imageLabel;
    void Start()
    {
        imageLabel = GetComponent<Image>();
    }
    void Update()
    {
        int newindex = Mathf.RoundToInt((playerHealth.Health+1)/HealthSprites.Count);
        if (newindex >= 0 && newindex < HealthSprites.Count)
        {
            Sprite newsprite = HealthSprites[newindex];
            if (imageLabel.sprite != newsprite)
            {
                imageLabel.sprite = newsprite;
            }
        }
    }
}

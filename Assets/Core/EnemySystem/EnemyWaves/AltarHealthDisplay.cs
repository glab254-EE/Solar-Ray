using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class AltarHealthDisplay : MonoBehaviour
{
     [field:SerializeField]
    private EnemyAltarBehaviour altarHealth;
    [field:SerializeField]
    private List<Sprite> HealthSprites = new();
    private Image imageLabel;
    private float MaxHealth;
    void Start()
    {
        imageLabel = GetComponent<Image>();
        MaxHealth = altarHealth.Health;
    }
    void Update()
    {
        int newindex = Mathf.RoundToInt(altarHealth.Health/MaxHealth*HealthSprites.Count);
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

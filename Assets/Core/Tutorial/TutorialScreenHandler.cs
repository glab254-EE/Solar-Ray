using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialScreenHandler : MonoBehaviour
{
    [field:SerializeField]
    private Button Clicker;
    [field:SerializeField]
    private TMP_Text TextLabel;
    [field:SerializeField]
    private List<string> Strings;
    void Start()
    {
        Clicker.onClick.AddListener(ProceedSlide);
    }
    private void ProceedSlide()
    {
        if (Strings.Count < 1)
        {
            Destroy(gameObject);            
        } else
        {
            TextLabel.text = Strings[0];
            Strings.RemoveAt(0);            
        }
    }
}

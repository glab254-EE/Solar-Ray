using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ExitGameOnClick : MonoBehaviour
{

    void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(ExitGame);
    }
    void ExitGame()
    {
        Application.Quit(0);
    }
}

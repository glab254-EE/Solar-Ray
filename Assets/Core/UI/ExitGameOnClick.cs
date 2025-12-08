using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ExitGameOnClick : MonoBehaviour
{

    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(ExitGame);
    }
    void ExitGame()
    {
        Application.Quit(0);
    }
}

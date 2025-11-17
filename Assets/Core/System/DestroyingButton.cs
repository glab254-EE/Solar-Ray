using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Button))]
public class DestroyingButton : MonoBehaviour
{
    [field:SerializeField]
    private GameObject ToDestroy;
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(OnClick);
    }
    void OnClick()
    {
        Destroy(ToDestroy);
    }
}

using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Button))]
public class DestroyingButton : MonoBehaviour
{
    [field:SerializeField]
    private GameObject ToDestroy;
    [field:SerializeField]
    private float TimeScaleOnStart = 1f;
    [field:SerializeField]
    private float TimeScaleOnDestroy = 1f;
    void Start()
    {
        Time.timeScale = TimeScaleOnStart;
        gameObject.GetComponent<Button>().onClick.AddListener(OnClick);
    }
    void OnClick()
    {
        Time.timeScale = TimeScaleOnDestroy;
        Destroy(ToDestroy);
        if (gameObject != null)
        {
            gameObject.SetActive(false);
        }
    }
}

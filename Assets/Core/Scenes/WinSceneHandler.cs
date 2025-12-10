using UnityEngine;

public class WinSceneHandler : MonoBehaviour
{
    [field:SerializeField]
    private int SceneToLoad;
    public void Trigger()
    {
        Cursor.lockState = CursorLockMode.None;
        GameScenesManager.LoadScene(SceneToLoad);
    }
}

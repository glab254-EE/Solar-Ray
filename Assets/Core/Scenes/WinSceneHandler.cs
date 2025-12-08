using UnityEngine;

public class WinSceneHandler : MonoBehaviour
{
    [field:SerializeField]
    private int SceneToLoad;
    public void Trigger()
    {
        GameScenesManager.LoadScene(SceneToLoad);
    }
}

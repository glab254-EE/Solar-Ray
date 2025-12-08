using UnityEngine;

public class WinSceneHandler : MonoBehaviour
{
    [field:SerializeField]
    private int SceneToLoad;
    internal void Trigger()
    {
        GameScenesManager.LoadScene(SceneToLoad);
    }
}

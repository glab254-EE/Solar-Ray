using UnityEngine;
using UnityEngine.SceneManagement;

public class GameScenesManager
{
    public int AvailableLastSceneIndex {get;internal set;} = 1;
    public const int MainMenuScene = 0;
    private static GameScenesManager source;
    public static GameScenesManager Instance
    {
        get
        {
            source ??= new();
            return source;
        }
    }
    public static void LoadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public static void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }
}

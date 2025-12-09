using System.IO;
using UnityEngine;

public class SceneIndexerSaver : MonoBehaviour
{
    [field:SerializeField]
    private int DefaultMaxScene = 1;
    [field:SerializeField]
    private string FileName;
    private GameScenesManager gameScenesManager;
    void Awake() // as this only saves data from other classes, and dont recieve any, singleton is not used here.
    {
        gameScenesManager = GameScenesManager.Instance;
        gameScenesManager.AvailableLastSceneIndex = LoadLastsceneFromFile();
    }
    void OnApplicationQuit()
    {
        SaveFile();
    }
    private int LoadLastsceneFromFile()
    {
        int output = DefaultMaxScene;
        try
        {
            string path = Path.Combine(Application.persistentDataPath,"Resources",FileName,".json");
            if (File.Exists(path))
            {
                string readfile = File.ReadAllText(path);
                int.TryParse(readfile,out output);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogWarning(e.Message);
        }
        return output;
    }
    private void SaveFile()
    {
      
        try
        {
            string filepathJSON = Path.Combine(Application.persistentDataPath,FileName+".json");
            if (!File.Exists(filepathJSON))
            {
                File.Create(filepathJSON);
            }
            File.WriteAllText(filepathJSON,gameScenesManager.AvailableLastSceneIndex.ToString());
        }
        catch (System.Exception)
        {
            
            throw;
        }      
    }
}

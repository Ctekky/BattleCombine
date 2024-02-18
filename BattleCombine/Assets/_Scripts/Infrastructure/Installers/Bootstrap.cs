using UnityEngine;
using UnityEngine.SceneManagement;

public static class PerformBootstrap
{
    private const string SceneName = "BootstrapScene";
    
    public static void Execute()
    {
        for (var sceneIndex = 0; sceneIndex < SceneManager.sceneCount; sceneIndex++)
        {
            var candidate = SceneManager.GetSceneAt(sceneIndex);
            if(candidate.name == SceneName)
                return;
        }

        SceneManager.LoadScene(SceneName, LoadSceneMode.Additive);
    }
}
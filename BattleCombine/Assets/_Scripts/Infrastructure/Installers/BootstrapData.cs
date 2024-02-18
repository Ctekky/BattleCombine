using UnityEngine;
using UnityEngine.SceneManagement;

public class BootstrapData : MonoBehaviour
{
    [SerializeField] private string nextSceneName;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private void Start()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
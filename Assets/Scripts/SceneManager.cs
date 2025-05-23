using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerController : MonoBehaviour
{
    public static SceneManagerController Instance;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    
    public void ReloadCurrentScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
    
    public void LoadSceneByName(string sceneName)
    {
        
        SceneManager.LoadScene(sceneName);
    }

    public void LoadNextScene()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
            ScoreManager.Instance.fixScore();
        }
        else
        {
            Debug.Log("Sahne bitti dayeeeeeeee");
            
        }
    }

    public void QuitGame()
    {
        Debug.Log("Çıkışşşş yapıyorum");
        Application.Quit();
    }
    
}

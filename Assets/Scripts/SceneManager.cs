using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic; 
public class SceneManagerController : MonoBehaviour
{
    public static SceneManagerController Instance;
    private Stack<string> sceneHistory = new Stack<string>();


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string sceneName = scene.name;
        if (sceneHistory.Count == 0 || sceneHistory.Peek() != sceneName)
        {
            sceneHistory.Push(sceneName);
        }
    }



    public void LoadPreviousScene()
    {
        if (sceneHistory.Count > 1)
        {
            sceneHistory.Pop();

            string previousScene = sceneHistory.Peek();
            ScoreManager.Instance.fixScore();
            SceneManager.LoadScene(previousScene);
        }
        else
        {
            Debug.LogWarning("Önceki sahne yok.");
        }
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
            Debug.Log("Sahne bitti");
            
        }
    }

    public void QuitGame()
    {
        Debug.Log("Çıkış yapılıyor.");
        Application.Quit();
    }
    
}

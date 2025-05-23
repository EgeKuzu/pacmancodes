using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int score = 0;
    public TextMeshProUGUI scoreText;

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

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        scoreText = GameObject.Find("ScoreText")?.GetComponent<TextMeshProUGUI>();
        UpdateScoreUI();
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    void Update()
    {
        GetToEndScreen();
    }

    public void GetToEndScreen()
    {
        if (score == 500)
        {
            if (GameFacade.Instance != null)
            {
                GameFacade.Instance.LoadNextScene();
            }
            else
            {
                SceneManagerController.Instance.LoadNextScene();
            }
        }
    }

    public void fixScore()
    {
        score = 0;
        UpdateScoreUI();
    }
}

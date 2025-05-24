using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartTheGame : MonoBehaviour
{
    public Button retryButton;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {

        retryButton.onClick.AddListener(() =>  SceneManager.LoadScene("Level1"));

    }
}
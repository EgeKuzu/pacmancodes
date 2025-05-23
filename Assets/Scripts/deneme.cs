using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class deneme : MonoBehaviour
{
    public Button retryButton;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        retryButton.onClick.AddListener(() => SceneManagerController.Instance.LoadSceneByName("Level1"));

    }
}

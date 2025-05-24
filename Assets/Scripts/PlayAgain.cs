using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class YenidenOyna : MonoBehaviour
{
    public Button retryButton;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {

        retryButton.onClick.AddListener(() => SceneManagerController.Instance.LoadPreviousScene());

    }


}
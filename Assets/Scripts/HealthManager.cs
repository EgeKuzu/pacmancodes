
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; 
public class HealthManager : MonoBehaviour
{
    public static HealthManager Instance;

    public int HP = 3;
    public TextMeshProUGUI HPText;

    private void Awake()
    {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
            SceneManager.sceneLoaded += OnSceneLoaded; 
        }

            
        else
            Destroy(gameObject);
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        HP = 3;

        HPText = GameObject.Find("HealthText")?.GetComponent<TextMeshProUGUI>();

    }

    public void DamageToPacman()
    {
        HP = HP - 1;
        UpdateHPUI();
    }

    private void UpdateHPUI()
    {
        if (HPText != null)
            HPText.text = "HP: " + GetCurrentHP();
    }
    
    public int GetCurrentHP(){
        return HP;
    }

    public void FixHP(){
        HP = 3;
    }
}
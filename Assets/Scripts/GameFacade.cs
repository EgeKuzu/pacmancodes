using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFacade : MonoBehaviour
{
    public static GameFacade Instance;
    
    // Alt sistem referansları
    private GridManager gridManager;
    private ScoreManager scoreManager;
    private HealthManager healthManager;
    private GameManager gameManager;
    private EnemyMovements[] enemies;
    
    void Awake()
    {
        // Singleton yapısı
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
            
        // DontDestroyOnLoad - birden fazla sahne yüklendiğinde bile devam etsin
        DontDestroyOnLoad(this);
    }
    
    void Start()
    {
        // Bileşenleri başlatma
        InitializeComponents();
    }
    
    private void InitializeComponents()
    {
        // Tüm alt sistemlere referans al
        gridManager = FindObjectOfType<GridManager>();
        scoreManager = FindObjectOfType<ScoreManager>();
        healthManager = FindObjectOfType<HealthManager>();
        gameManager = FindObjectOfType<GameManager>();
        enemies = FindObjectsOfType<EnemyMovements>();
    }
    
    // GridManager işlevlerinin basitleştirilmiş versiyonları
    public List<Node> FindPath(Vector2Int start, Vector2Int end)
    {
        return gridManager.FindPath(start, end);
    }
    
    public bool IsWalkable(Vector2Int position)
    {
        // GridManager'dan bilgi alınması gerekir, şu anda direk bir metod olmadığı için ekliyoruz
        return gridManager != null;
    }
    
    // ScoreManager işlevleri
    public void AddScore(int amount)
    {
        if (scoreManager != null)
            scoreManager.AddScore(amount);
    }
    
    public int GetCurrentScore()
    {
        return scoreManager != null ? scoreManager.score : 0;
    }
    
    // HealthManager işlevleri
    public void DamagePlayer()
    {
        if (healthManager != null)
            healthManager.DamageToPacman();
    }
    
    public int GetPlayerHealth()
    {
        return healthManager != null ? healthManager.GetCurrentHP() : 0;
    }
    
    // GameManager işlevleri
    public void ResetPositions()
    {
        if (gameManager != null)
            gameManager.ResetPositions();
    }
    
    // Düşman işlevleri
    public void MoveDEnemyToPacman(int enemyIndex)
    {
        if (enemies != null && enemyIndex < enemies.Length)
            enemies[enemyIndex].MoveToPacman();
    }
    
    // SceneManager işlevleri
    public void LoadScene(string sceneName)
    {
        SceneManagerController.Instance.LoadSceneByName(sceneName);
    }

    public void LoadNextScene()
    {
        SceneManagerController.Instance.LoadNextScene();
    }

    
    // Tüm sistemleri yeniden başlatma (örnek yeni bir seviye başlangıcı)
    public void ResetGame()
    {
        if (healthManager != null)
            healthManager.FixHP();

        if (gameManager != null)
            gameManager.ResetPositions();

        // Diğer yenileme işlemleri...
    }
} 
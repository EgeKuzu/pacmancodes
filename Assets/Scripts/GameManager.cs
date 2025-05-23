using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Transform pacman;
    public Transform[] ghosts;
    public Vector3 pacmanStartPos;
    public Vector3[] ghostStartPositions;
    private Coroutine spawnCoroutine;
    [SerializeField] private Vector3 spawnPoint;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        
        pacman = GameObject.FindGameObjectWithTag("Pacman")?.transform;

        GameObject[] ghostObjects = GameObject.FindGameObjectsWithTag("Ghost");
        ghosts = ghostObjects.Select(g => g.transform).ToArray();

        
        pacmanStartPos = pacman != null ? pacman.position : Vector3.zero;
    }

    void Start()
    {
        pacmanStartPos = pacman.position;

        StartCoroutine(SpawnGhosts(10f)); 
    }

    public void ResetPositions()
    {
        
    pacman.position = pacmanStartPos;

    
    for (int i = 0; i < ghosts.Length; i++)
    {
        ghosts[i].position = ghostStartPositions[i];     
        EnemyMovements enemyScript = ghosts[i].GetComponent<EnemyMovements>();
        if (enemyScript != null)
        {
            enemyScript.canMove = false;
        }
    }

    
        if (spawnCoroutine != null)
            StopCoroutine(spawnCoroutine);

        spawnCoroutine = StartCoroutine(SpawnGhosts(10f));


        StartCoroutine(EnableGhostsAfterDelay(1f));
    }



    IEnumerator EnableGhostsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        for (int i = 0; i < ghosts.Length; i++)
        {
            EnemyMovements enemyScript = ghosts[i].GetComponent<EnemyMovements>();
            if (enemyScript != null)
                enemyScript.canMove = true;
        }
       
    }
  
    IEnumerator SpawnGhosts(float delay)
    {
        for (int i = 0; i < ghosts.Length; i++)
        {
            yield return new WaitForSeconds(delay);

            ghosts[i].position = spawnPoint;

            EnemyMovements enemyScript = ghosts[i].GetComponent<EnemyMovements>();
            if (enemyScript != null)
                enemyScript.canMove = true;
        }

        spawnCoroutine = null;
    }
}


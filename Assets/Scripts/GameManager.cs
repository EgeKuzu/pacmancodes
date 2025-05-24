using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        pacman = GameObject.FindGameObjectWithTag("Pacman")?.transform;

        GameObject[] ghostObjects = GameObject.FindGameObjectsWithTag("Ghost");
        ghosts = ghostObjects.Select(g => g.transform).ToArray();

        ghostStartPositions = ghosts.Select(g => g.position).ToArray();

        if (ghosts.Length > 0 && pacman != null)
        {
            pacmanStartPos = pacman.position;
            spawnCoroutine = StartCoroutine(SpawnGhosts(1f));
        }
    }

    public void ResetPositions()
    {
        if (pacman != null)
            pacman.position = pacmanStartPos;

        if (ghosts != null && ghosts.Length == ghostStartPositions.Length)
        {
            for (int i = 0; i < ghosts.Length; i++)
            {
                ghosts[i].position = ghostStartPositions[i];

                EnemyMovements enemyScript = ghosts[i].GetComponent<EnemyMovements>();
                if (enemyScript != null)
                    enemyScript.canMove = false;
            }

            if (spawnCoroutine != null)
                StopCoroutine(spawnCoroutine);

            spawnCoroutine = StartCoroutine(SpawnGhosts(1f));
            StartCoroutine(EnableGhostsAfterDelay(1f));
        }
    }

    IEnumerator EnableGhostsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        foreach (var ghost in ghosts)
        {
            var enemyScript = ghost.GetComponent<EnemyMovements>();
            if (enemyScript != null)
                enemyScript.canMove = true;
        }
    }
    IEnumerator SpawnGhosts(float delay)
    {
        if (ghosts == null || ghosts.Length == 0)
        {
            yield break; 
        }

        for (int i = 0; i < ghosts.Length; i++)
        {
            if (i == 0)
                yield return new WaitForSeconds(delay);
            else
                yield return new WaitForSeconds(5f);

            if (i >= ghosts.Length) yield break; 

            if (ghosts[i] == null) continue;

            ghosts[i].position = spawnPoint;

            var enemyScript = ghosts[i].GetComponent<EnemyMovements>();
            if (enemyScript != null)
                enemyScript.canMove = true;
        }

        spawnCoroutine = null;
    }

}

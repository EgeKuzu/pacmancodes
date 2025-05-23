using UnityEngine;
using System.Collections.Generic;

public class ChaseState : IEnemyState
{
    private float pathTimer = 0f;
    
    public void EnterState(EnemyMovements enemy)
    {
        enemy.SetColor(Color.white); // Kovalama rengini ayarla
    }
    
    public void ExitState(EnemyMovements enemy)
    {
        // Çıkış işlemleri
    }
    
    public void Update(EnemyMovements enemy)
    {
        pathTimer += Time.deltaTime;

        if (pathTimer >= enemy.pathRefreshRate)
        {
            pathTimer = 0f;
            ChaseTarget(enemy);
        }
    }
    
    public void OnPacmanCollision(EnemyMovements enemy)
    {
        // State deseni ve Facade deseni birlikte kullanımı
        if (GameFacade.Instance != null)
        {
            GameFacade.Instance.DamagePlayer();
            GameFacade.Instance.ResetPositions();
        }
        else
        {
            HealthManager.Instance.DamageToPacman();
            GameManager.Instance.ResetPositions();
        }
    }
    
    private void ChaseTarget(EnemyMovements enemy)
    {
        Vector2Int start = new Vector2Int(Mathf.FloorToInt(enemy.transform.position.x), Mathf.FloorToInt(enemy.transform.position.y));
        Vector2Int target = new Vector2Int(Mathf.FloorToInt(enemy.pacman.position.x), Mathf.FloorToInt(enemy.pacman.position.y));

        List<Node> path = enemy.gridManager.FindPath(start, target);
        if (path != null && path.Count > 0)
        {
            Vector3 targetPos = new Vector3(path[0].Position.x + 0.5f, path[0].Position.y + 0.5f, 0f);
            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, targetPos, enemy.moveSpeed * Time.deltaTime);
        }
    }
} 
using UnityEngine;
using System.Collections.Generic;

public class FrightenedState : IEnemyState
{
    private float pathTimer = 0f;
    private float stateDuration = 8f; // Korkma süresi
    private float stateTimer = 0f;
    
    public void EnterState(EnemyMovements enemy)
    {
        enemy.SetColor(Color.blue); // Korkma durumu rengi
        enemy.moveSpeed = enemy.moveSpeed * 0.5f; // Yavaşlatma
        stateTimer = 0f;
    }
    
    public void ExitState(EnemyMovements enemy)
    {
        enemy.moveSpeed = enemy.moveSpeed * 2f; // Normal hıza geri dön
    }
    
    public void Update(EnemyMovements enemy)
    {
        stateTimer += Time.deltaTime;
        if (stateTimer >= stateDuration)
        {
            // Süre dolduğunda Chase moduna geçiş yap
            enemy.SetState(new ChaseState());
            return;
        }
            
        pathTimer += Time.deltaTime;
        if (pathTimer >= enemy.pathRefreshRate)
        {
            pathTimer = 0f;
            RunAwayFromPacman(enemy);
        }
    }
    
    public void OnPacmanCollision(EnemyMovements enemy)
    {
        // Korkma durumunda Pacman düşmanı yiyebilir
        // Düşman yeniden doğar ve skor eklenir
        if (GameFacade.Instance != null)
        {
            GameFacade.Instance.AddScore(200); // Ekstra puan
            enemy.RespawnAtHome();
            enemy.SetState(new ChaseState());
        }
        else
        {
            ScoreManager.Instance.AddScore(200);
            enemy.RespawnAtHome();
            enemy.SetState(new ChaseState());
        }
    }
    
    private void RunAwayFromPacman(EnemyMovements enemy)
    {
        // Pacman'ın tam ters yönünde bir nokta hesapla
        Vector2 pacmanPos = enemy.pacman.position;
        Vector2 enemyPos = enemy.transform.position;
        Vector2 directionFromPacman = (enemyPos - pacmanPos).normalized;
        
        // Hedef, Pacman'dan belirli bir mesafe uzakta olsun
        Vector2 targetPos = enemyPos + directionFromPacman * 5f;
        
        // Grid sınırlarını kontrol et
        Vector2Int gridTarget = new Vector2Int(
            Mathf.Clamp(Mathf.FloorToInt(targetPos.x), 0, enemy.gridManager.width - 1),
            Mathf.Clamp(Mathf.FloorToInt(targetPos.y), 0, enemy.gridManager.height - 1)
        );
        
        Vector2Int start = new Vector2Int(Mathf.FloorToInt(enemy.transform.position.x), Mathf.FloorToInt(enemy.transform.position.y));
        
        List<Node> path = enemy.gridManager.FindPath(start, gridTarget);
        if (path != null && path.Count > 0)
        {
            Vector3 nextPos = new Vector3(path[0].Position.x + 0.5f, path[0].Position.y + 0.5f, 0f);
            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, nextPos, enemy.moveSpeed * Time.deltaTime);
        }
    }
} 
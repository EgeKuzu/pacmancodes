using UnityEngine;
using System.Collections.Generic;

public class ScatterState : IEnemyState
{
    private float pathTimer = 0f;
    private Vector2Int scatterTarget; // Düşmanın kaçınma noktası
    private float stateDuration = 7f; // Durum süresi
    private float stateTimer = 0f;
    
    public ScatterState(Vector2Int cornerPosition)
    {
        scatterTarget = cornerPosition;
    }
    
    public void EnterState(EnemyMovements enemy)
    {
        enemy.SetColor(Color.white); // Saçılma modu rengi
        stateTimer = 0f;
    }
    
    public void ExitState(EnemyMovements enemy)
    {
        // Çıkış işlemleri
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
            ScatterMovement(enemy);
        }
    }
    
    public void OnPacmanCollision(EnemyMovements enemy)
    {
        // Scatter durumunda da aynı davranış
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
    
    private void ScatterMovement(EnemyMovements enemy)
    {
        Vector2Int start = new Vector2Int(Mathf.FloorToInt(enemy.transform.position.x), Mathf.FloorToInt(enemy.transform.position.y));
        
        List<Node> path = enemy.gridManager.FindPath(start, scatterTarget);
        if (path != null && path.Count > 0)
        {
            Vector3 targetPos = new Vector3(path[0].Position.x + 0.5f, path[0].Position.y + 0.5f, 0f);
            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, targetPos, enemy.moveSpeed * Time.deltaTime);
        }
    }
} 
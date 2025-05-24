using UnityEngine;
using System.Collections.Generic;

public class FrightenedState : IEnemyState
{
    private float pathTimer = 0.01f;
    private float stateDuration = 3f;
    private float stateTimer = 0f;
    public float fearDistance = 15f;
    private Queue<Vector3> currentPath = new Queue<Vector3>();
    public void EnterState(EnemyMovements enemy)
    {
        enemy.SetColor(Color.blue); // Korkma durumu rengi
        enemy.moveSpeed = enemy.moveSpeed * 1.25f; // Yavaşlatma
        stateTimer = 0f;
    }

    public void ExitState(EnemyMovements enemy)
    {
        enemy.moveSpeed = enemy.moveSpeed * 0.8f; // Normal hıza geri dön
    }

    public void OnPacmanCollision(EnemyMovements enemy)
    {
        if (GameFacade.Instance != null)
        {
            enemy.RespawnAtHome();
            enemy.SetState(new ChaseState());
        }
        else
        {
            enemy.RespawnAtHome();
            enemy.SetState(new ChaseState());
        }
    }

    public void Update(EnemyMovements enemy)
    {
        stateTimer += Time.deltaTime;

        if (stateTimer >= stateDuration)
        {
            enemy.SetState(new ChaseState());
            return;
        }

        pathTimer += Time.deltaTime;
        if (pathTimer >= enemy.pathRefreshRate || currentPath.Count == 0)
        {
            pathTimer = 0.01f;
            CalculateRunAwayPath(enemy);
        }

        FollowPath(enemy);
    }

    private void CalculateRunAwayPath(EnemyMovements enemy)
    {
        Vector2 pacmanPos = enemy.pacman.position;
        Vector2 enemyPos = enemy.transform.position;

        float distance = Vector2.Distance(pacmanPos, enemyPos);
        if (distance > fearDistance) return;

        Vector2 direction = (enemyPos - pacmanPos).normalized;
        Vector2 target = enemyPos + direction * 5f;

        Vector2Int gridTarget = new Vector2Int(
            Mathf.Clamp(Mathf.FloorToInt(target.x), 0, enemy.gridManager.width - 1),
            Mathf.Clamp(Mathf.FloorToInt(target.y), 0, enemy.gridManager.height - 1)
        );
        Vector2Int start = new Vector2Int(Mathf.FloorToInt(enemyPos.x), Mathf.FloorToInt(enemyPos.y));

        List<Node> path = enemy.gridManager.FindPath(start, gridTarget);
        currentPath.Clear();
        if (path != null && path.Count > 0)
        {
            foreach (Node node in path)
            {
                Vector3 pos = new Vector3(node.Position.x + 0.5f, node.Position.y + 0.5f, 0f);
                currentPath.Enqueue(pos);
            }
        }
    }

    private void FollowPath(EnemyMovements enemy)
    {
        if (currentPath.Count == 0) return;

        Vector3 target = currentPath.Peek();
        enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, target, enemy.moveSpeed * Time.deltaTime);

        if (Vector3.Distance(enemy.transform.position, target) < 0.05f)
            currentPath.Dequeue();
    }
} 
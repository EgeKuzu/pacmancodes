using UnityEngine;

public interface IEnemyState
{

    void Update(EnemyMovements enemy);
    
    
    void EnterState(EnemyMovements enemy);
    
    
    void ExitState(EnemyMovements enemy);
    
    void OnPacmanCollision(EnemyMovements enemy);
} 
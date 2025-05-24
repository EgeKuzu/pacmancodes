using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPellet : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Pacman"))
        {
            ActivateFrightened();
            
            if (GameFacade.Instance != null)
                GameFacade.Instance.AddScore(50);
            else
                ScoreManager.Instance.AddScore(50);
                
            Destroy(gameObject);
        }
    }
    
    private void ActivateFrightened()
    {
        EnemyMovements[] enemies = FindObjectsOfType<EnemyMovements>();
        foreach (EnemyMovements enemy in enemies)
        {
            enemy.SetFrightenedState();
        }
    }
} 
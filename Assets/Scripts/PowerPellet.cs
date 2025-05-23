using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPellet : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Pacman"))
        {
            // Power-up etkinleştir
            ActivateFrightened();
            
            // Puan ekle
            if (GameFacade.Instance != null)
                GameFacade.Instance.AddScore(50);
            else
                ScoreManager.Instance.AddScore(50);
                
            // Power-up'ı yok et
            Destroy(gameObject);
        }
    }
    
    private void ActivateFrightened()
    {
        // Tüm düşmanları bul ve korkmuş duruma getir
        EnemyMovements[] enemies = FindObjectsOfType<EnemyMovements>();
        foreach (EnemyMovements enemy in enemies)
        {
            enemy.SetFrightenedState();
        }
    }
} 
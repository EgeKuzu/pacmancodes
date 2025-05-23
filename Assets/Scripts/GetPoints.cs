using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Dot : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Pacman"))
        {
            if (GameFacade.Instance != null)
            {
                GameFacade.Instance.AddScore(10);
            }
            else
            {
                
                ScoreManager.Instance.AddScore(10);
                AudioManager.Instance.PlaySFX(AudioManager.Instance.eatPelletSound);
            }
            Destroy(gameObject);
        }
    }

}


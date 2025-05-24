using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovements : MonoBehaviour
{
    public Transform pacman;
    public float moveSpeed = 2f;
    public GridManager gridManager;

    public float pathRefreshRate = 0.001f;

    private Vector2Int currentGridPos => new Vector2Int(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y));
    public bool canMove = true;

    private IEnemyState currentState;
    private SpriteRenderer spriteRenderer;
    public Vector3 homePosition; 

    void Start()
    {
        canMove = true;
        spriteRenderer = GetComponent<SpriteRenderer>();
        homePosition = transform.position;
        SetInitialState();
    }

    private void SetInitialState()
    {
        SetState(new ChaseState());
    }

    void FixedUpdate()
    {
        currentState.Update(this);
    }

    public void SetState(IEnemyState newState)
    {
        if (currentState != null)
            currentState.ExitState(this);

        currentState = newState;

        if (currentState != null)
            currentState.EnterState(this);
    }

    // Düşman rengini ayarla
    public void SetColor(Color color)
    {
        if (spriteRenderer != null)
            spriteRenderer.color = color;
    }

    // Düşmanı ev pozisyonuna döndür
    public void RespawnAtHome()
    {
        transform.position = homePosition;
    }

    // Bu metod daha önce private idi, şimdi GameFacade tarafından kullanılabilmesi için public yapıldı
    public void MoveToPacman()
    {
        // Artık bu metod durum üzerinden çağrılıyor
        if (currentState == null)
            SetState(new ChaseState());

        // Doğrudan hareketi zorlamak için
        if (currentState is ChaseState)
            ((ChaseState)currentState).Update(this);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Pacman") && currentState != null)
        {
            // Çarpışma işlemlerini mevcut duruma delege et
            currentState.OnPacmanCollision(this);
        }
    }

    // Power-up yendiğinde çağrılacak metod (örn. büyük noktalar)
    public void SetFrightenedState()
    {
        SetState(new FrightenedState());
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovements : MonoBehaviour
{
    public Transform pacman;
    public float moveSpeed = 2f;
    public GridManager gridManager;

    public float pathRefreshRate = 0.01f; 

    private Vector2Int currentGridPos => new Vector2Int(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y));
    public bool canMove = true;
    
    private IEnemyState currentState;
    private SpriteRenderer spriteRenderer;
    public Vector3 homePosition; // Düşmanın başlangıç/ev pozisyonu
    
    private float stateChangeTimer = 0f;
    private float stateChangeInterval = 20f; // Düzenli olarak Scatter ve Chase arası geçiş
    
    void Start()
    {
        canMove = true;
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // Ev pozisyonunu kaydet
        homePosition = transform.position;
        
        // Başlangıç durumunu ayarla - her düşman farklı köşelere scatter yapabilir
        SetInitialState();
    }
    
    private void SetInitialState()
    {
        // Her düşman için farklı bir köşe pozisyonu belirle
        Vector2Int scatterPos;
        
        // Düşman sırasına göre farklı köşeler
        if (name.Contains("1"))
            scatterPos = new Vector2Int(1, gridManager.height - 2);
        else if (name.Contains("2"))
            scatterPos = new Vector2Int(gridManager.width - 2, gridManager.height - 2);
        else if (name.Contains("3"))
            scatterPos = new Vector2Int(1, 1);
        else // 4 veya diğerleri
            scatterPos = new Vector2Int(gridManager.width - 2, 1);
            
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
    
    private void ToggleScatterChase()
    {
        if (currentState is ChaseState)
        {
            // Düşman sırasına göre farklı köşeler
            Vector2Int scatterPos;
            if (name.Contains("1"))
                scatterPos = new Vector2Int(1, gridManager.height - 2);
            else if (name.Contains("2"))
                scatterPos = new Vector2Int(gridManager.width - 2, gridManager.height - 2);
            else if (name.Contains("3"))
                scatterPos = new Vector2Int(1, 1);
            else // 4 veya diğerleri
                scatterPos = new Vector2Int(gridManager.width - 2, 1);

            SetState(new ScatterState(scatterPos));
        }
        else if (currentState is ScatterState)
        {
            SetState(new ChaseState());
        }
        // Frightened durumunu bozmuyoruz
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

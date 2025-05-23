using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacManMovement : MonoBehaviour
{

    public float moveForce = 10f;
    public float maxSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 direction = Vector2.right;
    private Vector2 lastDirection = Vector2.right;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        HandleInput();
        HandleDirectionChange();
        RotatePacman();
        PacmanGG();
    }

    void FixedUpdate()
    {
        RestrictMovement();
        MovePacman();
    }
    
    void HandleInput()
    {
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            direction = Vector2.up;
        else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            direction = Vector2.down;
        else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            direction = Vector2.left;
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            direction = Vector2.right;
    }
    



    void HandleDirectionChange()
    {
        if (direction != lastDirection)
        {
            rb.velocity = Vector2.zero;
            lastDirection = direction;
        }
    }

    void MovePacman()
    {
            if (rb.velocity.magnitude < maxSpeed)
            {
                rb.AddForce(direction * moveForce);
            }
    
    }

    void RotatePacman()
    {
        float angle = 0f;

        if (direction == Vector2.up)
            angle = 90f;
        else if (direction == Vector2.down)
            angle = -90f;
        else if (direction == Vector2.left)
            angle = 180f;
        else if (direction == Vector2.right)
            angle = 0f;

        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    void RestrictMovement()
    {
        Vector2 velocity = rb.velocity;

        if (direction == Vector2.right || direction == Vector2.left)
            velocity.y = 0f; 
        if (direction == Vector2.up || direction == Vector2.down)
            velocity.x = 0f; 

        rb.velocity = velocity;
    }

    void PacmanGG(){
        // Facade deseni kullanımı
        if (GameFacade.Instance != null)
        {
            if (GameFacade.Instance.GetPlayerHealth() == 0)
            {
                GameFacade.Instance.LoadScene("DeathScreen");
            }
        }
        else
        {
            // Geriye dönük uyumluluk
            if(HealthManager.Instance.GetCurrentHP() == 0){
                SceneManagerController.Instance.LoadSceneByName("DeathScreen");
            }
        }
    }


}




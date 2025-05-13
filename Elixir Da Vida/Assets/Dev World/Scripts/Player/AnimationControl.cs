using System.Runtime.InteropServices;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Velocidade de movimento do jogador
    private Vector2 input;
    public Rigidbody2D rb;        // Referência ao componente Rigidbody2D do jogador

    public Animator anim;     // Referência ao componente Animator do jogador
    private Vector2 lastMovement;      // Vetor para armazenar a direção do movimento
    bool facingLeft = true;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        
    }

    void Update()
    {
        ProcessInputs();
        Animate();
        if(input.x<0 && !facingLeft || input.x>0 && facingLeft){
            Flip();
        }
    }

        // FixedUpdate é chamado em intervalos de tempo fixos e é melhor para física
    void FixedUpdate(){
        // Aplica o movimento ao Rigidbody2D
        rb.linearVelocity = input * moveSpeed;
    }

    void ProcessInputs(){

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if (moveX==0 && moveY==0 && (input.x != 0 || input.y != 0)){
            lastMovement = input;
        }

        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

    }

    void Animate(){
        anim.SetFloat("MoveX",input.x);
        anim.SetFloat("MoveY",input.y);
        anim.SetFloat("MoveMag",input.magnitude);
        anim.SetFloat("LastMoveX",lastMovement.x);
        anim.SetFloat("LastMoveY",lastMovement.y);
    }
    
    void Flip(){
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        facingLeft = !facingLeft;
    }

}

/*
    (antido update)

        // Obtém a entrada do jogador para movimento horizontal e vertical (-1, 0 ou 1)
        lastMovement.x = Input.GetAxisRaw("Horizontal");
        lastMovement.y = Input.GetAxisRaw("Vertical");

        // Atualiza os parâmetros do Animator para controlar as animações
        anim.SetFloat("Horizontal", lastMovement.x);
        anim.SetFloat("Vertical", lastMovement.y);
        anim.SetFloat("Speed", lastMovement.sqrMagnitude); // Usa a magnitude ao quadrado para otimização (0 ou > 0)
    



*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rbPlayer; //criar variável que só aceita valores relacionados ao Rigidbody2D

    [Header("Movimento")]
    [SerializeField] float speed = 6f;

    [Header("Pulo")]
    [SerializeField] float jumpForce = 15f;
    [SerializeField] bool canJump;
    [SerializeField] bool inFloor = true;
    [SerializeField] Transform groundCheck; //objeto de verificação
    [SerializeField] LayerMask groundLayer; //camada que será usada na verificação, chão

    Animator animPlayer;

    private void Awake()
    {
        animPlayer = GetComponent <Animator> ();
        rbPlayer = GetComponent<Rigidbody2D>(); //diz para a variável qual Rigidbody2D ela pode acessar
    }

    private void Update()
    {
        inFloor = Physics2D.Linecast(transform.position, groundCheck.position, groundLayer); //retorna um valor true ou false
        Debug.DrawLine(transform.position, groundCheck.position, Color.blue); //desenha a linha

        animPlayer.SetBool ("Jump", !inFloor);

        if (Input.GetButtonDown("Jump") && inFloor) //"Se o jogador pressionar o botão de pulo E o player estiver no chão"
        {
            canJump = true; //"Pulo autorizado"
        }
        //Caso  a de cima seja falsa...
        else if (Input.GetButtonDown("Jump") && rbPlayer.velocity.y > 0) //"Se o jogador pressionar o botão de pulo E o player estiver finalizando o impulso"
        {
            rbPlayer.velocity = new Vector2(rbPlayer.velocity.x, rbPlayer.velocity.y * 2); //"Pulo recebe um sobreimpulso"
        }
    }

    private void FixedUpdate() //50 vezes por segundo
    {
        Move();
        Jump();
    }

    void Move ()
    {
        float xMove = Input.GetAxis("Horizontal"); //atribui o valor do Input para a variável
        rbPlayer.velocity = new Vector2(xMove * speed, rbPlayer.velocity.y);

        animPlayer.SetFloat ("Speed", Mathf.Abs (xMove));

        if (xMove > 0)
        {
            transform.eulerAngles = new Vector2(0, 0); //mantém a angulação do objeto
            Debug.Log ("Direita");
        }
        else if (xMove < 0)
        {
            transform.eulerAngles = new Vector2(0, 180); //muda a angulação para o objeto se virar para extremo contrário
        }
    }

    void Jump()
    {
        if (canJump) //"Se o jogador puder pular"
        {
            rbPlayer.velocity = Vector2.up * jumpForce; //"Player recebe impulso para cima"
            canJump = false; //"Quando o jogador pular (a função ativa), o jogador não pode mais pular de novo"
        }
    }
}
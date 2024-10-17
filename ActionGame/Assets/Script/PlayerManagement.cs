using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerManagement : MonoBehaviour
{
    [SerializeField, Header("�ړ����x")]
    private float moveSpeed;
    [SerializeField, Header("�W�����v���x")]
    private float jumpSpeed;
    [SerializeField, Header("HP")]
    private int hp = 3;



    private Vector2 inputDirection;
    private Rigidbody2D rigid;
    private Animator anime;
    private bool bJump;

     void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        anime = GetComponent<Animator>();
        bJump = false;

    }

     void Update()
    {

        //LookMoveDirec();
        Move();

        LookMoveDirec();

    }

    private void Move()
    {
        rigid.velocity = new Vector2(inputDirection.x * moveSpeed, rigid.velocity.y);
        anime.SetBool("Walk", inputDirection.x != 0.0f);
    }

    private void LookMoveDirec()
    {
        if (inputDirection.x > 0.0f)
        {
            transform.eulerAngles = Vector3.zero;
        }
        else if (inputDirection.x < 0.0f)
        {
            transform.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Tilemap")
        {
            bJump = false;
            anime.SetBool("Jump", bJump);
        }
        if(collision.gameObject.tag == "Enemy")
        {
           bool enemyDefeated =  HitEnemy(collision.gameObject,collision);

            if(!enemyDefeated)
            {
                //�_���[�W���󂯂鏈��
                TakeDamage(1);

            }
        }
    }

    private bool HitEnemy(GameObject enemy, Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {

            //�v���C���[���G�̏ォ�瓖�����Ă��邩�`�F�b�N
            if(contact.normal.y > 0.5f)
            {
                Destroy(enemy);
                rigid.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
                return true;
            }
        }

        return false;

    }
    private void TakeDamage(int damage)
    {
        hp -= damage;

        Debug.Log(hp);
        if(hp <= 0)
        {
            Die();
        }
    }
    public int GetHp()
    {
        return hp;
    }


    private void Die()
    {
        Debug.Log("���S");
        if(hp <= 0)
        {
            Destroy(gameObject);
        }

    }


    public void OnMove(InputAction.CallbackContext context)
    {
        inputDirection = context.ReadValue<Vector2>();


    }


    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed || bJump) return;

        rigid.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
        bJump = true;
        anime.SetBool("Jump", bJump);

    }


}


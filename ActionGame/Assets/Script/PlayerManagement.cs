using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerManagement : MonoBehaviour
{
    [SerializeField, Header("移動速度")]
    private float moveSpeed;
    [SerializeField, Header("ジャンプ速度")]
    private float jumpSpeed;
    [SerializeField, Header("HP")]
    private int hp = 3;

    private int currentHp;


    private Vector2 inputDirection;
    private Rigidbody2D rigid;
    private Animator anime;
    private bool bJump;

     void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        anime = GetComponent<Animator>();
        bJump = false;

        //現在のHPを最大HPに設定
        currentHp = hp;
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
        }
        if(collision.gameObject.tag == "Enemy")
        {
           bool enemyDefeated =  HitEnemy(collision.gameObject,collision);

            if(!enemyDefeated)
            {
                //ダメージを受ける処理
                TakeDamage(1);

            }
        }
    }

    private bool HitEnemy(GameObject enemy, Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {

            //プレイヤーが敵の上から当たっているかチェック
            if(contact.normal.y > 0.5f)
            {
                Destroy(enemy);
                return true;
            }
        }

        return false;

    }
    private void TakeDamage(int damage)
    {
        currentHp -= damage;

        Debug.Log(currentHp);
        if(currentHp <= 0)
        {
            Die();
        }
    }


    private void Die()
    {
        Debug.Log("死亡");

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

    }

}


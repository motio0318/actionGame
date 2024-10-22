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

    [SerializeField, Header("無敵時間")]
    private float damageaTime;

    [SerializeField, Header("点滅時間")]
    private float flashTime;




    private Vector2 inputDirection;
    private Rigidbody2D rigid;
    private Animator anime;
    private SpriteRenderer spriteRenderer;
    private bool bJump;

     void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        anime = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        bJump = false;

    }

     void FixedUpdate()
    {

        //LookMoveDirec();
        Move();
        LookMoveDirec();

    }

    private void Move()
    {
        // 現在の位置を取得し、入力方向に基づいて新しい位置を計算
        Vector3 newPosition = transform.position + new Vector3(inputDirection.x, 0, 0) * moveSpeed * Time.deltaTime;
        // 計算した位置にオブジェクトを移動
        transform.position = newPosition;
        anime.SetBool("Walk", inputDirection.x != 0.0f);
    }

    private void LookMoveDirec()

    {
        Vector3 currentScale = transform.localScale;

        if (inputDirection.x > 0.0f)

        {

            // 右向きにする (スケールを正に設定)

            transform.localScale = new Vector3(Mathf.Abs(currentScale.x), transform.localScale.y, transform.localScale.z);

        }

        else if (inputDirection.x < 0.0f)

        {

            // 左向きにする (スケールを反転)

            transform.localScale = new Vector3(-Mathf.Abs(currentScale.x), transform.localScale.y, transform.localScale.z);

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
            gameObject.layer = LayerMask.NameToLayer("PlayerDamage");
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
                rigid.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
                return true;
            }
            else
            {
                StartCoroutine(Damage());

            }
        }

        return false;

    }


    IEnumerator Damage()
    {
        Color color = spriteRenderer.color;
        for(int i = 0;i < damageaTime; i++)
        {
            yield return new WaitForSeconds(flashTime);
            spriteRenderer.color = new Color(color.r, color.g, color.b, 0.0f);

            yield return new WaitForSeconds(flashTime);
            spriteRenderer.color = new Color(color.r, color.g, color.b, 1.0f);

        }
        spriteRenderer.color = color;
        gameObject.layer = LayerMask.NameToLayer("Default");


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
        Debug.Log("死亡");
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


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

    public Transform attackPoint;
    public float     attackRadius;
    public LayerMask enemyLayer;

     void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        anime = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        bJump = false;

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Attack();
        }

    }
    void FixedUpdate()
    {

        //LookMoveDirec();
        Move();
        LookMoveDirec();
        //HitFloor();

    }
    void Attack()
    {

        anime.SetTrigger("Is Attack");
        Debug.Log("攻撃");
        Collider2D[] hitenemys = Physics2D.OverlapCircleAll(attackPoint.position,attackRadius,enemyLayer);
        foreach(Collider2D hitEnemy in hitenemys)
        {
            Debug.Log(hitEnemy.gameObject.name + "に攻撃");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(attackPoint.position, attackRadius);
    }


    private void Move()
    {
        //if (bJump) return;
        
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
        //Vector3 currentScale = transform.localScale;

        //if (inputDirection.x > 0.0f)

        //{
        //    // 右向きにする (スケールを正に設定)
        //    transform.localScale = new Vector3(Mathf.Abs(currentScale.x), transform.localScale.y, transform.localScale.z);

        //}

        //else if (inputDirection.x < 0.0f)

        //{

        //    // 左向きにする (スケールを反転)

        //    transform.localScale = new Vector3(-Mathf.Abs(currentScale.x), transform.localScale.y, transform.localScale.z);

        //}

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Tilemap")
        {
            bJump = false;
            anime.SetBool("Jump", bJump);

        }
        if (collision.gameObject.tag == "Enemy")
        {
           bool enemyDefeated =  HitEnemy(collision.gameObject,collision);
            if(!enemyDefeated)
            {
                //ダメージを受ける処理
                TakeDamage(1);

            }
        }
        else if(collision.gameObject.tag == "Goal")
        {
            FindAnyObjectByType<MainManager>().ShowGameClearUI();
            enabled = false;
            GetComponent<PlayerInput>().enabled = false;

            anime.SetBool("Walk", inputDirection.x != 0.0f);

        }
    }

    //private void HitFloor()
    //{
    //    int layerMask = LayerMask.GetMask("Tilemap");
    //    float rayLength = 0.1f; // 地面の検出距離

    //    Vector2 collisionSize = GetComponent<BoxCollider2D>().size;

    //    Vector2 leftRayOrigin = (Vector2)transform.position + new Vector2(-collisionSize.x * transform.lossyScale.x / 2 + 0.1f, -collisionSize.y * transform.lossyScale.y / 2);
    //    Vector2 rightRayOrigin = (Vector2)transform.position + new Vector2(collisionSize.x * transform.lossyScale.x / 2 - 0.1f, -collisionSize.y * transform.lossyScale.y / 2);
    //    RaycastHit2D leftHit = Physics2D.Raycast(leftRayOrigin, Vector2.down, rayLength, layerMask);
    //    RaycastHit2D rightHit = Physics2D.Raycast(rightRayOrigin, Vector2.down, rayLength, layerMask);
    //    if (leftHit.collider != null || rightHit.collider != null)
    //    {
    //        // 地面に接触している
    //        if (bJump)
    //        {
    //            bJump = false;
    //            anime.SetBool("Jump", bJump);
    //        }
    //    }
    //    else
    //    {
    //        // 地面に接触していない
    //        if (!bJump)
    //        {
    //            bJump = true;
    //            anime.SetBool("Jump", bJump);
    //        }
    //    }
    //}
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
        gameObject.layer = LayerMask.NameToLayer("PlayerDamage");

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

    private void OnBecameInvisible()
    {
        Camera camera = Camera.main;

        if(camera != null)
        {
            if (camera.name == "Main Camera" && camera.transform.position.y > transform.position.y)
            {
                Destroy(gameObject);

            }

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


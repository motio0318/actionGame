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

    [SerializeField, Header("���G����")]
    private float damageaTime;

    [SerializeField, Header("�_�Ŏ���")]
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
        Debug.Log("�U��");
        Collider2D[] hitenemys = Physics2D.OverlapCircleAll(attackPoint.position,attackRadius,enemyLayer);
        foreach(Collider2D hitEnemy in hitenemys)
        {
            Debug.Log(hitEnemy.gameObject.name + "�ɍU��");
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
        //    // �E�����ɂ��� (�X�P�[���𐳂ɐݒ�)
        //    transform.localScale = new Vector3(Mathf.Abs(currentScale.x), transform.localScale.y, transform.localScale.z);

        //}

        //else if (inputDirection.x < 0.0f)

        //{

        //    // �������ɂ��� (�X�P�[���𔽓])

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
                //�_���[�W���󂯂鏈��
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
    //    float rayLength = 0.1f; // �n�ʂ̌��o����

    //    Vector2 collisionSize = GetComponent<BoxCollider2D>().size;

    //    Vector2 leftRayOrigin = (Vector2)transform.position + new Vector2(-collisionSize.x * transform.lossyScale.x / 2 + 0.1f, -collisionSize.y * transform.lossyScale.y / 2);
    //    Vector2 rightRayOrigin = (Vector2)transform.position + new Vector2(collisionSize.x * transform.lossyScale.x / 2 - 0.1f, -collisionSize.y * transform.lossyScale.y / 2);
    //    RaycastHit2D leftHit = Physics2D.Raycast(leftRayOrigin, Vector2.down, rayLength, layerMask);
    //    RaycastHit2D rightHit = Physics2D.Raycast(rightRayOrigin, Vector2.down, rayLength, layerMask);
    //    if (leftHit.collider != null || rightHit.collider != null)
    //    {
    //        // �n�ʂɐڐG���Ă���
    //        if (bJump)
    //        {
    //            bJump = false;
    //            anime.SetBool("Jump", bJump);
    //        }
    //    }
    //    else
    //    {
    //        // �n�ʂɐڐG���Ă��Ȃ�
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

            //�v���C���[���G�̏ォ�瓖�����Ă��邩�`�F�b�N
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
        Debug.Log("���S");
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


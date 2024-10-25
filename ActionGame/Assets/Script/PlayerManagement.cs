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
        //// ���݂̈ʒu���擾���A���͕����Ɋ�Â��ĐV�����ʒu���v�Z
        //Vector3 newPosition = transform.position + new Vector3(inputDirection.x, 0, 0) * moveSpeed * Time.deltaTime;
        //// �v�Z�����ʒu�ɃI�u�W�F�N�g���ړ�
        //transform.position = newPosition;
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
        if(collision.gameObject.tag == "Tilemap")
        {
            bJump = false;
            anime.SetBool("Jump", bJump);
        }
        else if(collision.gameObject.tag == "Enemy")
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


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField, Header("ˆÚ“®‘¬“x")]
    private float moveSpeed;

    public int hp = 3;
    Animator animator;
    private Rigidbody2D rigid;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        Move();


    }


    private void Move()
    {
        rigid.velocity = new Vector2(Vector2.left.x * moveSpeed, rigid.velocity.y);
    }

    public void OnDamage(int damage)
    {
        hp -= damage;
        if(hp <= 0)
        {
            Die();
        }
    }


    void Die()
    {
        hp = 0;
        Destroy(gameObject);
        //animator.SetTrigger("Die");
    }

}

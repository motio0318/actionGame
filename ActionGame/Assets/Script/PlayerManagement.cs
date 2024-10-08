using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerManagement : MonoBehaviour
{
    public LayerMask StageLayer;
    // �����Ԃ��`����
    private enum MOVE_TYPE
    {
        STOP,
        RIGHT,
        LEFT,
    }
    MOVE_TYPE move = MOVE_TYPE.STOP; // ������Ԃ�STOP
    Rigidbody2D rbody2D;             // Rigidbody2D���`
    float speed;                     // �ړ����x���i�[����ϐ�
    private void Start()
    {
        // Rigidbody2D�̃R���|�[�l���g���擾
        rbody2D = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        float horizonkey = Input.GetAxis("Horizontal");      // ���������̃L�[�擾
                                                             // �擾�������������̃L�[�����ɕ���
        if (horizonkey == 0)
        {
            // �L�[���͂Ȃ��̏ꍇ�͒�~
            move = MOVE_TYPE.STOP;
        }
        else if (horizonkey > 0)
        {
            // �L�[���͂����̏ꍇ�͉E�ֈړ�����
            move = MOVE_TYPE.RIGHT;
        }
        else if (horizonkey < 0)
        {
            // �L�[���͂����̏ꍇ�͍��ֈړ�����
            move = MOVE_TYPE.LEFT;
        }
        // space����������W�����v�֐���
        if (GroundChk())
        {
            if (Input.GetKeyDown("space"))
            {
                Jump();
            }
        }
    }
    // �������Z(rigidbody)��FixedUpdate�ŏ�������
    private void FixedUpdate()
    {
        //��]�̃^�[�Q�b�g�p�x�����߂�
        float targetRotation = 0f;
        Vector3 scale = transform.localScale;
        if (move == MOVE_TYPE.STOP)
        {
            speed = 0;
        }
        else if (move == MOVE_TYPE.RIGHT)
        {
            targetRotation = 0f; // �E����
            speed = 3;
        }
        else if (move == MOVE_TYPE.LEFT)
        {
            targetRotation = 180f; // ������
            speed = -3;
        }

        //�v���C���[�̉�]�����炩�ɖڕW�p�x�֕��
        Quaternion targetQuaternion = Quaternion.Euler(0, targetRotation, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetQuaternion, Time.deltaTime * 10f);//�����͕�ԑ��x�̒���

        //rigidbody2D��velocity(���x)�֎擾����speed������By�����͓����Ȃ��̂ł��̂܂܂ɂ���
        rbody2D.velocity = new Vector2(speed, rbody2D.velocity.y);
    }
    void Jump()
    {
        // ��ɗ͂�������
        rbody2D.AddForce(Vector2.up * 300);
    }
    bool GroundChk()
    {
        Vector3 startposition = transform.position;                     // Player�̒��S���n�_�Ƃ���
        Vector3 endposition = transform.position - transform.up * 1.40f; // Player�̑������I�_�Ƃ���
                                                                        // Debug�p�Ɏn�_�ƏI�_��\������
        Debug.DrawLine(startposition, endposition, Color.red);
        // Physics2D.Linecast���g���A�x�N�g����StageLayer���ڐG���Ă�����True��Ԃ�
        return Physics2D.Linecast(startposition, endposition, StageLayer);
    }
}

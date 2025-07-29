// Enemy1Script.cs �̒��g

using UnityEngine;
using System.Collections; // �R���[�`�����g�����߂ɕK�v

public class Enemy1Script : MonoBehaviour // ���O��Enemy1Script�ɂȂ��Ă��邩�m�F
{
    [Header("�U���ݒ�")]
    public GameObject attackBallPrefab; // ���˂��鋅�̃v���n�u�������ɐݒ肷�� (Unity�G�f�B�^����h���b�O���h���b�v)
    public float attackForce = 10f; // �����΂���
    public float attackInterval = 1f; // �U���̊Ԋu�i�^�[�����̏ꍇ�́A���̊Ԋu�ōU�����J�n�����j

    [Header("�^�[�Q�b�g�ݒ�")]
    public string playerTag = "Player"; // �����i�v���C���[�j�̃^�O��

    private bool canAttack = false; // �U���ł����Ԃ�

    void Start()
    {
        // GameManager����̃^�[���Ǘ��ƘA�g����ꍇ�́AGameManager����AttackTurn()���Ăяo��
    }

    void Update()
    {
        // �^�[���Ǘ���GameManager���s�����߁A�����ł͒��ڍU���������L�q���Ȃ�
    }

    /// <summary>
    /// �^�[���J�n����GameManager����Ăяo�����U������
    /// </summary>
    public void AttackTurn() // ������ ���̕�����Enemy1Script�ɕK�v����I ������
    {
        Debug.Log(gameObject.name + " �̍U���^�[���I");
        canAttack = true;
        StartCoroutine(PerformAttack()); // �U���R���[�`�����J�n
    }

    /// <summary>
    /// ���ۂ̍U�������i���̔��ˁj
    /// </summary>
    IEnumerator PerformAttack()
    {
        // �܂��U����������Ă��Ȃ��A�܂��̓N�[���_�E�����̏ꍇ�͉������Ȃ�
        if (!canAttack)
        {
            yield break; // �R���[�`�����I��
        }

        // �V�[�����̖�����T��
        GameObject[] players = GameObject.FindGameObjectsWithTag(playerTag);

        if (players.Length == 0)
        {
            Debug.LogWarning("�^�[�Q�b�g�ƂȂ閡����������܂���ł����B�^�O: " + playerTag);
            canAttack = false; // �U���ł��Ȃ���Ԃɂ���
            yield break;
        }

        // �����_���Ȗ������^�[�Q�b�g�ɂ���
        GameObject targetPlayer = players[Random.Range(0, players.Length)];

        // ���𐶐�����
        GameObject ball = Instantiate(attackBallPrefab, transform.position, Quaternion.identity);

        // ����RigidBody2D���擾����
        Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("�U�����v���n�u��Rigidbody2D���A�^�b�`����Ă��܂���I");
            yield break;
        }

        // �^�[�Q�b�g�ւ̕������v�Z����
        Vector2 direction = (targetPlayer.transform.position - transform.position).normalized;

        // �����΂��͂�������
        rb.AddForce(direction * attackForce, ForceMode2D.Impulse);

        Debug.Log(gameObject.name + " �� " + targetPlayer.name + " �Ɍ����ċ��𔭎˂��܂����B");

        // �U�����I���������Ƃ�GameManager�ɓ`����Ȃǂ̏������K�v�ɂȂ�
        // ��: GameManager.Instance.NextEnemyTurn();
        // �i�^�[���I��������GameManager��EnemyTurnRoutine�Ŏ����I�Ɏ��̓G�Ɉڂ�悤�ɂ��Ă���̂ŁA�����ł͏����Ȃ��Ă�OK�j
    }

    // �Փˎ��̏����i���ɃA�^�b�`����X�N���v�g�Ń_���[�W�������s���j
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ���̃X�N���v�g�͓G�{�̂Ȃ̂ŁA�G���Փ˂����ꍇ�̏����B
        // ���˂������̃X�N���v�g�ŁA�����ւ̃_���[�W�������s���ׂ��B
    }
}
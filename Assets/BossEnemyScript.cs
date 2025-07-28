using UnityEngine;
using System.Collections;

public class BossEnemyScript : MonoBehaviour
{
    [Header("�U���ݒ�")]
    public GameObject attackBallPrefab; // ���˂��鋅�̃v���n�u�������ɐݒ�
    public float attackForce = 20f; // ���ύX�_1: �U���͂�Enemy1Script��苭������
    public float attackInterval = 1.0f; // �U���̊Ԋu (�K�v�Ȃ�ύX)

    [Header("�^�[�Q�b�g�ݒ�")]
    public string playerTag = "Player"; // �����i�v���C���[�j�̃^�O��

    // ���ǉ��_1: �{�X�Ǝ��̗̑͐ݒ�
    [Header("�{�X�ݒ�")]
    public int maxBossHealth = 1000; // �{�X�̍ő�̗� (���ʂ̓G��肩�Ȃ荂��)
    private int currentBossHealth; // ���݂̗̑�

    private bool canAttack = false; // �U���ł����Ԃ�
    private bool isBossDefeated = false; // ���ǉ��_2: �{�X���|���ꂽ��

    void Start()
    {
        currentBossHealth = maxBossHealth; // �̗͂��ő�l�ŏ�����
        // GameManager����̃^�[���Ǘ��ƘA�g����ꍇ�́AGameManager����AttackTurn()���Ăяo��
    }

    void Update()
    {
        // �^�[���Ǘ���GameManager���s�����߁A�����ł͒��ڍU���������L�q���Ȃ�
    }

    /// <summary>
    /// �^�[���J�n����GameManager����Ăяo�����U������
    /// </summary>
    public void AttackTurn()
    {
        if (isBossDefeated)
        { // �{�X���|����Ă�����U�����Ȃ�
            Debug.Log(gameObject.name + " �͓|��Ă���̂ōU�����܂���B");
            return;
        }

        Debug.Log(gameObject.name + " �̍U���^�[���I");
        canAttack = true;
        StartCoroutine(PerformAttack()); // �U���R���[�`�����J�n
    }

    /// <summary>
    /// ���ۂ̍U�������i���̔��ˁj
    /// </summary>
    IEnumerator PerformAttack()
    {
        if (!canAttack)
        {
            yield break;
        }

        GameObject[] players = GameObject.FindGameObjectsWithTag(playerTag);

        if (players.Length == 0)
        {
            Debug.LogWarning("�^�[�Q�b�g�ƂȂ閡����������܂���ł����B�^�O: " + playerTag);
            canAttack = false;
            yield break;
        }

        GameObject targetPlayer = players[Random.Range(0, players.Length)];

        GameObject ball = Instantiate(attackBallPrefab, transform.position, Quaternion.identity);

        Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("�U�����v���n�u��Rigidbody2D���A�^�b�`����Ă��܂���I");
            yield break;
        }

        Vector2 direction = (targetPlayer.transform.position - transform.position).normalized;

        // ���ύX�_2: AddForce��ForceMode��Impulse����VelocityChange�ɕύX���邱�Ƃ����� (�C��)
        // �����{�X�������̒e��A���Ŕ�΂��Ȃ�Impulse�B�ꔭ�̋����e�Ȃ�VelocityChange���A���B
        rb.AddForce(direction * attackForce, ForceMode2D.Impulse);

        Debug.Log(gameObject.name + " �� " + targetPlayer.name + " �Ɍ����ċ��𔭎˂��܂����B");
    }

    /// <summary>
    /// ���ǉ��_3: �{�X���_���[�W���󂯂鏈��
    /// </summary>
    public void TakeDamage(int damageAmount)
    {
        if (isBossDefeated) return; // ���ɓ|��Ă����牽�����Ȃ�

        currentBossHealth -= damageAmount;
        Debug.Log(gameObject.name + " �� " + damageAmount + " �_���[�W���󂯂܂����B�c��HP: " + currentBossHealth);

        if (currentBossHealth <= 0)
        {
            currentBossHealth = 0; // HP���}�C�i�X�ɂȂ�Ȃ��悤��
            isBossDefeated = true; // �|���ꂽ��Ԃɂ���
            Debug.Log(gameObject.name + " ��|���܂����I");
            // �����Ń{�X���\���ɂ�����A�����G�t�F�N�g���o������A
            // GameManager�ɃQ�[���N���A��ʒm����Ȃǂ̏���
            // ��: GameManager.Instance.GameClear();
            gameObject.SetActive(false); // �{�X���\���ɂ����
        }
        // ������HP�o�[�̍X�V�Ȃǂ��s��
    }

    // �Փˎ��̏����i���˂������ɃA�^�b�`����X�N���v�g�Ń_���[�W�������s���z��j
    // ����OnCollisionEnter2D�̓{�X���g�����̃I�u�W�F�N�g�ƂԂ��������̏����Ȃ̂ŁA
    // �v���C���[�̍U�����{�X�ɓ����������̃_���[�W�����́A�ʂ̕��@�i�v���C���[�̍U���X�N���v�g����TakeDamage���ĂԂȂǁj�ɂȂ�B
}
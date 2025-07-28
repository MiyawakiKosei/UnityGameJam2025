using UnityEngine;
using System.Collections; // �R���[�`�����g�����߂ɕK�v
using System.Collections.Generic; // ���X�g���g�����߂ɕK�v

public class GameManager : MonoBehaviour
{
    // GameManager���ǂ�����ł��A�N�Z�X�ł���悤�ɂ���i�V���O���g���p�^�[���j
    public static GameManager Instance { get; private set; }

    [Header("�^�[���ݒ�")]
    public float turnDelay = 1.0f; // �e�^�[���Ԃ̑ҋ@����

    // EnemyScript�̕����� Enemy1Script �ɕύX
    private List<Enemy1Script> enemies = new List<Enemy1Script>(); // �V�[�����̓G���Ǘ����郊�X�g
    private int currentEnemyIndex = 0; // ���ݍs������G�̃C���f�b�N�X

    // �Q�[���̏�ԊǗ��i��j
    public enum GameState
    {
        PlayerTurn,
        EnemyTurn,
        GameOver,
        GameClear
    }
    public GameState currentState; // ���݂̃Q�[�����

    void Awake()
    {
        // �V���O���g���C���X�^���X�̏�����
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �V�[����؂�ւ��Ă�����GameManager���j������Ȃ��悤�ɂ���i�K�v�ł���΁j
        }
        else
        {
            Destroy(gameObject); // ���ɃC���X�^���X�����݂���ꍇ�͎�����j��
        }
    }

    void Start()
    {
        // �Q�[���J�n���ɓG��S�Č�����
        FindAllEnemies();

        // �Q�[�����X�^�[�g������
        StartGame();
    }

    /// <summary>
    /// �V�[�����̑S�Ă̓G�������ă��X�g�ɒǉ�����
    /// </summary>
    void FindAllEnemies()
    {
        enemies.Clear(); // ��x�N���A
        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("Enemy"); // �G��GameObject���^�O�ŒT��

        foreach (GameObject enemyObj in enemyObjects)
        {
            // EnemyScript�̕����� Enemy1Script �ɕύX
            Enemy1Script es = enemyObj.GetComponent<Enemy1Script>();
            if (es != null)
            {
                enemies.Add(es);
            }
        }
        Debug.Log("GameManager: " + enemies.Count + "�̂̓G�������܂����B");
    }

    /// <summary>
    /// �Q�[�����J�n����
    /// </summary>
    public void StartGame()
    {
        currentState = GameState.PlayerTurn; // �܂��̓v���C���[�̃^�[������
        Debug.Log("�Q�[���J�n�I�v���C���[�̃^�[���ł��B");
        // �����Ńv���C���[�̍s��UI��\��������A�s����҂����肷�鏈��
        // ��: PlayerController.Instance.EnablePlayerInput();
    }

    /// <summary>
    /// �v���C���[�̃^�[�����I�����A�G�̃^�[�����J�n����
    /// (�v���C���[�̍s���X�N���v�g����Ăяo��)
    /// </summary>
    public void EndPlayerTurn()
    {
        if (currentState != GameState.PlayerTurn) return; // �v���C���[�^�[�����łȂ���Ή������Ȃ�

        currentState = GameState.EnemyTurn;
        Debug.Log("�v���C���[�̃^�[���I���I�G�̃^�[�����J�n���܂��B");
        StartCoroutine(EnemyTurnRoutine()); // �G�̃^�[���������J�n
    }

    /// <summary>
    /// �G�̃^�[�������R���[�`��
    /// </summary>
    IEnumerator EnemyTurnRoutine()
    {
        yield return new WaitForSeconds(turnDelay); // �^�[���J�n�O�̏����̑҂�����

        currentEnemyIndex = 0; // �ŏ��̓G����J�n

        while (currentEnemyIndex < enemies.Count)
        {
            // ���݂̓G�����݂��邩�`�F�b�N
            if (enemies[currentEnemyIndex] != null)
            {
                // �G�ɍU���^�[�����w��
                // EnemyScript�̕����� Enemy1Script �ɕύX
                enemies[currentEnemyIndex].AttackTurn();

                yield return new WaitForSeconds(turnDelay); // �e�G�̍s����̑҂�����
            }

            currentEnemyIndex++;
        }

        Debug.Log("�S�Ă̓G�̍s�����I�����܂����B");
        // �S�Ă̓G�̍s�����I�������A�Ăуv���C���[�̃^�[����
        currentState = GameState.PlayerTurn;
        Debug.Log("�v���C���[�̃^�[���ł��B");
        // �����Ńv���C���[�̍s��UI���Ăѕ\�������肷��
        // ��: PlayerController.Instance.EnablePlayerInput();
    }


    // ------------------------------------------------------------------------------------
    // ���̑��A�Q�[���̏�ԑJ�ڂ⏟�s����̃��\�b�h�Ȃǂ������ɒǉ����Ă���
    // ------------------------------------------------------------------------------------

    /// <summary>
    /// �Q�[���I�[�o�[����
    /// </summary>
    public void GameOver()
    {
        currentState = GameState.GameOver;
        Debug.Log("�Q�[���I�[�o�[�I");
        // �Q�[���I�[�o�[��ʂ�\��������A���g���C�{�^����\��������
    }

    /// <summary>
    /// �Q�[���N���A����
    /// </summary>
    public void GameClear()
    {
        currentState = GameState.GameClear;
        Debug.Log("�Q�[���N���A�I");
        // �Q�[���N���A��ʂ�\��������
    }
}
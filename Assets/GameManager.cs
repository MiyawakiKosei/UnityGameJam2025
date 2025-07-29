using UnityEngine;
using System.Collections; // コルーチンを使うために必要
using System.Collections.Generic; // リストを使うために必要

public class GameManager : MonoBehaviour
{
    // GameManagerをどこからでもアクセスできるようにする（シングルトンパターン）
    public static GameManager Instance { get; private set; }

    [Header("ターン設定")]
    public float turnDelay = 1.0f; // 各ターン間の待機時間

    // EnemyScriptの部分を Enemy1Script に変更
    private List<Enemy1Script> enemies = new List<Enemy1Script>(); // シーン内の敵を管理するリスト
    private int currentEnemyIndex = 0; // 現在行動する敵のインデックス

    // ゲームの状態管理（例）
    public enum GameState
    {
        PlayerTurn,
        EnemyTurn,
        GameOver,
        GameClear
    }
    public GameState currentState; // 現在のゲーム状態

    void Awake()
    {
        // シングルトンインスタンスの初期化
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // シーンを切り替えてもこのGameManagerが破棄されないようにする（必要であれば）
        }
        else
        {
            Destroy(gameObject); // 既にインスタンスが存在する場合は自分を破棄
        }
    }

    void Start()
    {
        // ゲーム開始時に敵を全て見つける
        FindAllEnemies();

        // ゲームをスタートさせる
        StartGame();
    }

    /// <summary>
    /// シーン内の全ての敵を見つけてリストに追加する
    /// </summary>
    void FindAllEnemies()
    {
        enemies.Clear(); // 一度クリア
        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("Enemy"); // 敵のGameObjectをタグで探す

        foreach (GameObject enemyObj in enemyObjects)
        {
            // EnemyScriptの部分を Enemy1Script に変更
            Enemy1Script es = enemyObj.GetComponent<Enemy1Script>();
            if (es != null)
            {
                enemies.Add(es);
            }
        }
        Debug.Log("GameManager: " + enemies.Count + "体の敵を見つけました。");
    }

    /// <summary>
    /// ゲームを開始する
    /// </summary>
    public void StartGame()
    {
        currentState = GameState.PlayerTurn; // まずはプレイヤーのターンから
        Debug.Log("ゲーム開始！プレイヤーのターンです。");
        // ここでプレイヤーの行動UIを表示したり、行動を待ったりする処理
        // 例: PlayerController.Instance.EnablePlayerInput();
    }

    /// <summary>
    /// プレイヤーのターンを終了し、敵のターンを開始する
    /// (プレイヤーの行動スクリプトから呼び出す)
    /// </summary>
    public void EndPlayerTurn()
    {
        if (currentState != GameState.PlayerTurn) return; // プレイヤーターン中でなければ何もしない

        currentState = GameState.EnemyTurn;
        Debug.Log("プレイヤーのターン終了！敵のターンを開始します。");
        StartCoroutine(EnemyTurnRoutine()); // 敵のターン処理を開始
    }

    /// <summary>
    /// 敵のターン処理コルーチン
    /// </summary>
    IEnumerator EnemyTurnRoutine()
    {
        yield return new WaitForSeconds(turnDelay); // ターン開始前の少しの待ち時間

        currentEnemyIndex = 0; // 最初の敵から開始

        while (currentEnemyIndex < enemies.Count)
        {
            // 現在の敵が存在するかチェック
            if (enemies[currentEnemyIndex] != null)
            {
                // 敵に攻撃ターンを指示
                // EnemyScriptの部分を Enemy1Script に変更
                enemies[currentEnemyIndex].AttackTurn();

                yield return new WaitForSeconds(turnDelay); // 各敵の行動後の待ち時間
            }

            currentEnemyIndex++;
        }

        Debug.Log("全ての敵の行動が終了しました。");
        // 全ての敵の行動が終わったら、再びプレイヤーのターンへ
        currentState = GameState.PlayerTurn;
        Debug.Log("プレイヤーのターンです。");
        // ここでプレイヤーの行動UIを再び表示したりする
        // 例: PlayerController.Instance.EnablePlayerInput();
    }


    // ------------------------------------------------------------------------------------
    // その他、ゲームの状態遷移や勝敗判定のメソッドなどをここに追加していく
    // ------------------------------------------------------------------------------------

    /// <summary>
    /// ゲームオーバー処理
    /// </summary>
    public void GameOver()
    {
        currentState = GameState.GameOver;
        Debug.Log("ゲームオーバー！");
        // ゲームオーバー画面を表示したり、リトライボタンを表示したり
    }

    /// <summary>
    /// ゲームクリア処理
    /// </summary>
    public void GameClear()
    {
        currentState = GameState.GameClear;
        Debug.Log("ゲームクリア！");
        // ゲームクリア画面を表示したり
    }
}
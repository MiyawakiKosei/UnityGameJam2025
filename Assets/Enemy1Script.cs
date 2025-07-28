// Enemy1Script.cs の中身

using UnityEngine;
using System.Collections; // コルーチンを使うために必要

public class Enemy1Script : MonoBehaviour // 名前がEnemy1Scriptになっているか確認
{
    [Header("攻撃設定")]
    public GameObject attackBallPrefab; // 発射する球のプレハブをここに設定する (Unityエディタからドラッグ＆ドロップ)
    public float attackForce = 10f; // 球を飛ばす力
    public float attackInterval = 1f; // 攻撃の間隔（ターン制の場合は、この間隔で攻撃が開始される）

    [Header("ターゲット設定")]
    public string playerTag = "Player"; // 味方（プレイヤー）のタグ名

    private bool canAttack = false; // 攻撃できる状態か

    void Start()
    {
        // GameManagerからのターン管理と連携する場合は、GameManagerからAttackTurn()を呼び出す
    }

    void Update()
    {
        // ターン管理はGameManagerが行うため、ここでは直接攻撃処理を記述しない
    }

    /// <summary>
    /// ターン開始時にGameManagerから呼び出される攻撃処理
    /// </summary>
    public void AttackTurn() // ★★★ この部分がEnemy1Scriptに必要だよ！ ★★★
    {
        Debug.Log(gameObject.name + " の攻撃ターン！");
        canAttack = true;
        StartCoroutine(PerformAttack()); // 攻撃コルーチンを開始
    }

    /// <summary>
    /// 実際の攻撃処理（球の発射）
    /// </summary>
    IEnumerator PerformAttack()
    {
        // まだ攻撃が許可されていない、またはクールダウン中の場合は何もしない
        if (!canAttack)
        {
            yield break; // コルーチンを終了
        }

        // シーン内の味方を探す
        GameObject[] players = GameObject.FindGameObjectsWithTag(playerTag);

        if (players.Length == 0)
        {
            Debug.LogWarning("ターゲットとなる味方が見つかりませんでした。タグ: " + playerTag);
            canAttack = false; // 攻撃できない状態にする
            yield break;
        }

        // ランダムな味方をターゲットにする
        GameObject targetPlayer = players[Random.Range(0, players.Length)];

        // 球を生成する
        GameObject ball = Instantiate(attackBallPrefab, transform.position, Quaternion.identity);

        // 球のRigidBody2Dを取得する
        Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("攻撃球プレハブにRigidbody2Dがアタッチされていません！");
            yield break;
        }

        // ターゲットへの方向を計算する
        Vector2 direction = (targetPlayer.transform.position - transform.position).normalized;

        // 球を飛ばす力を加える
        rb.AddForce(direction * attackForce, ForceMode2D.Impulse);

        Debug.Log(gameObject.name + " が " + targetPlayer.name + " に向けて球を発射しました。");

        // 攻撃が終了したことをGameManagerに伝えるなどの処理が必要になる
        // 例: GameManager.Instance.NextEnemyTurn();
        // （ターン終了処理はGameManagerのEnemyTurnRoutineで自動的に次の敵に移るようにしているので、ここでは書かなくてもOK）
    }

    // 衝突時の処理（球にアタッチするスクリプトでダメージ処理を行う）
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // このスクリプトは敵本体なので、敵が衝突した場合の処理。
        // 発射した球のスクリプトで、味方へのダメージ処理を行うべき。
    }
}
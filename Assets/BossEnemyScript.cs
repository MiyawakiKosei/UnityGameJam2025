using UnityEngine;
using System.Collections;

public class BossEnemyScript : MonoBehaviour
{
    [Header("攻撃設定")]
    public GameObject attackBallPrefab; // 発射する球のプレハブをここに設定
    public float attackForce = 20f; // ★変更点1: 攻撃力をEnemy1Scriptより強くする
    public float attackInterval = 1.0f; // 攻撃の間隔 (必要なら変更)

    [Header("ターゲット設定")]
    public string playerTag = "Player"; // 味方（プレイヤー）のタグ名

    // ★追加点1: ボス独自の体力設定
    [Header("ボス設定")]
    public int maxBossHealth = 1000; // ボスの最大体力 (普通の敵よりかなり高く)
    private int currentBossHealth; // 現在の体力

    private bool canAttack = false; // 攻撃できる状態か
    private bool isBossDefeated = false; // ★追加点2: ボスが倒されたか

    void Start()
    {
        currentBossHealth = maxBossHealth; // 体力を最大値で初期化
        // GameManagerからのターン管理と連携する場合は、GameManagerからAttackTurn()を呼び出す
    }

    void Update()
    {
        // ターン管理はGameManagerが行うため、ここでは直接攻撃処理を記述しない
    }

    /// <summary>
    /// ターン開始時にGameManagerから呼び出される攻撃処理
    /// </summary>
    public void AttackTurn()
    {
        if (isBossDefeated)
        { // ボスが倒されていたら攻撃しない
            Debug.Log(gameObject.name + " は倒れているので攻撃しません。");
            return;
        }

        Debug.Log(gameObject.name + " の攻撃ターン！");
        canAttack = true;
        StartCoroutine(PerformAttack()); // 攻撃コルーチンを開始
    }

    /// <summary>
    /// 実際の攻撃処理（球の発射）
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
            Debug.LogWarning("ターゲットとなる味方が見つかりませんでした。タグ: " + playerTag);
            canAttack = false;
            yield break;
        }

        GameObject targetPlayer = players[Random.Range(0, players.Length)];

        GameObject ball = Instantiate(attackBallPrefab, transform.position, Quaternion.identity);

        Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("攻撃球プレハブにRigidbody2Dがアタッチされていません！");
            yield break;
        }

        Vector2 direction = (targetPlayer.transform.position - transform.position).normalized;

        // ★変更点2: AddForceのForceModeをImpulseからVelocityChangeに変更することも検討 (任意)
        // もしボスが複数の弾を連続で飛ばすならImpulse。一発の強い弾ならVelocityChangeもアリ。
        rb.AddForce(direction * attackForce, ForceMode2D.Impulse);

        Debug.Log(gameObject.name + " が " + targetPlayer.name + " に向けて球を発射しました。");
    }

    /// <summary>
    /// ★追加点3: ボスがダメージを受ける処理
    /// </summary>
    public void TakeDamage(int damageAmount)
    {
        if (isBossDefeated) return; // 既に倒れていたら何もしない

        currentBossHealth -= damageAmount;
        Debug.Log(gameObject.name + " が " + damageAmount + " ダメージを受けました。残りHP: " + currentBossHealth);

        if (currentBossHealth <= 0)
        {
            currentBossHealth = 0; // HPがマイナスにならないように
            isBossDefeated = true; // 倒された状態にする
            Debug.Log(gameObject.name + " を倒しました！");
            // ここでボスを非表示にしたり、爆発エフェクトを出したり、
            // GameManagerにゲームクリアを通知するなどの処理
            // 例: GameManager.Instance.GameClear();
            gameObject.SetActive(false); // ボスを非表示にする例
        }
        // ここでHPバーの更新などを行う
    }

    // 衝突時の処理（発射した球にアタッチするスクリプトでダメージ処理を行う想定）
    // このOnCollisionEnter2Dはボス自身が他のオブジェクトとぶつかった時の処理なので、
    // プレイヤーの攻撃がボスに当たった時のダメージ処理は、別の方法（プレイヤーの攻撃スクリプトからTakeDamageを呼ぶなど）になる。
}
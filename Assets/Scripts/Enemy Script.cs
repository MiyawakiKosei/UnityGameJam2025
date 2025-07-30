using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] private int maxHits = 3;
    private int currentHits = 0;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //プレイヤーとぶつかったら
        if(collision.gameObject.CompareTag("Player"))
        {
            Damage();
        }
    }

    private void Damage()
    {
        currentHits++;
        Debug.Log("ヒット");

        if (currentHits >= maxHits)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

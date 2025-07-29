using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    private Rigidbody2D _rb;
    private Vector2 startPos;
    private Vector2 endPos;

    [SerializeField] private float power = 20f;
    [SerializeField] private float maxPower = 30f;

    private bool isDragging = false;
    private bool canPull = true;

    private int bounceCount = 0;
    [SerializeField] private int maxBounceCount = 5;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnMouseDown()
    {
        if (!canPull) return;

        startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        isDragging = true;

        _rb.velocity = Vector2.zero;
        _rb.angularVelocity = 0f;
        _rb.bodyType = RigidbodyType2D.Kinematic;
    }

    private void OnMouseDrag()
    {
        if (!isDragging) return;

        endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseUp()
    {
        if (!isDragging) return;

        isDragging = false;
        canPull = false;

        bounceCount = 0; //バウンド回数リセット

        endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 force = startPos - endPos;
        force = Vector2.ClampMagnitude(force, maxPower);

        _rb.bodyType = RigidbodyType2D.Dynamic;
        _rb.AddForce(force * power, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!canPull)
        {
            bounceCount++;

            if (bounceCount >= maxBounceCount)
            {
                canPull = true;
                _rb.velocity = Vector2.zero; // ピタッと止める
            }
        }
    }
}

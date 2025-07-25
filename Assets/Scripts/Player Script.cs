using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D _rb;

    private float powar = 2.0f;
    private float maxpowar = 5.0f;

    private Vector2 StartPos;
    private Vector2 EndPos;

    // Start is called before the first frame update
    void Start()
    {
        _rb=GetComponent<Rigidbody2D>();
    }

    private void OnMouseDown()
    {
        StartPos=(Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseDrag()
    {
        transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 force = Vector2.ClampMagnitude((StartPos - EndPos), maxpowar);
        _rb.AddForce(force * powar, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {

    }
}

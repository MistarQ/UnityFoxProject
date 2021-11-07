using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Eagle : Enemy
{
    private Rigidbody2D rb;
    // private Collider2D Coll;
    public float Speed;
    private float TopY, BottomY;
    public Transform topPoint, bottomPoint;
    private bool isUp;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        // Coll = GetComponent<Collider2D>();
        TopY = topPoint.position.y;
        BottomY = bottomPoint.position.y;
        Destroy(topPoint.gameObject);
        Destroy(bottomPoint.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        Movement();   
    }

    void Movement() {
        if (isUp)
        {
            rb.velocity = new Vector2(rb.velocity.x, Speed);

            if (transform.position.y > TopY)
            {
                isUp = false;
            }
        }
        else {
            rb.velocity = new Vector2(rb.velocity.x, -Speed);

            if (transform.position.y < BottomY)
            {
                isUp = true;
            }
        }
    }
}

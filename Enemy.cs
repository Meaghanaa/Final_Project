using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform startMarker;
    public Transform endMarker;
    public float speed = 1.0F;
    private float startTime;
    private float journeyLength;
    SpriteRenderer m_SpriteRenderer;
    public float hozMovement;

    private bool facingRight = true;

    private float lastX;




    void Start()
    {
        startTime = Time.time;
        m_SpriteRenderer = GetComponent<SpriteRenderer>();

        journeyLength = Vector2.Distance(startMarker.position, endMarker.position);
    }
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
    }

    void Update()
    {
        float distCovered = (Time.time - startTime) * speed;
        Vector2 localScale = transform.localScale;

        float fracJourney = distCovered / journeyLength;

        transform.position = Vector2.Lerp(startMarker.position, endMarker.position, Mathf.PingPong(fracJourney, 1));

       

        if (lastX > transform.position.x && facingRight == true)
        {
            Flip();
        }
        else if (lastX < transform.position.x && facingRight == false)
        {
            Flip();
        }

        lastX = transform.position.x;

    }
    void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }


}

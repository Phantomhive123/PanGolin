using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private Rigidbody2D rigid;
    public float speed = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //rigid.velocity = Vector2.right * speed;
        if(rigid.velocity.y <= 0.01f)
            transform.Translate(Vector2.right * speed * Time.deltaTime, Space.World);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public struct BoxColliderRaycastOrigins
{
    public Vector2 topLeft;
    public Vector2 bottomRight;
    public Vector2 bottomLeft;
    public Vector2 topRight;
}

[RequireComponent(typeof(BoxCollider2D),typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private BoxColliderRaycastOrigins raycastOrigins;
    private float skinWidth = 0.001f;
    private float rayDistance = 0.05f;
    private bool isGrounded = false;
    private BoxCollider2D boxCollider;
    private Rigidbody2D rigid;

    private Vector3 dir;
    private bool pause = false;

    [SerializeField]
    private float speed = 5f;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rigid = GetComponent<Rigidbody2D>();
        dir = Vector2.right;
    }

    private void Update()
    {
        CheckIfGrounded();
        if (!isGrounded)
        {
            rigid.velocity = speed * Vector2.down;
        }
        else if(!pause)
        {
            rigid.velocity = speed * dir;
        }
    }

    private void LateUpdate()
    {
        
    }

    public void ChangeDirection()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z); 
        dir *= -1;
    }

    public void StopMove()
    {
        pause = true;
        rigid.velocity = Vector2.zero;
        StartCoroutine(DelayContinue());
    }

    IEnumerator DelayContinue()
    {
        yield return new WaitForSeconds(2f);
        pause = false;
    }

    private void CheckIfGrounded()
    {
        var modifiedBounds = boxCollider.bounds;
        modifiedBounds.Expand(-1 * skinWidth);
        raycastOrigins.topLeft = new Vector2(modifiedBounds.min.x, modifiedBounds.max.y);
        raycastOrigins.topRight = new Vector2(modifiedBounds.max.x, modifiedBounds.max.y);
        raycastOrigins.bottomLeft = new Vector2(modifiedBounds.min.x, modifiedBounds.min.y);
        raycastOrigins.bottomRight = new Vector2(modifiedBounds.max.x, modifiedBounds.min.y);

        isGrounded = RaycastCheck(raycastOrigins.bottomLeft, Vector2.down) 
            || RaycastCheck(raycastOrigins.bottomRight, Vector2.down);
    }

    private bool RaycastCheck(Vector2 oringin, Vector2 dir)
    {
        RaycastHit2D[] raycastHits;
        Debug.DrawRay(oringin, dir * rayDistance, Color.red);
        raycastHits = Physics2D.RaycastAll(oringin, dir, rayDistance);
        foreach (RaycastHit2D raycastHit in raycastHits)
        {
            if (raycastHit.collider.gameObject != gameObject)
            {
                return true;
            }
        }
        return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        BasicElement bs = collision.gameObject.GetComponent<BasicElement>();
        if (!bs) return;
        bs.eventDelegate += StopMove;
        if (bs is Fire)
            ChangeDirection();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        BasicElement bs = collision.gameObject.GetComponent<BasicElement>();
        if (bs.eventDelegate != null) 
            bs.eventDelegate -= StopMove;
    }
}

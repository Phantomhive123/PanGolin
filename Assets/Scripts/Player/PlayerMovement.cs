using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : BoxObj
{
    [SerializeField]
    private float moveSpeed = 5f;
    [SerializeField]
    private float pushSpeed = 3f;
    [SerializeField]
    private Vector2 dir = Vector2.right;

    private float currentSpeed;
    private BasicElement bePushedObj = null;
    private bool pause = false;

    private void Start()
    {
        currentSpeed = moveSpeed;
        ComboManager.Instance.ContinueDelegate += ContinueMove;
    }

    protected override void Update()
    {
        base.Update();
        if(!pause)  targetVelocity.x += dir.x * currentSpeed;
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
        if (collisionState.wasGroundLastFrame && !isGrounded)
        {
            SetTrigger(false);
        }
        else if (collisionState.becameGroundedThisFrame)
        {
            SetTrigger(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isGrounded) return;
        if (pause) return;
        if (collision.GetComponent<Ignite>())
        {
            ChangeDirection();
            return;
        }
        BasicElement bs = collision.GetComponent<BasicElement>();
        if (!bs||bs is Magnet)
        {
            bePushedObj = null;
            return;
        } 
        else
        {
            bePushedObj = bs;
            bePushedObj.isInteracted = true;
            currentSpeed = pushSpeed;
            //挂载委托
            bs.StopDelegate += StopMove;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<BasicElement>() == bePushedObj)
        {
            //要不要删除委托？
            bePushedObj = null;
            currentSpeed = moveSpeed;
        } 
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!isGrounded) return;
        if (bePushedObj)
            bePushedObj.Move(targetVelocity * Time.deltaTime);
    }

    private void ChangeDirection()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        dir *= -1;
    }

    private void StopMove()
    {
        pause = true;
        //播放动画
    }

    private void ContinueMove()
    {
        pause = false;
    }

    private void SetTrigger(bool enable)
    {
        BoxCollider2D[] colliders = GetComponents<BoxCollider2D>();
        foreach(BoxCollider2D collider in colliders)
        {
            if (collider.isTrigger)
                collider.enabled = enable;
        }
    }
}

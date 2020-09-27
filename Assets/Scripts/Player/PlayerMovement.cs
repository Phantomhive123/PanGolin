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
    }

    protected override void Update()
    {
        base.Update();
        if(!pause)  targetVelocity.x += dir.x * currentSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isGrounded) return;
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
            if (bs.ComboNum < 0)
                bs.ComboNum = 0;
            currentSpeed = pushSpeed;
            //挂载委托
            bs.StopDelegate += StopMove;
            bs.ContinueDelegate += ContinueMove;
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
}

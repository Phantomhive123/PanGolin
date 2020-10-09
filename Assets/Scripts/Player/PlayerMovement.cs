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
    static public bool pause = false;

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
            GetComponent<Animator>().Play(Animator.StringToHash("Stand"));
        }
        else if (collisionState.becameGroundedThisFrame)
        {
            SetTrigger(true);
            GetComponent<Animator>().Play(Animator.StringToHash("Walk"));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isGrounded) return;
        if (pause) return;
        if (collision.GetComponent<Ignite>())
        {
            ChangeDirection();
            //GetComponent<Animator>().Play(Animator.StringToHash("ChangeDir"));
            return;
        }
        BasicElement bs = collision.GetComponent<BasicElement>();
        if (!bs || bs is Magnet || bs is Fire) 
        {
            bePushedObj = null;
            return;
        } 
        else
        {
            GetComponent<Animator>().Play(Animator.StringToHash("Push"));
            bePushedObj = bs;
            bs.StopDelegate += StopMove;
            bePushedObj.isInteracted = true;
            currentSpeed = pushSpeed;           
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<BasicElement>() == bePushedObj)
        {
            //要不要删除委托？
            bePushedObj = null;
            currentSpeed = moveSpeed;
            //GetComponent<Animator>().Play(Animator.StringToHash("Walk"));
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!isGrounded) return;
        if (bePushedObj)
            bePushedObj.Move(targetVelocity * Time.deltaTime);
    }

    public void ChangeDirection()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        dir *= -1;
    }

    private void StopMove()
    {
        pause = true;
        GetComponent<Animator>().Play(Animator.StringToHash("Stand"));
        SetTrigger(false);
    }

    private void ContinueMove()
    {
        pause = false;
        GetComponent<Animator>().Play(Animator.StringToHash("Walk"));
        SetTrigger(true);
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

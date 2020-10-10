using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : BoxObj
{
    private static PlayerMovement _instance;
    public static PlayerMovement Instance
    {
        get { return _instance; }
    }

    [SerializeField]
    private float moveSpeed = 5f;
    [SerializeField]
    private float pushSpeed = 3f;
    [SerializeField]
    private Vector2 dir = Vector2.right;

    private float currentSpeed;
    private BasicElement bePushedObj = null;
    public bool pause = false;
    private Animator animator;

    private void Start()
    {
        if (_instance == null) 
            _instance = this;
        else
            Destroy(gameObject);

        currentSpeed = moveSpeed;
        ComboManager.Instance.ContinueDelegate += ContinueMove;
        animator = GetComponent<Animator>();
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
            animator.Play(Animator.StringToHash("Stand"));
        }
        else if (collisionState.becameGroundedThisFrame)
        {
            SetTrigger(true);
            animator.Play(Animator.StringToHash("Walk"));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isGrounded) return;
        if (pause) return;
        if (collision.GetComponent<Ignite>()||collision.GetComponent<Fire>())
        {
            ChangeDirection();
            animator.Play(Animator.StringToHash("ChangeDir"));
            
            //ChangeDirection();
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
            animator.Play(Animator.StringToHash("Push"));
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
        animator.Play(Animator.StringToHash("Stand"));
        SetTrigger(false);
    }

    private void ContinueMove()
    {
        pause = false;
        animator.Play(Animator.StringToHash("Walk"));
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

    public void GameOver(bool win)
    {
        pause = true;
        ComboManager.Instance.ContinueDelegate -= ContinueMove;
        if (win)
            animator.Play(Animator.StringToHash("Win"));
        else
            animator.Play(Animator.StringToHash("Die"));
    }
}

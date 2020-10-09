using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionState2D
{
    public bool right;
    public bool left;
    public bool above;
    public bool below;
    public bool becameGroundedThisFrame;
    public bool wasGroundLastFrame = false;

    public bool HasCollision()
    {
        return below || right || left || above;
    }

    public void Reset()
    {
        right = left = above = below = false;
    }
}

public abstract class MobileObj : MonoBehaviour
{
    public float gravityModifier = 5f;
    [SerializeField]
    protected float moveThreshold = 0.005f;

    public Vector2 targetVelocity;
    public Vector2 velocity;

    protected new Transform transform;
    protected CollisionState2D collisionState = new CollisionState2D();

    protected bool hasGravity = true;

    protected virtual void Awake()
    {
        transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        CalculateGravity();
        //如果主角加力的话应该是在这里加的
    }

    protected virtual void LateUpdate()
    {
        Move(targetVelocity * Time.deltaTime);
    }

    public virtual void Move(Vector2 deltaMovement)
    {
        deltaMovement = MaxMoveableDis(deltaMovement);
        transform.Translate(deltaMovement, Space.World);
        velocity = deltaMovement / Time.deltaTime;
    }

    protected virtual void MoveHorizontally(ref Vector2 deltaMovement)
    {

    }

    protected virtual void MoveVertically(ref Vector2 deltaMovement)
    {

    }

    public virtual Vector2 MaxMoveableDis(Vector2 deltaMovement)
    {
        //这个逻辑里面，先朝下移动
        MoveVertically(ref deltaMovement);
        //如果竖直方向有位移，这一帧不水平检测，但是对后续的磁石可能会有问题
        if (Mathf.Abs(deltaMovement.y) > moveThreshold) return deltaMovement;
        MoveHorizontally(ref deltaMovement);
        return deltaMovement;
    }

    protected void CalculateGravity()
    {
        if (!hasGravity) return;
        targetVelocity = gravityModifier * Vector2.down;
    }

    public virtual void Hit(MobileObj another)
    {

    }

    public virtual void BeHit(MobileObj another)
    {

    }
}

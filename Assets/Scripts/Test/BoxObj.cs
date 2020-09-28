using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D),typeof(Rigidbody2D))]
public class BoxObj : MobileObj
{
    private struct BoxRaycastOrigins
    {
        public Vector3 topLeft;
        public Vector3 topRight;
        public Vector3 bottomLeft;
        public Vector3 bottomRight;
    }

    [SerializeField]
    protected int totalHorizontalRays = 3;

    [SerializeField]
    protected int totalVerticalRays = 3;

    [SerializeField]
    protected float skinWidth = 0.01f;

    const float kSkinWidthFloatFudgeFactor = 0.001f;
    private float _verticalDistanceBetweenRays;
    private float _horizontalDistanceBetweenRays;
    private BoxCollider2D boxCollider;
    private BoxRaycastOrigins _raycastOrigins;
    private RaycastHit2D _raycastHit;

    public bool isGrounded { get { return collisionState.below; } }

    protected override void Awake()
    {
        base.Awake();
        boxCollider = GetComponent<BoxCollider2D>();
        CalculateDistanceBetweenRays();
    }

    protected override void MoveHorizontally(ref Vector2 deltaMovement)
    {
        if (deltaMovement.x == 0) return;//如果没有这一行，物体没有水平位移的时候会朝左发射射线。如果撞到紧邻的物体，这个物体会被弹开？
        bool isGoingRight = deltaMovement.x > 0;
        float rayDistance = Mathf.Abs(deltaMovement.x) + skinWidth;
        Vector2 rayDirection = isGoingRight ? Vector2.right : Vector2.left;
        Vector2 initialRayOrigin = isGoingRight ? _raycastOrigins.bottomRight : _raycastOrigins.bottomLeft;

        if (hasGravity)
            initialRayOrigin.y += deltaMovement.y;

        for (int i = 0; i < totalHorizontalRays; i++)
        {
            //射线起点
            Vector2 ray = new Vector2(initialRayOrigin.x, initialRayOrigin.y + i * _verticalDistanceBetweenRays);
            Debug.DrawRay(ray, rayDirection * rayDistance, Color.red);

            _raycastHit = IsCurrentObj(ray, rayDirection, rayDistance);
            if (_raycastHit)
            {
                deltaMovement.x = _raycastHit.point.x - ray.x;

                if (isGoingRight)
                {
                    deltaMovement.x -= skinWidth;
                    collisionState.right = true;
                }
                else
                {
                    deltaMovement.x += skinWidth;
                    collisionState.left = true;
                }

                MobileObj another = _raycastHit.collider.gameObject.GetComponent<MobileObj>();
                if (another)
                {
                    Hit(another);
                    another.BeHit(this);
                }

                //如果这条射线已经检测到皮肤，之后的射线不用检测了
                if (Mathf.Abs(deltaMovement.x) < skinWidth + kSkinWidthFloatFudgeFactor)
                    break;
            }
        }
    }

    protected override void MoveVertically(ref Vector2 deltaMovement)
    {
        bool isGoingUp = deltaMovement.y > 0;
        float rayDistance = Mathf.Abs(deltaMovement.y) + skinWidth;
        Vector2 rayDirection = isGoingUp ? Vector2.up : Vector2.down;
        Vector2 initialRayOrigin = isGoingUp ? _raycastOrigins.topLeft : _raycastOrigins.bottomLeft;

        for (int i = 0; i < totalVerticalRays; i++)
        {
            //射线起点
            var ray = new Vector2(initialRayOrigin.x + i * _horizontalDistanceBetweenRays, initialRayOrigin.y);
            Debug.DrawRay(ray, rayDirection * rayDistance, Color.red);

            _raycastHit = IsCurrentObj(ray, rayDirection, rayDistance);
            if (_raycastHit)
            {
                deltaMovement.y = _raycastHit.point.y - ray.y;

                if (isGoingUp)
                {
                    deltaMovement.y -= skinWidth;
                    collisionState.above = true;
                }
                else
                {
                    deltaMovement.y += skinWidth;
                    collisionState.below = true;
                }

                MobileObj another = _raycastHit.collider.gameObject.GetComponent<MobileObj>();
                if (another)
                {
                    Hit(another);
                    another.BeHit(this);
                }

                if (Mathf.Abs(deltaMovement.y) < skinWidth + kSkinWidthFloatFudgeFactor)
                    break;
            }
        }
    }

    public override void Move(Vector2 deltaMovement)
    {
        collisionState.wasGroundLastFrame = collisionState.below;
        collisionState.Reset();

        PrimeRaycastOrigins();

        MoveVertically(ref deltaMovement);
        if (Mathf.Abs(deltaMovement.y) < moveThreshold)
            MoveHorizontally(ref deltaMovement);
        else
            deltaMovement.x = 0;
        transform.Translate(deltaMovement, Space.World);

        //计算实际速度
        velocity = deltaMovement / Time.deltaTime;
        collisionState.becameGroundedThisFrame = (!collisionState.wasGroundLastFrame && collisionState.below);
    }


    private void CalculateDistanceBetweenRays()
    {
        var colliderHeight = boxCollider.size.y * Mathf.Abs(transform.localScale.y) - 2*(skinWidth);
        var colliderWidth = boxCollider.size.x * Mathf.Abs(transform.localScale.x) - 2*(skinWidth);
        _verticalDistanceBetweenRays = colliderHeight / (totalHorizontalRays - 1);
        _horizontalDistanceBetweenRays = colliderWidth / (totalVerticalRays - 1);
    }

    private void PrimeRaycastOrigins()
    {
        var modifiedBounds = boxCollider.bounds;
        //modifiedBounds.Expand(-2*skinWidth);

        _raycastOrigins.topLeft = new Vector2(modifiedBounds.min.x + skinWidth, modifiedBounds.max.y - skinWidth);
        _raycastOrigins.topRight = new Vector2(modifiedBounds.max.x - skinWidth, modifiedBounds.max.y - skinWidth);
        _raycastOrigins.bottomLeft = new Vector2(modifiedBounds.min.x + skinWidth, modifiedBounds.min.y + skinWidth);
        _raycastOrigins.bottomRight = new Vector2(modifiedBounds.max.x - skinWidth, modifiedBounds.min.y + skinWidth);
    }

    private RaycastHit2D IsCurrentObj(Vector2 ray, Vector3 direction, float distance)
    {
        RaycastHit2D[] raycastHits = Physics2D.RaycastAll(ray, direction, distance);
        foreach (RaycastHit2D raycastHit in raycastHits)
        {
            if (raycastHit.collider.gameObject != gameObject && !raycastHit.collider.isTrigger)
            {
                return raycastHit;
            }
        }

        if (raycastHits.Length > 2)
            Debug.LogWarning("射线检测到两个以上物体！");

        return new RaycastHit2D();
    }

}

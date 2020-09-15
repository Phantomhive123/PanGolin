using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#region 外部类和接口
public class CollisionState2D
{
    public bool right;
    public bool left;
    public bool above;
    public bool below;
    public bool becameGroundedThisFrame;
    public bool wasGroundLastFrame;

    public bool HasCollision()
    {
        return below || right || left || above;
    }

    public void Reset()
    {
        right = left = above = below = false;
    }
}
#endregion

[RequireComponent(typeof(BoxCollider2D))]
public class PlayerMovement : MonoBehaviour
{
    #region 内部类/结构体
    private struct BoxRaycastOrigins
    {
        public Vector3 topLeft;
        public Vector3 topRight;
        public Vector3 bottomLeft;
        public Vector3 bottomRight;
    }
    #endregion

    #region protected变量
    [SerializeField]
    protected int totalHorizontalRays = 8;

    [SerializeField]
    protected int totalVerticalRays = 8;

    [SerializeField]
    protected float skinWidth = 0.02f;

    protected CollisionState2D collisionState = new CollisionState2D();
    #endregion

    #region private变量
    private float _verticalDistanceBetweenRays;
    private float _horizontalDistanceBetweenRays;
    private BoxCollider2D boxCollider;
    private BoxRaycastOrigins _raycastOrigins;
    private RaycastHit2D _raycastHit;
    #endregion

    #region public变量
    public bool isGrounded { get { return collisionState.below; } }
    #endregion

    private void Update()
    {
        
    }

    private void LateUpdate()
    {
        
    }


}

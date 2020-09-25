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

    protected override void Update()
    {
        base.Update();
        targetVelocity.x += dir.x * moveSpeed;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ElementType
{
    empty,
    wood,
    stone,
    magnet,
    fire,
    earth
}

public delegate void PlayerEvenetDelegate();

[RequireComponent(typeof(BoxCollider2D),typeof(Rigidbody2D))]
public class BasicElement : MonoBehaviour
{
    private BoxColliderRaycastOrigins raycastOrigins;
    private float skinWidth = 0.001f;
    private float rayDistance = 0.05f;
    private bool isGrounded = false;

    protected BoxCollider2D boxCollider;
    new protected Rigidbody2D rigidbody;

    public ElementType elementType = ElementType.empty;//后面应该改成静态的

    [SerializeField]
    protected float velocityJudgement = 0.01f;

    protected bool wasStaticLastFrame = true;

    public PlayerEvenetDelegate eventDelegate;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfGrounded();
    }

    private void LateUpdate()
    {
        //每一帧更新一个标志位，记录这一帧物体有没有移动
        wasStaticLastFrame = rigidbody.velocity.magnitude < velocityJudgement;
    }

    public void Combine(BasicElement anotherItemB)
    {
        if (wasStaticLastFrame && anotherItemB.wasStaticLastFrame)
            return;
        if (!wasStaticLastFrame && !anotherItemB.wasStaticLastFrame)
        {
            Debug.Log("两个都在动！");
            return;
        }

        eventDelegate?.Invoke();

        if (wasStaticLastFrame)
            BeHit(anotherItemB);
        else
            Hit(anotherItemB);
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        BasicElement basicElement = collision.gameObject.GetComponent<BasicElement>();
        if (!basicElement) return;
        Combine(basicElement);
    }

    protected virtual void Hit(BasicElement element)
    {
        
    }

    protected virtual void BeHit(BasicElement element)
    {
        
    }


    private void CheckIfGrounded()
    {
        var modifiedBounds = boxCollider.bounds;
        modifiedBounds.Expand(-3 * skinWidth);
        raycastOrigins.topLeft = new Vector2(modifiedBounds.min.x, modifiedBounds.max.y);
        raycastOrigins.topRight = new Vector2(modifiedBounds.max.x, modifiedBounds.max.y);
        raycastOrigins.bottomLeft = new Vector2(modifiedBounds.min.x, modifiedBounds.min.y);
        raycastOrigins.bottomRight = new Vector2(modifiedBounds.max.x, modifiedBounds.min.y);

        bool wasGroundLastFrame = isGrounded;

        isGrounded = RaycastCheck(raycastOrigins.bottomLeft, Vector2.down)
            || RaycastCheck(raycastOrigins.bottomRight, Vector2.down);

        if (wasGroundLastFrame && !isGrounded)
        {
            eventDelegate?.Invoke();
            rigidbody.velocity = Vector2.zero;
        }
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
}
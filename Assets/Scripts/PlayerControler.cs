using UnityEngine;
using System.Collections;

public class PlayerControler : MonoBehaviour
{
    const string horizontal = "Horizontal";
    const string jump = "Jump";

    [SerializeField] float m_threshold = 0.1f;
    [SerializeField] float m_maxSpeed = 2;
    [SerializeField] float m_acceleration = 5;
    [SerializeField] float m_jumpPower = 2;
    [SerializeField] float m_maxJumpDelay = 0.2f;
    [SerializeField] LayerMask m_groundLayer;
    [SerializeField] float m_groundCheckRadius = 0.5f;
    [SerializeField] float m_groundCheckDistance = 0.2f;
    [SerializeField] float m_capedFallSpeed = 5;

    Rigidbody2D m_rigidbody = null;

    bool m_grounded = false;

    float m_jumpPressDelay = 0;

    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();

        Event<AddClampEntityEvent>.Broadcast(new AddClampEntityEvent(gameObject, ClampEntityUpdateType.FixedUpdate));
    }
    
    void Update()
    {
        Move();

        Jump();
    }

    void FixedUpdate()
    {
        UpdateGround();

        CapFall();
    }

    void Move()
    {
        float moveValue = Input.GetAxisRaw(horizontal);
        if (Mathf.Abs(moveValue) > m_threshold)
        {
            float dir = Mathf.Sign(moveValue);
            moveValue = (Mathf.Abs(moveValue) - m_threshold) / (1 - m_threshold) * dir;
        }
        else moveValue = 0;

        float targetSpeed = moveValue * m_maxSpeed;

        var velocity = m_rigidbody.velocity;

        if (velocity.x < targetSpeed)
        {
            velocity.x += m_acceleration * Time.deltaTime;
            if (velocity.x > targetSpeed)
                velocity.x = targetSpeed;
        }
        else if (velocity.x > targetSpeed)
        {
            velocity.x -= m_acceleration * Time.deltaTime;
            if (velocity.x < targetSpeed)
                velocity.x = targetSpeed;
        }

        m_rigidbody.velocity = velocity;
    }

    void Jump()
    {
        if(m_jumpPressDelay >= 0)
            m_jumpPressDelay += Time.deltaTime;

        bool jumping = false;
        bool m_jumpPressed = Input.GetButtonDown(jump);
        bool m_jumpReleased = Input.GetButtonUp(jump);

        if (m_jumpReleased && !m_grounded)
            m_jumpPressDelay = -1;
        if(m_jumpPressDelay >= 0 && m_grounded)
        {
            if (m_jumpPressDelay < m_maxJumpDelay)
                jumping = true;
            m_jumpPressDelay = -1;
        }

        if (m_jumpPressed)
        {
            if (!m_grounded)
                m_jumpPressDelay = 0;
            else jumping = true;
        }

        if (!jumping)
            return;

        var velocity = m_rigidbody.velocity;

        m_jumpPressDelay = -1;
        velocity.y = m_jumpPower;

        m_rigidbody.velocity = velocity;
    }

    void UpdateGround()
    {
        var collider = Physics2D.OverlapCircle(transform.position + new Vector3(0, -m_groundCheckDistance, 0), m_groundCheckRadius, m_groundLayer.value);

        m_grounded = collider != null;
        if (collider == null)
            transform.parent = null;
        else transform.parent = collider.transform;
    }

    void CapFall()
    {
        var velocity = m_rigidbody.velocity;

        if (velocity.y < -m_capedFallSpeed)
            velocity.y = -m_capedFallSpeed;

        m_rigidbody.velocity = velocity;
    }
}

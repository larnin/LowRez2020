using UnityEngine;
using System.Collections;
using DG.Tweening;

[RequireComponent(typeof(PlayerControler))]
public class AttackBehaviour : MonoBehaviour
{
    const string attack = "Fire1";
    const string attackProperty = "Attack";

    [SerializeField] GameObject m_attackPrefab = null;
    [SerializeField] Vector2 m_attackOffset = new Vector2(1, 0);
    [SerializeField] float m_attackDelay = 0.1f;
    [SerializeField] float m_attackDuration = 0.3f;
    [SerializeField] float m_attackCooldown = 0.5f;

    PlayerControler m_controler;

    Animator m_animator;

    float m_lastAttackTime = 0;

    private void Start()
    {
        m_controler = GetComponent<PlayerControler>();
        m_animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(Input.GetButtonDown(attack) && m_lastAttackTime < Time.time)
            Attack();
    }

    void Attack()
    {
        DOVirtual.DelayedCall(m_attackDelay, () =>
        {
            if (this == null)
                return;

            var obj = Instantiate(m_attackPrefab);
            Destroy(obj, m_attackDuration);

            var pos = transform.position;
            var dir = m_controler.IsLeft() ? -1 : 1;
            pos.x += m_attackOffset.x * dir;
            pos.y += m_attackOffset.y;

            obj.transform.parent = transform;
            obj.transform.position = pos;
        });

        m_animator.SetTrigger(attackProperty);

        m_lastAttackTime = Time.time + m_attackCooldown;
    }
}

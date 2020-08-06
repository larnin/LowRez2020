using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

public class TeleportSystem : MonoBehaviour
{
    const string percentAttribute = "_Percent";
    const string posAttribute = "_PortalPos";

    [SerializeField] int m_startSide = 0;
    [SerializeField] Material m_material = null;
    [SerializeField] float m_cooldown = 1;
    [SerializeField] float m_worldOffset = 20;
    [SerializeField] float m_worldSize = 8;
    [SerializeField] float m_transitionDuration = 0.5f;

    SubscriberList m_subscriberList = new SubscriberList();

    float m_teleportTime = 0;

    private void Awake()
    {
        m_subscriberList.Add(new Event<TeleportEvent>.Subscriber(OnTeleport));
        m_subscriberList.Subscribe();

        m_material.SetFloat(percentAttribute, m_startSide == 0 ? 0 : 1);
    }

    private void OnDestroy()
    {
        m_subscriberList.Unsubscribe();
    }

    void OnTeleport(TeleportEvent e)
    {
        if (m_teleportTime > Time.time)
            return;

        m_teleportTime = Time.time + m_cooldown;

        var pos = e.entity.transform.position;

        float targetPercent = pos.x < 0 ? 1 : 0;
        m_material.DOFloat(targetPercent, percentAttribute, m_transitionDuration);

        var sign = Mathf.Sign(pos.x);

        Vector2 portalPos = new Vector2(pos.x - sign * m_worldOffset, pos.y);
        portalPos /= m_worldSize;
        portalPos.x += 0.5f;
        portalPos.y += 0.5f;
        m_material.SetVector(posAttribute, portalPos);


        if (sign < 0)
        {
            pos.x -= sign * m_worldOffset * 2;
            e.entity.transform.position = pos;
        }
        else
        {
            DOVirtual.DelayedCall(m_transitionDuration * 0.7f,
                () =>
                {
                    if (e.entity == null)
                        return;
                    pos = e.entity.transform.position;
                    pos.x -= sign * m_worldOffset * 2;
                    e.entity.transform.position = pos;
                });
        }
    }
}
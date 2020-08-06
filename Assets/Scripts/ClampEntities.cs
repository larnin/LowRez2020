using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClampEntities : MonoBehaviour
{
    [SerializeField] List<GameObject> m_zones = new List<GameObject>();

    [SerializeField] float m_worldSize = 8;

    SubscriberList m_subscriberList = new SubscriberList();

    List<GameObject> m_updateEntities = new List<GameObject>();
    List<GameObject> m_fixedEntities = new List<GameObject>();

    private void Awake()
    {
        m_subscriberList.Add(new Event<AddClampEntityEvent>.Subscriber(AddEntity));
        m_subscriberList.Add(new Event<RemoveClampEntityEvent>.Subscriber(RemoveEntity));
        m_subscriberList.Subscribe();
    }

    private void OnDestroy()
    {
        m_subscriberList.Unsubscribe();
    }

    void AddEntity(AddClampEntityEvent e)
    {
        if (m_updateEntities.Contains(e.entity))
            return;
        if (m_fixedEntities.Contains(e.entity))
            return;

        var list = e.type == ClampEntityUpdateType.FixedUpdate ? m_fixedEntities : m_updateEntities;
        list.Add(e.entity);
    }

    void RemoveEntity(RemoveClampEntityEvent e)
    {
        m_updateEntities.Remove(e.entity);
        m_fixedEntities.Remove(e.entity);
    }

    private void LateUpdate()
    {
        UpdateEntities(m_updateEntities);
    }

    private void FixedUpdate()
    {
        UpdateEntities(m_fixedEntities);
    }

    void UpdateEntities(List<GameObject> list)
    {
        list.RemoveAll(x => { return x == null; });

        if (m_zones.Count == 0)
            return;

        foreach(var e in list)
        {
            var pos = e.transform.position;

            var target = Vector3.zero;
            float dist = float.MaxValue;
            foreach(var z in m_zones)
            {
                var zPos = z.transform.position;
                float zDist = (pos - zPos).sqrMagnitude;
                if (zDist < dist)
                {
                    target = zPos;
                    dist = zDist;
                }
            }

            pos -= target;

            pos.x += m_worldSize / 2;
            pos.y += m_worldSize / 2;

            if (pos.x >= 0)
                pos.x = pos.x % m_worldSize;
            else pos.x = m_worldSize + (pos.x % m_worldSize);

            if (pos.y > 0)
                pos.y = pos.y % m_worldSize;
            else pos.y = m_worldSize + (pos.y % m_worldSize);

            pos.x -= m_worldSize / 2;
            pos.y -= m_worldSize / 2;

            pos += target;

            e.transform.position = pos;
        }
    }
}

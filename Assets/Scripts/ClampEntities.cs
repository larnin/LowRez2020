using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClampEntities : MonoBehaviour
{
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

        foreach(var e in list)
        {
            var pos = e.transform.position;
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

            e.transform.position = pos;
        }
    }
}

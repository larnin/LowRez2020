using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public enum ClampEntityUpdateType
{
    LateUpdate,
    FixedUpdate,
}

public class AddClampEntityEvent
{
    public GameObject entity;
    public ClampEntityUpdateType type;

    public AddClampEntityEvent(GameObject _entity, ClampEntityUpdateType _type)
    {
        entity = _entity;
        type = _type;
    }
}

public class RemoveClampEntityEvent
{
    public GameObject entity;

    public RemoveClampEntityEvent(GameObject _entity)
    {
        entity = _entity;
    }
}
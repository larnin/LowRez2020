using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class TeleportEvent
{
    public GameObject entity;

    public TeleportEvent(GameObject _entity)
    {
        entity = _entity;
    }
}

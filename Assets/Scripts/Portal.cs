using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<PlayerControler>();
        if (player == null)
            return;

        Event<TeleportEvent>.Broadcast(new TeleportEvent(other.gameObject));
    }
}

using UnityEngine;
using System.Collections;

public class HitBehaviour : MonoBehaviour
{
    [SerializeField] bool m_fromPlayer = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(m_fromPlayer)
        {
            var hitable = collision.GetComponent<Hitable>();
            if (hitable != null)
                hitable.Hit();
        }
        else
        {
            var player = collision.GetComponent<PlayerControler>();
            if (player != null)
                player.Hit();
        }
    }
}

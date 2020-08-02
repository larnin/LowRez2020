using UnityEngine;
using System.Collections;

public class ClampPosition : MonoBehaviour
{
    [SerializeField] float m_unite = 0;

    private void LateUpdate()
    {
        var pos = transform.position;
        pos.x = Mathf.Ceil(pos.x * m_unite) / m_unite;
        pos.y = Mathf.Ceil(pos.y * m_unite) / m_unite;
        transform.position = pos;
    }
}

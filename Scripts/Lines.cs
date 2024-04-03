using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lines : MonoBehaviour
{
    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 1, 1f);
        Gizmos.DrawLine(new Vector3(11f, 6, 1) + transform.position, new Vector3(-11f, 6, 1) + transform.position);
        Gizmos.DrawLine(new Vector3(11f, -6, 1) + transform.position, new Vector3(-11f, -6, 1) + transform.position);
        Gizmos.DrawLine(new Vector3(11f, 6, 1) + transform.position, new Vector3(11f, -6, 1) + transform.position);
        Gizmos.DrawLine(new Vector3(-11f, 6, 1) + transform.position, new Vector3(-11f, -6, 1) + transform.position);

        /*Gizmos.DrawLine(new Vector3(11f, 18f, 1), new Vector3(11f, -18f, 1));
        Gizmos.DrawLine(new Vector3(-11f, 18f, 1), new Vector3(-11f, -18f, 1));        
        Gizmos.DrawLine(new Vector3(33f, 18f, 1), new Vector3(33f, -18f, 1));
        Gizmos.DrawLine(new Vector3(-33f, 18f, 1), new Vector3(-33f, -18f, 1));

        Gizmos.DrawLine(new Vector3(33f, 18f, 1), new Vector3(-33f, 18f, 1));
        Gizmos.DrawLine(new Vector3(33f, 6f, 1), new Vector3(-33f, 6f, 1));
        Gizmos.DrawLine(new Vector3(33f, -6f, 1), new Vector3(-33f, -6f, 1));
        Gizmos.DrawLine(new Vector3(33f, -18f, 1), new Vector3(-33f, -18f, 1));*/

    }
}

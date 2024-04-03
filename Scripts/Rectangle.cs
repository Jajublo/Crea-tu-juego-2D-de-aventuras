using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rectangle : MonoBehaviour
{
    void OnDrawGizmosSelected()
    {
        // Draw a semitransparent red cube at the transforms position
        Gizmos.color = new Color(1, 0, 0, 0.2f);
        Gizmos.DrawCube(new Vector3(0, 0, 0), new Vector3(22, 12, 1));
        Gizmos.DrawCube(new Vector3(22, 12, 0), new Vector3(22, 12, 1));
        Gizmos.DrawCube(new Vector3(-22, 12, 0), new Vector3(22, 12, 1));
        Gizmos.DrawCube(new Vector3(22, -12, 0), new Vector3(22, 12, 1));
        Gizmos.DrawCube(new Vector3(-22, -12, 0), new Vector3(22, 12, 1));
        Gizmos.color = new Color(0, 0, 1, 0.2f);
        Gizmos.DrawCube(new Vector3(22, 0, 0), new Vector3(22, 12, 1));
        Gizmos.DrawCube(new Vector3(-22, 0, 0), new Vector3(22, 12, 1));
        Gizmos.DrawCube(new Vector3(0, 12, 0), new Vector3(22, 12, 1));
        Gizmos.DrawCube(new Vector3(0, -12, 0), new Vector3(22, 12, 1));
    }
}

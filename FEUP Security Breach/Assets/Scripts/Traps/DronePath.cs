using UnityEditor;
using UnityEngine;

/// <summary>
/// Drone path;
/// Provides the var "nodes" corresponding to the patrol path.
/// Provides the var "returnPaths" corresponding to the paths for the drone to return to its patrol path
/// </summary>
public class DronePath : MonoBehaviour
{
    public Vector2[] nodes;
    public RecoveryPath[] returnPaths;
    public int moveRange = 10;
    [System.Serializable]
    public struct RecoveryPath
    {
        public Vector2[] nodes;
    }
    private void OnDrawGizmosSelected()
    {
        DrawGizmos();
    }

    public void DrawGizmos()
    {
        for (int i = 0; i < nodes.Length - 1; i++)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(nodes[i], 0.3f);
            Gizmos.color = Color.red;
            Gizmos.DrawLine(nodes[i], nodes[i + 1]);
        }
        if (nodes.Length > 1)
        {
            Gizmos.DrawLine(nodes[nodes.Length - 1], nodes[0]);
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(nodes[nodes.Length - 1], 0.3f);
        }
        for (int i = 0; i < returnPaths.Length; i++)
        {
            for (int j = 0; j < returnPaths[i].nodes.Length - 1; j++)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(returnPaths[i].nodes[j], 0.3f);
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(returnPaths[i].nodes[j], returnPaths[i].nodes[j + 1]);
            }
            if (returnPaths[i].nodes.Length > 0)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(returnPaths[i].nodes[returnPaths[i].nodes.Length - 1], 0.3f);
            }
        }
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, moveRange);
    }
}

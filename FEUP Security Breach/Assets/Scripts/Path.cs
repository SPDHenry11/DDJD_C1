using UnityEngine;

public class Path : MonoBehaviour
{
    [HideInInspector] public Vector2[] nodes;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        for (int i = 0; i < transform.childCount - 1; i++)
        {
            Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild(i + 1).position);
        }
        if (transform.childCount > 1) Gizmos.DrawLine(transform.GetChild(transform.childCount - 1).position, transform.GetChild(0).position);
    }

    // Update is called once per frame
    void Awake()
    {
        nodes = new Vector2[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            nodes[i] = transform.GetChild(i).position;
        }
    }
}

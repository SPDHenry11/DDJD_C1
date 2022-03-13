using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Drone : MonoBehaviour
{
    [SerializeField] private Path path;
    private int destination;
    private Rigidbody2D rb;
    private float upDirection = 0;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        float closest = 100, secondClosest = 100;
        int closestIndex = 0, secondClosestIndex = 1;
        Vector2 pos = transform.position;
        for (int i = 0; i < path.nodes.Length; i++)
        {
            float distance = Vector2.Distance(pos, path.nodes[i]);
            if (distance < closest)
            {
                closest = distance;
                closestIndex = i;
            }
            else if (distance < secondClosest)
            {
                secondClosest = distance;
                secondClosestIndex = i;
            }
        }
        if ((closestIndex > secondClosestIndex && closestIndex > 0) || (closestIndex == 0 && secondClosestIndex == path.nodes.Length - 1)) destination = closestIndex;
        else destination = secondClosestIndex;

        StartCoroutine(Patrol());
    }

    void Update()
    {
        rb.AddTorque(Vector2.SignedAngle(new Vector2(transform.up.x, transform.up.y), new Vector2(upDirection, 1)) / 90, ForceMode2D.Force);
    }

    IEnumerator Patrol()
    {
        while (true)
        {
            Vector2 nextNode = path.nodes[destination];
            while (Vector2.Distance(transform.position, nextNode) > 1f)
            {
                Vector2 force = Vector2.ClampMagnitude((nextNode - new Vector2(transform.position.x, transform.position.y))/2, 2);
                rb.AddForce(force, ForceMode2D.Force);
                upDirection = force.x / 4;

                yield return null;
            }
            destination++;
            if (destination >= path.nodes.Length) destination = 0;
        }
    }
}

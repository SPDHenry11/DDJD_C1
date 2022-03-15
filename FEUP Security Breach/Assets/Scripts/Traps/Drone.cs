using System.Collections;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Drone : MonoBehaviour
{
    [SerializeField] private DronePath path;
    [SerializeField] private LayerMask visibleLayers;
    [SerializeField] private int sightRange = 15;
    [SerializeField] private int deathRange = 3;
    private int destination;
    private Rigidbody2D rb;
    private float upDirection = 0;
    private Transform target;

    private void OnDrawGizmosSelected()
    {
        path.DrawGizmos();
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, deathRange);
    }
    void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        FindDestination();
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
                RaycastHit2D hit = Physics2D.Raycast(transform.position, target.position - transform.position, sightRange, visibleLayers);
                if (!GameController.imunity && hit.collider != null && hit.collider.tag.Equals("Player"))
                {
                    yield return null;
                    StartCoroutine(Chase());
                    yield break;
                }
                Vector2 force = Vector2.ClampMagnitude((nextNode - new Vector2(transform.position.x, transform.position.y)) * Time.deltaTime * 100, 2);
                rb.AddForce(force, ForceMode2D.Force);
                upDirection = force.x / 4;
                yield return null;
            }
            destination++;
            if (destination >= path.nodes.Length) destination = 0;
        }
    }
    IEnumerator Chase()
    {
        Vector2 lastPos = target.position;
        while (!GameController.imunity && Vector2.Distance(transform.position, lastPos) > 1f)
        {
            Vector2 force = Vector2.ClampMagnitude((lastPos - new Vector2(transform.position.x, transform.position.y)) * Time.deltaTime * 200, 4);
            rb.AddForce(force, ForceMode2D.Force);
            upDirection = force.x / 4;
            yield return null;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, target.position - transform.position, sightRange, visibleLayers);
            if (hit.collider != null && hit.collider.tag.Equals("Player"))
            {
                lastPos = target.position;
                if (Vector2.Distance(target.position, transform.position) <= deathRange)
                {
                    UIController.instance.Busted();
                    StartCoroutine(Return());
                    yield break;
                }
            }
            if (Vector2.Distance(transform.position, path.transform.position) > path.moveRange)
            {
                StartCoroutine(Return());
                yield break;
            }
        }
        StartCoroutine(Return());
    }

    IEnumerator Return()
    {
        int returnPath = FindReturnPath();
        if (returnPath < 0)
        {
            yield return null;
            StartCoroutine(Patrol());
            yield break;
        }
        while (true)
        {
            Vector2 nextNode = path.returnPaths[returnPath].nodes[destination];
            while (Vector2.Distance(transform.position, nextNode) > 1f)
            {
                Vector2 force = Vector2.ClampMagnitude((nextNode - new Vector2(transform.position.x, transform.position.y)) * Time.deltaTime * 100, 2);
                rb.AddForce(force, ForceMode2D.Force);
                upDirection = force.x / 4;
                yield return null;
            }
            destination++;
            if (destination >= path.returnPaths[returnPath].nodes.Length)
            {
                yield return null;
                FindDestination();
                StartCoroutine(Patrol());
                yield break;
            }
        }
    }

    private void FindDestination()
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
    }

    private int FindReturnPath()
    {
        float closest = 100, secondClosest = 100;
        int closestIndex = 0, secondClosestIndex = 0, bestPath = 0, secondBestPath = 0;
        Vector2 pos = transform.position;
        for (int i = 0; i < path.returnPaths.Length; i++)
        {
            for (int j = 0; j < path.returnPaths[i].nodes.Length; j++)
            {
                float distance = Vector2.Distance(pos, path.returnPaths[i].nodes[j]);
                if (distance < closest)
                {
                    closest = distance;
                    closestIndex = j;
                    bestPath = i;
                }
                else if (distance < secondClosest)
                {
                    secondClosest = distance;
                    secondClosestIndex = j;
                    secondBestPath = i;
                }
            }
        }
        bool foundCloser = false, foundSecondCloser = false;
        for (int i = 0; i < path.nodes.Length; i++)
        {
            float distance = Vector2.Distance(pos, path.nodes[i]);
            if (distance < closest)
            {
                closest = distance;
                closestIndex = i;
                foundCloser = true;
            }
            else if (distance < secondClosest)
            {
                secondClosest = distance;
                secondClosestIndex = i;
                foundSecondCloser = true;
            }
        }
        if (foundCloser)
        {
            if (foundSecondCloser)
            {
                if ((closestIndex > secondClosestIndex && closestIndex > 0) || (closestIndex == 0 && secondClosestIndex == path.nodes.Length - 1)) destination = closestIndex;
                else destination = secondClosestIndex;
            }
            else
            {
                destination = closestIndex;
            }
            return -1;
        }
        if ((closestIndex > secondClosestIndex && closestIndex > 0) || (closestIndex == 0 && secondClosestIndex == path.nodes.Length - 1))
        {
            destination = closestIndex;
            return bestPath;
        }
        else
        {
            destination = secondClosestIndex;
            return secondBestPath;
        }
    }
}

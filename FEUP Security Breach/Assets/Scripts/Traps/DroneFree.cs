using System.Collections;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
/// <summary>
/// Drone full behaviour
/// </summary>
public class DroneFree : MonoBehaviour
{
    [SerializeField] private LayerMask visibleLayers;
    [SerializeField] private int sightRange = 15;
    [SerializeField] private int deathRange = 2;
    [SerializeField] private int maxSpeed = 12;
    private int destination;
    private Rigidbody2D rb;
    private float upDirection = 0;
    private Transform target;

    private void OnDrawGizmosSelected()
    {
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
        StartCoroutine(Chase());
    }

    void FixedUpdate()
    {
        rb.AddTorque(Vector2.SignedAngle(new Vector2(transform.up.x, transform.up.y), new Vector2(upDirection, 1)) / 90, ForceMode2D.Force);
    }

    IEnumerator Chase()
    {
        while (true)
        {
            if (PlayerOnSight())
            {
                AudioController.instance.Play("DroneDetection");
                Vector2 lastPos = target.position;
                while (!GameController.imunity && Vector2.Distance(transform.position, lastPos) > 1f)
                {
                    Vector2 force = Vector2.ClampMagnitude((lastPos - new Vector2(transform.position.x, transform.position.y)) * Time.fixedDeltaTime * 200, 20);
                    if (rb.velocity.magnitude < maxSpeed) rb.AddForce(force, ForceMode2D.Force);
                    upDirection = force.x / 20;
                    yield return new WaitForFixedUpdate();
                    if (PlayerOnSight())
                    {
                        lastPos = target.position;
                        if (Vector2.Distance(target.position, transform.position) <= deathRange)
                        {
                            UIController.instance.Busted();
                        }
                    }
                }
                upDirection = 0;
            }
            yield return null;
        }
    }

    private bool PlayerOnSight()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, target.position - transform.position, sightRange, visibleLayers);
        return (!GameController.imunity && hit.collider != null && hit.collider.tag.Equals("Player"));
    }
}

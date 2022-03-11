using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Transform target;
    private Vector2 velocity;
    public Vector2 minBounds;
    public Vector2 maxBounds;
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(minBounds, 0.5f);
        Gizmos.DrawSphere(maxBounds, 0.5f);
    }

    void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        transform.position = new Vector3(
            Mathf.SmoothDamp(transform.position.x, target.position.x, ref velocity.x, 0.2f),
            Mathf.SmoothDamp(transform.position.y, target.position.y, ref velocity.y, 0.2f),
            -10
        );
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, minBounds.x, maxBounds.x),
            Mathf.Clamp(transform.position.y, minBounds.y, maxBounds.y),
            -10
        );

    }
}

using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
/// <summary>
/// Limits the rigidbody falling speed
/// </summary>
public class FallSpeedLimit : MonoBehaviour
{
    private Rigidbody2D rb;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (rb.velocity.y < -25)
        {
            rb.velocity = new Vector2(rb.velocity.x, -25);
        }
    }
}

using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    public static Movement instance;
    public float speed = 5;
    public float jumpForce = 20;
    [SerializeField] private Transform groundCheckPivot;
    [SerializeField] private LayerMask groundLayer;
    private Rigidbody2D rb;
    [HideInInspector] public bool grounded = false;
    private Vector3 velocity = Vector3.zero;

    //Visualize Ground check collision
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheckPivot.position, 0.4f);
    }
    void Awake()
    {
        instance = this;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        SetGrounded();
        if (Input.GetButton("Jump") && grounded) rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    private void FixedUpdate()
    {
        if (grounded) rb.velocity = Vector3.SmoothDamp(rb.velocity, new Vector2(Input.GetAxis("Horizontal") * speed, rb.velocity.y), ref velocity, 0.1f);
        else rb.AddForce(new Vector2(Input.GetAxis("Horizontal") * speed, 0), ForceMode2D.Force);
    }

    private void SetGrounded()
    {
        Collider2D collider = Physics2D.OverlapCircle(groundCheckPivot.position, 0.4f, groundLayer);
        if (collider != null)
        {
            if (!grounded)
            {
                //landing sound or animation or effects
            }
            grounded = true;
        }
        else grounded = false;
    }

}
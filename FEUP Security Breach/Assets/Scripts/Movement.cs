using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
/// <summary>
/// Manages all the player's movement
/// </summary>
public class Movement : MonoBehaviour
{
    public static Movement instance;
    public float speed = 5;
    public float jumpForce = 5;
    [SerializeField] private Transform groundCheckPivot;
    [SerializeField] private Transform gun;
    [SerializeField] private LayerMask groundLayer;
    private Rigidbody2D rb;
    [HideInInspector] public bool grounded = false;
    private Vector3 velocity = Vector3.zero;
    private bool facingRight = true;
    private Animator anim;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheckPivot.position, 0.4f);
    }
    void Awake()
    {
        instance = this;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        //flip character
        var delta = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        if (!facingRight && delta.x >= 0)
        {
            transform.eulerAngles = new Vector3(0f, 0, 0f);
            gun.transform.Rotate(180, 0, 0);
            facingRight = true;
        }
        else if (facingRight && delta.x < 0)
        {
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
            gun.transform.Rotate(180, 0, 0);
            facingRight = false;
        }
        SetGrounded();
        if (Input.GetButton("Jump") && grounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    private void FixedUpdate()
    {
        if (grounded) rb.velocity = Vector3.SmoothDamp(rb.velocity, new Vector2(Input.GetAxis("Horizontal") * speed, rb.velocity.y), ref velocity, 0.1f);
        else
        {
            float horizontal = Input.GetAxis("Horizontal");
            if(!((horizontal<0 && rb.velocity.x<-speed) || (horizontal>0 && rb.velocity.x>speed)))
               rb.AddForce(new Vector2(Input.GetAxis("Horizontal") * speed, 0), ForceMode2D.Force);
        }
    }

    private void SetGrounded()
    {
        Collider2D collider = Physics2D.OverlapCircle(groundCheckPivot.position, 0.4f, groundLayer);
        if (collider != null)
        {
            if (!grounded)
            {
                anim.SetBool("grounded", true);
                grounded = true;
            }
            if (facingRight) anim.SetFloat("speed", Input.GetAxis("Horizontal") * speed);
            else anim.SetFloat("speed", Input.GetAxis("Horizontal") * -1);
        }
        else
        {
            if (grounded) anim.SetBool("grounded", false);
            grounded = false;
        }
    }

    void OnDisable()
    {
        anim.SetBool("grounded", true);
        anim.SetFloat("speed", 0);
    }

    void OnEnable()
    {
        if (!facingRight)
        {
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
            facingRight = false;
        }
    }

}
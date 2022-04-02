using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Pressure button full behaviour.
/// </summary>
public class PressureButton : MonoBehaviour
{
    private float maxHeight;
    public UnityEvent OnPressed, OnRelease;
    private Rigidbody2D rb;
    private float pressHeight;
    void Awake()
    {
        maxHeight = transform.position.y;
        pressHeight = maxHeight - transform.lossyScale.y / 4;
        rb = GetComponent<Rigidbody2D>();
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), transform.parent.GetComponent<Collider2D>());
    }

    void Update()
    {
        if (transform.position.y >= maxHeight)
            transform.position = new Vector2(transform.position.x, maxHeight);
        else
            rb.AddForce(transform.up * 100 * Time.deltaTime);

        if (gameObject.layer == 0 && transform.position.y < pressHeight)
        {
            AudioController.instance.Play("Press");
            OnPressed.Invoke();
            gameObject.layer = 6;
        }
        else if (gameObject.layer == 6 && transform.position.y >= pressHeight)
        {
            AudioController.instance.Play("Release");
            gameObject.layer = 0;
            OnRelease.Invoke();

        }
    }

}

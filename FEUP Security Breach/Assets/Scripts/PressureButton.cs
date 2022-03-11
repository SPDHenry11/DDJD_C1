using UnityEngine;
using UnityEngine.Events;

public class PressureButton : MonoBehaviour
{
    private float maxHeight;
    public UnityEvent OnPressed, OnRelease;
    private Rigidbody2D rb;
    // Update is called once per frame
    void Awake()
    {
        maxHeight = transform.localPosition.y;
        rb = GetComponent<Rigidbody2D>();
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), transform.parent.GetComponent<Collider2D>());
    }

    void Update()
    {
        if (transform.localPosition.y >= maxHeight)
            transform.localPosition = new Vector2(transform.localPosition.x, maxHeight);
        else
            rb.AddForce(transform.up * 100 * Time.deltaTime);

        if (gameObject.layer == 0 && transform.localPosition.y < maxHeight - 0.2f)
        {
            OnPressed.Invoke();
            gameObject.layer = 6;
        }
        else if (gameObject.layer == 6 && transform.localPosition.y >= maxHeight - 0.2f)
        {
            gameObject.layer = 0;
            OnRelease.Invoke();

        }
    }

}

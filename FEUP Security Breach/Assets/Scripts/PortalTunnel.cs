using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
/// <summary>
/// Adds the ability to go through portals
/// </summary>
public class PortalTunnel : MonoBehaviour
{
    private bool pause = false;
    [SerializeField] private UnityEvent onTunnel;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!pause)
        {
            if (other.tag.Equals("OrangePortal") && PortalGun.instantiatedPortals[1] != null)
            {
                Rigidbody2D rb = GetComponent<Rigidbody2D>();
                if (rb.velocity.magnitude > 1 && Vector3.Angle(rb.velocity, -other.transform.up) < 80) Tunnel(0, 1);
            }
            else if (other.tag.Equals("BluePortal") && PortalGun.instantiatedPortals[0] != null)
            {
                Rigidbody2D rb = GetComponent<Rigidbody2D>();
                if (rb.velocity.magnitude > 1 && Vector3.Angle(rb.velocity, -other.transform.up) < 80) Tunnel(1, 0);
            }
        }
    }

    private void Tunnel(int entrance, int exit)
    {
        AudioController.instance.Play("Tunnel");
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        Vector2 velocity = rb.velocity;
        Transform exitPortal = PortalGun.instantiatedPortals[exit].transform;
        float angle = 180 + exitPortal.eulerAngles.z - PortalGun.instantiatedPortals[entrance].transform.rotation.eulerAngles.z;
        transform.position = exitPortal.position + exitPortal.up / 2;
        rb.velocity = Quaternion.AngleAxis(angle, Vector3.forward) * rb.velocity;
        rb.velocity *= Vector3.Dot(rb.velocity.normalized, exitPortal.up);
        onTunnel.Invoke();
        StartCoroutine(Pause());
    }

    IEnumerator Pause()
    {
        pause = true;
        yield return new WaitForSeconds(0.5f);
        pause = false;
    }
}

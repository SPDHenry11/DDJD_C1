using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PortalTunnel : MonoBehaviour
{
    private bool pause = false;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!pause)
        {
            if (other.tag.Equals("OrangePortal") && PortalGun.instantiatedPortals[1] != null)
            {
                Tunnel(0, 1);
            }
            else if (other.tag.Equals("BluePortal") && PortalGun.instantiatedPortals[0] != null)
            {
                Tunnel(1, 0);
            }
        }
    }

    private void Tunnel(int entrance, int exit)
    {
        Movement.instance.grounded = false;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        Vector2 velocity = rb.velocity;
        Transform exitPortal = PortalGun.instantiatedPortals[exit].transform;
        float angle = 180 + exitPortal.eulerAngles.z - PortalGun.instantiatedPortals[entrance].transform.rotation.eulerAngles.z;
        transform.position = exitPortal.position + exitPortal.up;
        rb.velocity = Quaternion.AngleAxis(angle, Vector3.forward) * rb.velocity;
        rb.velocity *= Vector3.Dot(rb.velocity.normalized, exitPortal.up);
        StartCoroutine(Pause());
    }

    IEnumerator Pause()
    {
        pause = true;
        yield return new WaitForSeconds(0.5f);
        pause = false;
    }
}

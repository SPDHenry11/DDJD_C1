using System.Collections;
using UnityEngine;

/// <summary>
/// Security Camera full behaviour
/// </summary>
public class SecurityCamera : MonoBehaviour
{
    [SerializeField] private float angle = 45;
    [SerializeField] private float fov = 90;
    [SerializeField] private float range = 10;
    [SerializeField] private LayerMask visibleLayers;
    private Transform target;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + Quaternion.Euler(0, 0, -angle / 2) * -transform.up);
        Gizmos.DrawLine(transform.position, transform.position + Quaternion.Euler(0, 0, angle / 2) * -transform.up);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Quaternion.Euler(0, 0, -fov / 2) * -transform.up * range);
        Gizmos.DrawLine(transform.position, transform.position + Quaternion.Euler(0, 0, fov / 2) * -transform.up * range);
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position, range);
    }
    void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(Patrol());
    }

    IEnumerator Patrol()
    {
        float destination = -angle / 2;
        float currentAngle = transform.localEulerAngles.z;
        if (currentAngle > 180) currentAngle -= 360;
        float time = 0;
        while (true)
        {
            while (currentAngle > destination)
            {
                yield return null;
                currentAngle -= 20 * Time.deltaTime;
                transform.localEulerAngles = new Vector3(0, 0, currentAngle);

                if (PlayerOnSight())
                {
                    StartCoroutine(Follow());
                    yield break;
                }
            }
            transform.localEulerAngles = new Vector3(0, 0, destination);
            currentAngle = destination;
            time = Time.time;
            while (Time.time - time < 2)
            {
                if (PlayerOnSight())
                {
                    StartCoroutine(Follow());
                    yield break;
                }
                yield return null;
            }
            destination = angle / 2;
            while (currentAngle < destination)
            {
                yield return null;
                currentAngle += 20 * Time.deltaTime;
                transform.localEulerAngles = new Vector3(0, 0, currentAngle);
                if (PlayerOnSight())
                {
                    StartCoroutine(Follow());
                    yield break;
                }
            }
            currentAngle = destination;
            transform.localEulerAngles = new Vector3(0, 0, destination);
            time = Time.time;
            while (Time.time - time < 2)
            {
                if (PlayerOnSight())
                {
                    StartCoroutine(Follow());
                    yield break;
                }
                yield return null;
            }
            destination = -angle / 2;
        }
    }

    IEnumerator Follow()
    {
        GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 0.5f);
        AudioController.instance.Play("SecurityCamera");
        transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 0.5f);
        float currentAngle = 0;
        float time = 0;
        while (PlayerOnSight())
        {
            transform.up = Vector3.RotateTowards(transform.up, -(target.position - transform.position), Mathf.PI / 2 * Time.deltaTime, 0.0f);
            currentAngle = transform.localEulerAngles.z;
            if (currentAngle > 180) currentAngle -= 360;
            float clamped = Mathf.Clamp(currentAngle, -angle / 2, angle / 2);
            transform.localEulerAngles = new Vector3(0, 0, clamped);
            time += Time.deltaTime;
            yield return null;
            if (time >= 1.5f)
            {
                UIController.instance.Busted();
            }
        }
        float t = Time.time;
        while (!GameController.imunity && Time.time - t < 2)
        {
            if (PlayerOnSight())
            {
                StartCoroutine(Follow());
                yield break;
            }
            time -= Time.deltaTime;
            yield return null;
        }
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.7f);
        transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.7f);
        StartCoroutine(Patrol());
    }

    void OnDisable()
    {
        StopAllCoroutines();
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void Switch(bool on)
    {
        if (on)
        {
            StartCoroutine(Patrol());
            transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            StopAllCoroutines();
            transform.GetChild(0).gameObject.SetActive(false);
            AudioController.instance.Play("SecurityCameraOn");
        }
    }

    private bool PlayerOnSight()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, target.position - transform.position, range, visibleLayers);
        if(hit.collider!=null) return (!GameController.imunity && Vector2.Distance(target.position, transform.position) < range &&
            Vector2.Angle(-transform.up, (target.position - transform.position)) < fov / 2 && hit.collider.tag.Equals("Player"));
        return false;
    }
}

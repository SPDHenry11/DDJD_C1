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
    private Transform target;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + Quaternion.Euler(0, 0, -angle / 2) * Vector3.down);
        Gizmos.DrawLine(transform.position, transform.position + Quaternion.Euler(0, 0, angle / 2) * Vector3.down);
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
        float currentAngle = transform.eulerAngles.z;
        if (currentAngle > 180) currentAngle -= 360;
        float time = 0;
        while (true)
        {
            while (currentAngle > destination)
            {
                yield return null;
                currentAngle -= 20 * Time.deltaTime;
                transform.eulerAngles = new Vector3(0, 0, currentAngle);
                if (!GameController.imunity && Vector2.Distance(target.position, transform.position) < range && Vector2.Angle(-transform.up, (target.position - transform.position)) < fov / 2)
                {
                    StartCoroutine(Follow());
                    yield break;
                }
            }
            transform.eulerAngles = new Vector3(0, 0, destination);
            currentAngle = destination;
            time = Time.time;
            while (Time.time - time < 2)
            {
                if (!GameController.imunity && Vector2.Distance(target.position, transform.position) < range && Vector2.Angle(-transform.up, (target.position - transform.position)) < fov / 2)
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
                transform.eulerAngles = new Vector3(0, 0, currentAngle);
                if (!GameController.imunity && Vector2.Distance(target.position, transform.position) < range && Vector2.Angle(-transform.up, (target.position - transform.position)) < fov / 2)
                {
                    StartCoroutine(Follow());
                    yield break;
                }
            }
            currentAngle = destination;
            transform.eulerAngles = new Vector3(0, 0, destination);
            time = Time.time;
            while (Time.time - time < 2)
            {
                if (!GameController.imunity && Vector2.Distance(target.position, transform.position) < range && Vector2.Angle(-transform.up, (target.position - transform.position)) < fov / 2)
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
        while (!GameController.imunity && Vector2.Distance(target.position, transform.position) < range + 1 && Vector2.Angle(-transform.up, (target.position - transform.position)) < fov / 2)
        {
            transform.up = Vector3.RotateTowards(transform.up, -(target.position - transform.position), Mathf.PI / 2 * Time.deltaTime, 0.0f);
            currentAngle = transform.eulerAngles.z;
            if (currentAngle > 180) currentAngle -= 360;
            float clamped = Mathf.Clamp(currentAngle, -angle / 2, angle / 2);
            transform.eulerAngles = new Vector3(0, 0, clamped);
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
            if (Vector2.Distance(target.position, transform.position) < range + 1 && Vector2.Angle(-transform.up, (target.position - transform.position)) < fov / 2)
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
}

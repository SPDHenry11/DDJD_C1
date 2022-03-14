using System.Collections;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    [SerializeField] private int energyRequired = 1;
    [SerializeField] private float openedY;
    private float closedY;
    private Coroutine co;
    private int energy = 0;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector2(transform.position.x, transform.position.y), new Vector2(transform.position.x, openedY));
    }

    void Awake()
    {
        closedY = transform.position.y;
    }

    public void addEnergy(int value)
    {
        energy += value;
        if (energy == energyRequired)
        {
            if (co != null) StopCoroutine(co);
            co = StartCoroutine(Open());
        }
        else if (energy == energyRequired - 1)
        {
            if (co != null) StopCoroutine(co);
            co = StartCoroutine(Close());
        }
    }

    IEnumerator Open()
    {
        while (transform.position.y < openedY)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + speed * Time.deltaTime);
            yield return null;
        }
        transform.position = new Vector2(transform.position.x, openedY);
    }

    IEnumerator Close()
    {
        while (transform.position.y > closedY)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y - speed * Time.deltaTime);
            yield return null;
        }
        transform.position = new Vector2(transform.position.x, closedY);
    }
}

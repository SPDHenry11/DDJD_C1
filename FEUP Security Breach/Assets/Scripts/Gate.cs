using System.Collections;
using UnityEngine;

/// <summary>
/// Gate full behaviour. Gate only opens if "energy" == "energyRequired".
/// </summary>
public class Gate : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    [SerializeField] private int energyRequired = 1;
    [SerializeField] private float openedY;
    [SerializeField] private bool opened = false;
    private float closedY;
    private Coroutine co;
    private int energy = 0;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector2(transform.position.x, transform.position.y), new Vector2(transform.position.x, transform.position.y + openedY));
    }

    void Awake()
    {
        closedY = transform.position.y;
        if (opened)
        {
            transform.position = new Vector2(transform.position.x, (transform.position.y + openedY));
            energy = energyRequired;
        }
    }

    public void addEnergy(int value)
    {
        energy += value;
        if (energy >= energyRequired)
        {
            if (co != null) StopCoroutine(co);
            co = StartCoroutine(Open());
        }
        else
        {
            if (co != null) StopCoroutine(co);
            co = StartCoroutine(Close());
        }
    }

    IEnumerator Open()
    {
        GetComponent<AudioSource>().Play();
        while (transform.position.y < closedY + openedY)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + speed * Time.deltaTime);
            yield return null;
        }
        transform.position = new Vector2(transform.position.x, closedY + openedY);
        GetComponent<AudioSource>().Stop();
    }

    IEnumerator Close()
    {
        GetComponent<AudioSource>().Play();
        while (transform.position.y > closedY)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y - speed * Time.deltaTime);
            yield return null;
        }
        transform.position = new Vector2(transform.position.x, closedY);
        GetComponent<AudioSource>().Stop();
    }
}

using System.Collections;
using UnityEngine;

/// <summary>
/// Manages the life time of instantiated effects.
/// </summary>
public class LifeTime : MonoBehaviour
{
    [SerializeField] private float time = 2;
    void Start()
    {
        StartCoroutine(Clock());
    }
    IEnumerator Clock(){
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}

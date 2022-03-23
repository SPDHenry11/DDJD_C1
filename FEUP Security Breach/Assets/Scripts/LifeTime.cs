using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

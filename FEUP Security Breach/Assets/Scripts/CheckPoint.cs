using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private Object[] savedObjects;
    [System.Serializable]
    public struct Object
    {
        public Transform reference;
        [HideInInspector] public Vector2 savedPos;
        [HideInInspector] public float savedRot;
    }
    void Awake()
    {
        for(int i=0; i<savedObjects.Length;i++){
            savedObjects[i].savedPos = savedObjects[i].reference.position;
            savedObjects[i].savedRot = savedObjects[i].reference.eulerAngles.z;
        }
    }

    public void Restart(){
        for(int i = 0;i<savedObjects.Length;i++){
            savedObjects[i].reference.position = savedObjects[i].savedPos;
            savedObjects[i].reference.eulerAngles = new Vector3(0,0,savedObjects[i].savedRot);
        }
    }
}

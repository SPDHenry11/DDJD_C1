using UnityEngine;

/// <summary>
/// Manages checkpoints
/// </summary>
public class CheckPoint : MonoBehaviour
{
    [SerializeField] private Object[] savedObjects;
    [SerializeField] private Collider2D[] invalidObjects;

    [System.Serializable]
    public struct Object
    {
        public Transform reference;
        [HideInInspector] public Vector2 savedPos;
        [HideInInspector] public float savedRot;
    }
    void Awake()
    {
        for (int i = 0; i < savedObjects.Length; i++)
        {
            savedObjects[i].savedPos = savedObjects[i].reference.position;
            savedObjects[i].savedRot = savedObjects[i].reference.eulerAngles.z;
        }
    }

    public void Restart()
    {
        if (PortalGun.instantiatedPortals[0] != null) Destroy(PortalGun.instantiatedPortals[0]);
        if (PortalGun.instantiatedPortals[1] != null) Destroy(PortalGun.instantiatedPortals[1]);
        for (int i = 0; i < savedObjects.Length; i++)
        {
            savedObjects[i].reference.position = savedObjects[i].savedPos;
            savedObjects[i].reference.eulerAngles = new Vector3(0, 0, savedObjects[i].savedRot);
        }
        Movement.instance.transform.position = transform.position;
    }

    public void InvalidateObjects()
    {
        if (PortalGun.instantiatedPortals[0] != null) Destroy(PortalGun.instantiatedPortals[0]);
        if (PortalGun.instantiatedPortals[1] != null) Destroy(PortalGun.instantiatedPortals[1]);
        foreach (Collider2D c in invalidObjects)
        {
            c.gameObject.AddComponent<LifeTime>();
            c.enabled = false;
        }

    }
}


using UnityEngine;

public class PortalGun : MonoBehaviour
{
    [SerializeField] private LayerMask portalGunShotLayers;
    [SerializeField] private Transform muzzle;
    [SerializeField] private GameObject[] portals;
    [HideInInspector] public static GameObject[] instantiatedPortals;

    void Awake()
    {
        instantiatedPortals = new GameObject[2];
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(muzzle.position, muzzle.position + muzzle.forward * 3);
    }
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;

        Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        if (instantiatedPortals[0] != null) instantiatedPortals[0].SetActive(false);
        //Orange Portal
        if (Input.GetButtonDown("Fire1"))
        {
            RaycastHit2D hit = Physics2D.Raycast(muzzle.position, muzzle.forward, 30, portalGunShotLayers);
            if (hit.collider != null)
            {
                ValidatePortal(hit.point, -hit.normal, 0);
            }
        }
        if (instantiatedPortals[0] != null) instantiatedPortals[0].SetActive(true);
        if (instantiatedPortals[1] != null) instantiatedPortals[1].SetActive(false);
        if (Input.GetButtonDown("Fire2"))
        {
            RaycastHit2D hit = Physics2D.Raycast(muzzle.position, muzzle.forward, 30, portalGunShotLayers);
            if (hit.collider != null && hit.collider.tag.Equals("PortalWall"))
            {
                ValidatePortal(hit.point, -hit.normal, 1);
            }
        }
        if (instantiatedPortals[1] != null) instantiatedPortals[1].SetActive(true);
    }
    private void ValidatePortal(Vector2 spot, Vector2 direction, int id)
    {
        Vector2 parallel = Vector2.Perpendicular(direction);
        parallel = new Vector2(Mathf.Abs(parallel.x), Mathf.Abs(parallel.y));

        RaycastHit2D boundHit1 = Physics2D.Raycast(spot + parallel - direction, direction, 2, portalGunShotLayers);
        RaycastHit2D boundHit2 = Physics2D.Raycast(spot - parallel - direction, direction, 2, portalGunShotLayers);
        int compensation = 1;
        if (boundHit1.collider == null || !boundHit1.collider.tag.Equals("PortalWall"))
        {
            compensation = -1;
            if (boundHit2.collider == null || !boundHit2.collider.tag.Equals("PortalWall")) return;
        }
        else if (boundHit1.collider != null && boundHit1.collider.tag.Equals("PortalWall") && !Physics2D.OverlapArea(boundHit1.point - direction * 0.1f, boundHit2.point - direction,portalGunShotLayers))
        {
            InstantiatePortal(spot, direction, id);
            return;
        }
        for (int i = 1; i < 10; i++)
        {
            boundHit1 = Physics2D.Raycast(spot + parallel + parallel * Mathf.Sign(compensation) * i / 10 - direction, direction, 2, portalGunShotLayers);
            boundHit2 = Physics2D.Raycast(spot - parallel + parallel * Mathf.Sign(compensation) * i / 10 - direction, direction, 2, portalGunShotLayers);
            if (boundHit1.collider != null && boundHit1.collider.tag.Equals("PortalWall")
            && boundHit2.collider != null && boundHit2.collider.tag.Equals("PortalWall")
            && !Physics2D.OverlapArea(boundHit1.point - direction * 0.1f, boundHit2.point - direction,portalGunShotLayers))
            {
                InstantiatePortal(new Vector2((boundHit1.point.x + boundHit2.point.x) / 2, (boundHit1.point.y + boundHit2.point.y) / 2), direction, id);
                return;
            }
        }
    }

    private void InstantiatePortal(Vector2 spot, Vector2 direction, int id)
    {
        if (instantiatedPortals[id] != null)
        {
            instantiatedPortals[id].transform.position = spot;
            instantiatedPortals[id].transform.rotation = Quaternion.FromToRotation(Vector3.down, direction);
        }
        else instantiatedPortals[id] = Instantiate(portals[id], spot, Quaternion.FromToRotation(Vector3.down, direction));
    }

}

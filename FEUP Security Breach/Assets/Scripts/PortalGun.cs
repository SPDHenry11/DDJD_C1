
using System.Collections;
using UnityEngine;

/// <summary>
/// Manages the portal gun shots and portals
/// </summary>
public class PortalGun : MonoBehaviour
{
    [HideInInspector] public static GameObject[] instantiatedPortals;
    public static bool toggler = true;
    [SerializeField] private float range = 50;
    [SerializeField] private LayerMask portalGunShotLayers;
    [SerializeField] private Transform muzzle;
    [SerializeField] private GameObject[] portals;
    [SerializeField] private GameObject bulletTrail;

    private Coroutine[] coroutines;
    void Awake()
    {
        instantiatedPortals = new GameObject[2];
        coroutines = new Coroutine[2];
        toggler = true;
    }
    void Update()
    {
        if (toggler)
        {
            Vector3 mousePos = Input.mousePosition;

            Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
            mousePos.x = mousePos.x - objectPos.x;
            mousePos.y = mousePos.y - objectPos.y;

            float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            if (instantiatedPortals[0] != null) instantiatedPortals[0].GetComponent<BoxCollider2D>().enabled = false;
            //Orange Portal
            if (Input.GetButtonDown("Fire1"))
            {
                RaycastHit2D hit = Physics2D.Raycast(muzzle.position, muzzle.forward, range, portalGunShotLayers);
                if (hit.collider != null)
                {
                    StartCoroutine(AnimateBulletTrail(hit.point, portals[0].GetComponentInChildren<SpriteRenderer>().color));
                    if (hit.collider.tag.Equals("PortalWall")) ValidatePortal(hit.point, -hit.normal, 0);
                }
                else StartCoroutine(AnimateBulletTrail(muzzle.position + muzzle.forward * range, portals[0].GetComponentInChildren<SpriteRenderer>().color));
            }
            if (instantiatedPortals[0] != null) instantiatedPortals[0].GetComponent<BoxCollider2D>().enabled = true;
            if (instantiatedPortals[1] != null) instantiatedPortals[1].GetComponent<BoxCollider2D>().enabled = false;
            if (Input.GetButtonDown("Fire2"))
            {
                RaycastHit2D hit = Physics2D.Raycast(muzzle.position, muzzle.forward, range, portalGunShotLayers);
                if (hit.collider != null)
                {
                    StartCoroutine(AnimateBulletTrail(hit.point, portals[1].GetComponentInChildren<SpriteRenderer>().color));
                    if (hit.collider.tag.Equals("PortalWall")) ValidatePortal(hit.point, -hit.normal, 1);
                }
                else StartCoroutine(AnimateBulletTrail(muzzle.position + muzzle.forward * range, portals[1].GetComponentInChildren<SpriteRenderer>().color));
            }
            if (instantiatedPortals[1] != null) instantiatedPortals[1].GetComponent<BoxCollider2D>().enabled = true;
        }
    }
    private void ValidatePortal(Vector2 spot, Vector2 direction, int id)
    {
        Vector2 parallel = Vector2.Perpendicular(direction);
        parallel = new Vector2(Mathf.Abs(parallel.x), Mathf.Abs(parallel.y)) * 1.3f;

        RaycastHit2D boundHit1 = Physics2D.Raycast(spot + parallel - direction, direction, 2, portalGunShotLayers);
        RaycastHit2D boundHit2 = Physics2D.Raycast(spot - parallel - direction, direction, 2, portalGunShotLayers);
        int compensation = 1;
        if (boundHit1.collider == null || !boundHit1.collider.tag.Equals("PortalWall"))
        {
            compensation = -1;
            if (boundHit2.collider == null || !boundHit2.collider.tag.Equals("PortalWall")) return;
        }
        else if (boundHit1.collider != null && boundHit1.collider.tag.Equals("PortalWall") && !Physics2D.OverlapArea(boundHit1.point - direction * 0.1f, boundHit2.point - direction, portalGunShotLayers)
            && boundHit2.collider != null && boundHit2.collider.tag.Equals("PortalWall"))
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
            && !Physics2D.OverlapArea(boundHit1.point - direction * 0.1f, boundHit2.point - direction, portalGunShotLayers))
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
        if (coroutines[id] != null) StopCoroutine(coroutines[id]);
        coroutines[id] = StartCoroutine(AnimatePortal(instantiatedPortals[id].transform));
    }

    IEnumerator AnimatePortal(Transform portal)
    {
        portal.gameObject.GetComponentInChildren<ParticleSystem>().Clear();
        float scale = 0;
        float destination = portals[0].transform.GetChild(0).localScale.x;
        Transform target = portal.GetChild(0).transform;
        target.localScale = new Vector3(0, 1, 1);
        while (target.localScale.x < destination * 0.95f)
        {
            scale += (destination - scale) * Time.deltaTime * 6;
            target.localScale = new Vector3(scale, destination, 1);
            yield return null;
        }
        target.localScale = new Vector3(destination, destination, 1);

    }

    IEnumerator AnimateBulletTrail(Vector3 destination, Color color)
    {
        AudioController.instance.Play("PortalGun");
        float maxDistance = Vector2.Distance(destination, muzzle.position);
        Transform effect = Instantiate(bulletTrail, muzzle.position, muzzle.rotation).transform;
        TrailRenderer tr = effect.gameObject.GetComponent<TrailRenderer>();
        tr.startColor = color;
        while (Vector2.Distance(muzzle.position, effect.position) < maxDistance)
        {
            yield return null;
            effect.position += (destination - muzzle.position).normalized * 200 * Time.deltaTime;
        }
        effect.position = destination;
        yield return new WaitForSeconds(0.05f);
        Destroy(effect.gameObject);

    }
}

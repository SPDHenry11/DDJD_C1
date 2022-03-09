using System.Collections;
using UnityEngine;

public class InteractionsController : MonoBehaviour
{
    [HideInInspector] public static Rigidbody2D item;
    [SerializeField] private Transform itemHolder;
    [SerializeField] private LayerMask interactables;
    private interactable currentInteractable;
    private struct interactable
    {
        public GameObject obj;
        public Color color;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(itemHolder.position - itemHolder.forward * 2, itemHolder.position + .5f * itemHolder.forward);
    }

    void Update()
    {
        if (item != null)
        {
            float distance = Vector2.Distance(item.position, itemHolder.position);
            if (distance > 5)
            {
                item.gravityScale = 1;
                item.drag = 0;
                item.gameObject.layer = 9;
                item = null;
            }
            else item.MovePosition(item.position + ((new Vector2(itemHolder.position.x, itemHolder.position.y) - item.position) * 25) * Time.deltaTime);
            if (Input.GetKeyDown(KeyCode.E))
            {
                item.velocity = (new Vector2(itemHolder.position.x, itemHolder.position.y) - item.position) * 5f;
                DropItem();
            }
        }
        else
        {
            RaycastHit2D hit = Physics2D.Raycast(itemHolder.position - itemHolder.forward * 2, itemHolder.forward, 2.5f, interactables);
            if (hit.collider != null)
            {
                if (currentInteractable.obj != null && !GameObject.ReferenceEquals(currentInteractable.obj, hit.collider.gameObject))
                {
                    ResetColor();
                }
                else if (currentInteractable.obj == null)
                {
                    //Selection Effect (gray color)
                    currentInteractable.obj = hit.collider.gameObject;
                    SpriteRenderer spr = hit.collider.gameObject.GetComponent<SpriteRenderer>();
                    currentInteractable.color = spr.color;
                    spr.color = new Color(0.3f, 0.3f, 0.3f);
                }
                if (Input.GetKeyDown(KeyCode.E))
                {
                    switch (hit.collider.tag)
                    {
                        case "Item":
                            GameObject pickup = hit.collider.gameObject;
                            item = pickup.GetComponent<Rigidbody2D>();
                            item.gravityScale = 0;
                            pickup.layer = 10;
                            ResetColor();
                            break;
                        case "Coffee": 
                            break;
                    }
                }
            }
            else if (currentInteractable.obj != null)
            {
                ResetColor();
            }
        }

    }

    private void DropItem()
    {
        item.gravityScale = 1;
        item.drag = 0;
        item.gameObject.layer = 9;
        item = null;
    }

    private void ResetColor()
    {
        currentInteractable.obj.GetComponent<SpriteRenderer>().color = currentInteractable.color;
        currentInteractable.obj = null;
    }
}

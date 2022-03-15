using System.Collections.Generic;
using UnityEngine;

public class InteractionsController : MonoBehaviour
{
    //Items
    [HideInInspector] public static Rigidbody2D item;
    [SerializeField] private Transform itemHolder;
    [SerializeField] private LayerMask itemsRaycast;
    private Item currentItem;
    private struct Item
    {
        public GameObject obj;
        public Color color;
    }
    //Triggers
    private List<GameObject> currentTriggers = new List<GameObject>();

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
                DropItem();
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
            RaycastHit2D hit = Physics2D.Raycast(itemHolder.position - itemHolder.forward * 2, itemHolder.forward, 2.5f, itemsRaycast);
            if (hit.collider != null && hit.collider.gameObject.layer == 11)
            {
                if (currentItem.obj != null && !GameObject.ReferenceEquals(currentItem.obj, hit.collider.gameObject))
                {
                    ResetColor();
                }
                else if (currentItem.obj == null)
                {
                    //Selection Effect - gray(ish) color
                    currentItem.obj = hit.collider.gameObject;
                    SpriteRenderer spr = hit.collider.gameObject.GetComponent<SpriteRenderer>();
                    currentItem.color = spr.color;
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
                        case "Wire":
                            break;
                    }
                }
            }
            else
            {
                if (currentItem.obj != null) ResetColor();
                if (currentTriggers.Count > 0)
                {
                    float closestDistance = 100;
                    int closestIndex = 0;
                    for (int i = 1; i < currentTriggers.Count; i++)
                    {
                        float distance = Vector2.Distance(transform.position, currentTriggers[i].transform.position);
                        if (distance < closestDistance)
                        {
                            closestDistance = distance;
                            closestIndex = i;
                        }
                    }
                    //TODO: Add some sort of highlight on the trigger
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        switch (currentTriggers[closestIndex].tag)
                        {
                            case "CoffeeMachine":
                                if (UIController.instance.PurchaseCoffee())
                                {
                                    //Do Effects
                                    Movement.instance.speed = 10;
                                }
                                break;
                            case "Button":
                                break;
                        }
                    }
                }
            }
        }
    }

    private void DropItem()
    {
        item.gravityScale = 1;
        item.drag = 0;
        item.gameObject.layer = 11;
        item = null;
    }

    private void ResetColor()
    {
        currentItem.obj.GetComponent<SpriteRenderer>().color = currentItem.color;
        currentItem.obj = null;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 9)
        {
            currentTriggers.Add(other.gameObject);
            if (currentItem.obj != null) ResetColor();
        }
        if (other.tag == "Coin")
        {
            UIController.instance.AddCoin();
            Destroy(other.gameObject);
        }
        else if (other.tag == "CheckPoint")
        {
            GameController.instance.SetCheckPoint(other.GetComponent<CheckPoint>());
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == 9)
        {
            currentTriggers.Remove(other.gameObject);
        }
    }
}

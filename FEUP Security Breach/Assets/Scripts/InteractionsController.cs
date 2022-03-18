using System.Collections.Generic;
using UnityEngine;

public class InteractionsController : MonoBehaviour
{
    //Items
    [HideInInspector] public static Rigidbody2D item;
    [SerializeField] private Transform itemHolder;
    [SerializeField] private LayerMask itemsRaycast;
    [SerializeField] private ParticleSystem effect;
    private Item currentItem;
    private struct Item
    {
        public GameObject obj;
        public Color color;
    }
    //Triggers
    private List<GameObject> currentTriggers = new List<GameObject>();
    [SerializeField] private GameObject triggerEffect;
    private Vector2 triggerEffectDefaultSize;
    private int closestTriggerIndex = -1;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(itemHolder.position - itemHolder.forward * 2, itemHolder.position + .5f * itemHolder.forward);
    }

    void Awake()
    {
        triggerEffectDefaultSize = triggerEffect.GetComponent<SpriteRenderer>().bounds.size;
        triggerEffect.transform.SetParent(null);
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
                    if (closestTriggerIndex != closestIndex)
                    {
                        triggerEffect.SetActive(true);
                        Vector2 triggerSize = currentTriggers[closestIndex].GetComponent<SpriteRenderer>().sprite.bounds.size;
                        Vector2 triggerScale = currentTriggers[closestIndex].transform.lossyScale;
                        float xRatio = triggerSize.x / triggerEffectDefaultSize.x;
                        float yRatio = triggerSize.y / triggerEffectDefaultSize.y;
                        triggerEffect.transform.localScale = new Vector3(xRatio * triggerScale.x, yRatio * triggerScale.y, 1);
                        triggerEffect.transform.position = currentTriggers[closestIndex].transform.position;
                        closestTriggerIndex = closestIndex;
                    }
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        switch (currentTriggers[closestIndex].tag)
                        {
                            case "CoffeeMachine":
                                if (UIController.instance.PurchaseCoffee())
                                {
                                    AudioController.instance.Play("Good");
                                    effect.Play();
                                    currentTriggers[closestIndex].GetComponent<Collider2D>().enabled = false;
                                    Movement.instance.speed = 10;
                                }
                                break;
                        }
                    }
                }
                else if (closestTriggerIndex != -1)
                {
                    triggerEffect.SetActive(false);
                    closestTriggerIndex = -1;
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
        switch (other.tag)
        {
            case "Coin":
                AudioController.instance.Play("Coin");
                UIController.instance.AddCoin();
                Destroy(other.gameObject);
                break;
            case "CheckPoint":
                GameController.instance.SetCheckPoint(other.GetComponent<CheckPoint>());
                break;
            case "EndGame":
                if (!GameController.imunity) UIController.instance.End();
                break;
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

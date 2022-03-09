using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinCollect : MonoBehaviour
{
    private float coin = 0;
    public TextMeshProUGUI textCoins;
    private void OnTriggerEnter2D(Collider2D collider2D) {
        if(collider2D.transform.tag == "Coin") {
            coin ++;
            textCoins.text = coin.ToString();
            
            Destroy(collider2D.gameObject);
        }
    }
}

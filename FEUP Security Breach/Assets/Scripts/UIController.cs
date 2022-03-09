using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    public static UIController instance;
    [SerializeField] private TextMeshProUGUI textCoins;
    private int coins = 0;
    void Awake()
    {
        instance = this;
    }

    public void AddCoin()
    {
        coins++;
        textCoins.text = "Coins " + coins.ToString();
    }

    public bool PurchaseCoffee(){
        if(coins>=5) {
            coins-=5;
            textCoins.text = "Coins " + coins.ToString();
            return true;
        }
        return false;
    }
}

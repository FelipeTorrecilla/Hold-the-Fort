using UnityEngine;
using UnityEngine.UI;

public class CurrencyDisplay : MonoBehaviour
{
    private CurrencyManager currencyManager;
    private Text currencyText;

    private void Start()
    {
        currencyManager = FindObjectOfType<CurrencyManager>();
        currencyText = GetComponent<Text>();
    }

    private void Update()
    {
        // Update the displayed currency value
        currencyText.text = "$ " + currencyManager.CurrentCurrency;
    }
}
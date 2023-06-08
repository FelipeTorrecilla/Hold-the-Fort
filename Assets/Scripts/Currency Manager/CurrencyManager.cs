using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    private int currentCurrency = 0;

    public int CurrentCurrency
    {
        get { return currentCurrency; }
    }

    public void AddCurrency(int amount)
    {
        currentCurrency += amount;
    }

    public void RemoveCurrency(int amount)
    {
        currentCurrency -= amount;

        // Ensure the currency doesn't go below zero
        currentCurrency = Mathf.Max(currentCurrency, 0);
    }
}


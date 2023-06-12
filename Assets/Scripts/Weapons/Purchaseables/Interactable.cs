using Code.Weapons;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public int refillCost = 10;  // Currency cost to refill ammo
    public WeaponController weaponController;  // Reference to the player's weapon controller

    private CurrencyManager currencyManager;
    [SerializeField] private bool isDoor;
    private bool doorOpened = false;

    private void Start()
    {
        currencyManager = FindObjectOfType<CurrencyManager>();
    }

    public void Interact()
    {
        if (isDoor && !doorOpened)
        {
            // Check if the player has enough currency
            if (currencyManager.CurrentCurrency >= refillCost)
            {
                // Deduct the refill cost from the player's currency
                currencyManager.RemoveCurrency(refillCost);

                // Open the door (play animation, particles, etc.)
                OpenDoor();
            }
            else
            {
                Debug.Log("Not enough currency to open the door!");
            }
        }
        else
        {
            // Perform the default interaction (refilling ammo)
            RefillAmmo();
        }
    }

    private void OpenDoor()
    {
        // Play animation, particles, or any other visual effects for opening the door

        // Disable this object (door)
        gameObject.SetActive(false);

        // Set the doorOpened flag to true to prevent further interactions
        doorOpened = true;
    }

    private void RefillAmmo()
    {
        // Check if the player has enough currency
        if (currencyManager.CurrentCurrency >= refillCost)
        {
            // Deduct the refill cost from the player's currency
            currencyManager.RemoveCurrency(refillCost);

            // Refill the player's weapon ammo
            weaponController.RefillAmmo();
        }
        else
        {
            Debug.Log("Not enough currency to refill ammo!");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBuy : MonoBehaviour, IInteractable
{
    [SerializeField] private int openCost = 10;
    private CurrencyManager currencyManager;
    private bool doorOpened = false;
    
    [SerializeField] private GameObject[] spawnPointsToActivate;

    [SerializeField] private AudioClip _purchaseSound;
    private AudioSource _audioSource;
    private BoxCollider2D doorCollider;

    
    private void Start()
    {
        currencyManager = FindObjectOfType<CurrencyManager>();
        _audioSource = GetComponent<AudioSource>();
        doorCollider = GetComponent<BoxCollider2D>();
    }
    
    public void Interact()
    {
        if (!doorOpened)
        {
            // Check if the player has enough currency
            if (currencyManager.CurrentCurrency >= openCost)
            {
                // Deduct the refill cost from the player's currency
                currencyManager.RemoveCurrency(openCost);
                
                OpenDoor();
                _audioSource.PlayOneShot(_purchaseSound);
                ActivateSpawnPoints();
            }
            else
            {
                Debug.Log("Not enough currency to open the door!");
            }
        }
    }

    private void OpenDoor()
    {
        // Play animation, particles, or any other visual effects for opening the door
        
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        doorCollider.enabled = false;
        doorOpened = true;
    }
    
    public void ActivateSpawnPoints()
    {
        foreach (GameObject obj in spawnPointsToActivate)
        {
            obj.SetActive(true);
        }
    }
}

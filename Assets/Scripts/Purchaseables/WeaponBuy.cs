using System.Collections;
using System.Collections.Generic;
using Code.Weapons;
using UnityEngine;

public class WeaponBuy : MonoBehaviour, IInteractable
{
   [SerializeField] private int refillCost = 10;  // Currency cost to refill ammo
   public WeaponController weaponController;  // Reference to the player's weapon controller

   private CurrencyManager currencyManager;
   
   [SerializeField] private AudioClip _purchaseSound;
   private AudioSource _audioSource;
   
   private void Start()
   {
      _audioSource = GetComponent<AudioSource>();
      currencyManager = FindObjectOfType<CurrencyManager>();
   }
   public void Interact()
   {
      // Check if the player has enough currency
      if (currencyManager.CurrentCurrency >= refillCost)
      {
         RefillAmmo();
         _audioSource.PlayOneShot(_purchaseSound);
      }
      else
      {
         Debug.Log("Not enough currency to refill ammo!");
      }
   }
   
   private void RefillAmmo()
   {
      // Deduct the refill cost from the player's currency
      currencyManager.RemoveCurrency(refillCost);
      
      // Refill the player's weapon ammo
      weaponController.RefillAmmo();
   }
}

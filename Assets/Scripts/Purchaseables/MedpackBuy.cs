using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedpackBuy : MonoBehaviour, IInteractable
{
   [SerializeField] private int healCost = 10;
   [SerializeField] private int healAmount;
   public CharacterHealth characterHealth;

   private CurrencyManager currencyManager;
   
   private void Start()
   {
      currencyManager = FindObjectOfType<CurrencyManager>();
   }
   public void Interact()
   {
      // Check if the player has enough currency
      if (currencyManager.CurrentCurrency >= healCost)
      {
         Heal();
      }
      else
      {
         Debug.Log("Not enough currency to heal player!");
      }
   }
   
   private void Heal()
   {
      currencyManager.RemoveCurrency(healCost);
      
      characterHealth.Heal(healAmount);
   }
}

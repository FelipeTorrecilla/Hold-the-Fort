using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buckshot : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private int damageAmount = 25;
    [SerializeField] private float destroyDelay = 0.5f; // Delay before the bullet is destroyed
    
    private float timer; // Timer to track the bullet's lifetime

    private void Awake()
    {
        gameObject.GetComponent<Rigidbody2D>().velocity = transform.right * _speed;
    }
    
    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= destroyDelay)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       ZombieAI zombie = collision.GetComponent<ZombieAI>();

        if (zombie != null)
        {
            zombie.TakeDamage(damageAmount);
        }
    }
    
    public void DestroyObject()
    {
        Destroy(gameObject);
    }
    
}
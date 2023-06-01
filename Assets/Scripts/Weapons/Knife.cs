using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Knife : MonoBehaviour
{
    [SerializeField] private Text _ammoText; // UI text component to display ammo count
    
    public int damage = 10;
    public float stabDistance = 1.5f;
    public float stabDuration = 0.2f;
    public float damageDuration = 0.1f;

    private bool isStabbing = false;

    private void OnEnable()
    {
        _ammoText.text = "-/-";
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isStabbing)
        {
            StartCoroutine(PerformStab());
        }
    }

    private System.Collections.IEnumerator PerformStab()
    {
        isStabbing = true;
        float stabTimer = 0f;
        float stabSpeed = stabDistance / stabDuration;

        while (stabTimer < stabDuration)
        {
            float stabAmount = stabSpeed * Time.deltaTime;
            transform.Translate(Vector3.up * stabAmount);
            stabTimer += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(damageDuration);

        while (stabTimer > 0f)
        {
            float retractAmount = stabSpeed * Time.deltaTime;
            transform.Translate(Vector3.down * retractAmount);
            stabTimer -= Time.deltaTime;
            yield return null;
        }

        isStabbing = false;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!isStabbing)
            return;

        ZombieAI zombie = other.GetComponent<ZombieAI>();
        if (zombie != null)
        {
            zombie.TakeDamage(damage);
        }
    }
}
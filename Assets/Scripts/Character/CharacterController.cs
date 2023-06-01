using System.Collections;
using System.Collections.Generic;
using Code.Weapons;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CharacterController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float fadeSpeed = 5f;

    //public Transform reticle; // Reference to the aiming reticle object
    //public float maxReticleSize = 5f; // Maximum size of the reticle when it's farthest away
    
    public List<GameObject> weapons; // List of weapon prefabs
    public int currentWeaponIndex = 0; // Index of the currently selected weapon

    private Rigidbody2D rb;
    private Vector2 movement;
    private bool isMoving = false;
  private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // Disable all weapons except the currently selected one
        for (int i = 0; i < weapons.Count; i++)
        {
            if (i != currentWeaponIndex)
                weapons[i].SetActive(false);
        }
    }
    
    private void Update()
    {
        Movement();
        WeaponSelection();
    }

    private void Movement()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        movement = new Vector2(moveX, moveY);
        movement.Normalize();

        if (movement.magnitude > 0)
        {
            isMoving = true;
            rb.velocity = movement * moveSpeed;
        }
        else if (isMoving)
        {
            isMoving = false;
            StartCoroutine(FadeVelocity());
        }
        
        // Character Aiming
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 aimDirection = mousePosition - transform.position;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private System.Collections.IEnumerator FadeVelocity()
    {
        while (rb.velocity.magnitude > 0.01f)
        {
            rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, fadeSpeed * Time.deltaTime);
            yield return null;
        }

        rb.velocity = Vector2.zero;
    }

    private void WeaponSelection()
    {
        // Weapon Selection
        if (Input.mouseScrollDelta.y > 0f)
            SelectNextWeapon();

        else if (Input.mouseScrollDelta.y < 0f)
            SelectPreviousWeapon();

        for (int i = 0; i < weapons.Count; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                SelectWeapon(i);
        }
    }
    
    private void SelectWeapon(int index)
    {
        if (index == currentWeaponIndex)
            return;

        weapons[currentWeaponIndex].SetActive(false); // Disable the current weapon
        currentWeaponIndex = index;
        weapons[currentWeaponIndex].SetActive(true); // Enable the newly selected weapon
    }

    private void SelectNextWeapon()
    {
        int nextIndex = (currentWeaponIndex + 1) % weapons.Count;
        SelectWeapon(nextIndex);
    }

    private void SelectPreviousWeapon()
    {
        int previousIndex = currentWeaponIndex - 1;
        if (previousIndex < 0)
            previousIndex = weapons.Count - 1;
        SelectWeapon(previousIndex);
    }
}


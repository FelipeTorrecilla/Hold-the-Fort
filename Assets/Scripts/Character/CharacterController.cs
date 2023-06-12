using System.Collections.Generic;
using UnityEngine;


public class CharacterController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float fadeSpeed = 5f;
    public float dashDistance = 5f;
    public float dashCooldown = 2f;

    public List<GameObject> weapons; // List of weapon prefabs
    public int currentWeaponIndex = 0; // Index of the currently selected weapon

    private Rigidbody2D rb;
    private Vector2 movement;
    private bool isMoving = false;
    private Transform playerTransform;
    private bool isDashing = false;
    private bool isDashCooldown = false;
    public ParticleSystem dashParticles; // Reference to the Particle System GameObject

    
    public float interactDistance = 2f;
    private Interactable interactable;
    
    public bool _weaponReloading = false;
    
    public CameraBehaviour cameraBehaviour;
    
  private void Awake()
  {
        playerTransform = transform;
        rb = GetComponent<Rigidbody2D>();
        // Disable all weapons except the currently selected one
        for (int i = 0; i < weapons.Count; i++)
        {
            if (i != currentWeaponIndex)
                weapons[i].SetActive(false);
        }
        cameraBehaviour = Camera.main.GetComponent<CameraBehaviour>();
        dashParticles.Stop(); // Ensure the particle system is initially stopped
       
  }
    
    private void Update()
    {
        Movement();
        WeaponSelection();
        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
        
        if (!isDashing && !isDashCooldown && Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Dash());
        }
    }

    private void Movement()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        movement = new Vector2(moveX, moveY);
        movement.Normalize();

        if (isDashing)
        {
            rb.velocity = movement * moveSpeed * 2f;
        }
        else if (movement.magnitude > 0)
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
    
    private System.Collections.IEnumerator Dash()
    {
        cameraBehaviour.ShakeCamera(0.2f, 5f);
        isDashing = true;
        dashParticles.Play();

        Vector2 dashDirection = movement.normalized;
        Vector2 dashTarget = rb.position + dashDirection * dashDistance;

        rb.position = dashTarget;
        isDashing = false;
        isDashCooldown = true;

        yield return new WaitForSeconds(dashCooldown);

        isDashCooldown = false;
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
    
    public void SetWeaponReloading(bool reloading)
    {
        _weaponReloading = reloading;
    }

    private void WeaponSelection()
    {
        if (!_weaponReloading)
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
    
    private void Interact()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(playerTransform.position, interactDistance);

        foreach (Collider2D collider in colliders)
        {
            Interactable interactable = collider.GetComponent<Interactable>();

            if (interactable != null)
            {
                interactable.Interact();
            }
        }
    }
}


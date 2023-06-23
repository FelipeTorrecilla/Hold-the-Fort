using System;
using System.Collections;
using System.Collections.Generic;
using Code.Weapons;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Code.Weapons
{
    public class WeaponController : MonoBehaviour
    {
        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private Transform _spawnReference;
        [SerializeField] private int _maxAmmo = 10; // Maximum ammo count
        [SerializeField] private int _maxAmmoInMagazine = 10; // Maximum ammo count in each magazine
        [SerializeField] private float _reloadTime = 2f; // Time it takes to reload
        [SerializeField] private Text _ammoText; // UI text component to display ammo count

        private int _currentAmmo; // Current ammo count
        private int _currentAmmoInMagazine; // Current ammo count in the magazine
        private bool _reloading; // Flag indicating whether the pistol is reloading

        [SerializeField] private float FireCooldown;
        [SerializeField] private bool Automatic;
        private float CurrentCooldown;

        public Transform reticle; // Reference to the aiming reticle object
        public float maxReticleSize = 5f; // Maximum size of the reticle when it's farthest away
        
        private CharacterController _characterController;
        
        [SerializeField] private AudioClip _shootingSound; // Sound effect for shooting
        [SerializeField] private AudioClip _reloadingSound; // Sound effect for reloading
        private AudioSource _audioSource; // Reference to the Audio Source component

        private void Awake()
        {
            _currentAmmo = _maxAmmo;
            _currentAmmoInMagazine = _maxAmmoInMagazine;
            CurrentCooldown = FireCooldown;
            _characterController = FindObjectOfType<CharacterController>();
            if (_characterController == null)
            {
                Debug.LogError("CharacterController component not found in the scene.");
            }
            
            _audioSource = GetComponent<AudioSource>();
        }
        private void OnEnable()
        {
            UpdateAmmoText();
        }

        private void Update()
        {
            Attack();

            // Character Aiming
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Aiming Reticle
            mousePosition.z = -1f; // Set the desired Z position of the reticle
            reticle.position = mousePosition;
            float distance = Vector2.Distance(transform.position, mousePosition);
            float reticleScale = Mathf.Clamp(distance, 0f, maxReticleSize) / maxReticleSize;
            reticle.localScale = Vector3.one * reticleScale;
            
            if (Input.GetKeyDown(KeyCode.R))
            {
                Reload();
            }
        }

        public void Attack()
        {
            if (_reloading)
                return;

            if (Automatic)
            {
                if (Input.GetMouseButton(0))
                {
                    if (CurrentCooldown <= 0f && _currentAmmoInMagazine > 0)
                    {
                        var bullet = Instantiate(_bulletPrefab, _spawnReference.position, _spawnReference.rotation);
                        CurrentCooldown = FireCooldown;
                        _currentAmmoInMagazine--;
                        UpdateAmmoText();
                        
                         if (_shootingSound != null && _audioSource != null)
                         {
                            _audioSource.PlayOneShot(_shootingSound);
                         }
                    }
                    if (_currentAmmoInMagazine <= 0 && _currentAmmo > 0)
                    {
                        Reload();
                    }
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (CurrentCooldown <= 0f && _currentAmmoInMagazine > 0)
                    {
                        var bullet = Instantiate(_bulletPrefab, _spawnReference.position, _spawnReference.rotation);
                        CurrentCooldown = FireCooldown;
                        _currentAmmoInMagazine--;
                        UpdateAmmoText();
                        
                        if (_shootingSound != null && _audioSource != null)
                        {
                            _audioSource.PlayOneShot(_shootingSound);
                        }
                    }

                    if (_currentAmmoInMagazine <= 0 && _currentAmmo > 0)
                    {
                        Reload();
                    }
                }
            }

            CurrentCooldown -= Time.deltaTime;
        }

        public void UpdateAmmoText()
        {
            if (_ammoText != null)
                _ammoText.text = _currentAmmoInMagazine.ToString() + " / " + _currentAmmo.ToString();
        }

        public void Reload()
        {
            if (_reloadingSound != null && _audioSource != null)
            {
                _audioSource.PlayOneShot(_reloadingSound);
            }
            
            if (!_reloading && _currentAmmoInMagazine < _maxAmmoInMagazine && _currentAmmo > 0)
            {
                StartCoroutine(ReloadCoroutine());
            }   
            
        }

        private IEnumerator ReloadCoroutine()
        {
            _reloading = true;
            _characterController.SetWeaponReloading(true);

            int ammoNeeded = _maxAmmoInMagazine - _currentAmmoInMagazine;
            int ammoAvailable = Mathf.Min(ammoNeeded, _currentAmmo);
            yield return new WaitForSeconds(_reloadTime);
            _currentAmmoInMagazine += ammoAvailable;
            _currentAmmo -= ammoAvailable;
            UpdateAmmoText();

            _reloading = false;
            _characterController.SetWeaponReloading(false);
            
        }
        
        public void RefillAmmo()
        {
            int ammoToAdd = _maxAmmo - _currentAmmo;
            _currentAmmo += ammoToAdd;
            UpdateAmmoText();
        }
    }
}
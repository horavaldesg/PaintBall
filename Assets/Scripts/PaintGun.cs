using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintGun : MonoBehaviour
{
   [SerializeField] private GameObject projectile;
   public static event Action<float,float> GunShot;
   [SerializeField] private float damage;
   public int ammo;
   public int shootSpeed;
   [HideInInspector] public int currentAmmo;
   private PlayerInput _playerInput;
   [SerializeField] private Transform projectileSpawn;
   private bool _canShoot;
   

   private void Awake()
   {
      _playerInput = new PlayerInput();
      currentAmmo = ammo;
      _canShoot = true;
      _playerInput.Player.Fire.performed += tgb => Shoot();
      _playerInput.Player.Reload.performed += tgb => Reload();
      UpdateUI();
   }

   private void OnEnable()
   {
      _playerInput.Enable();
   }

   private void OnDisable()
   {
      _playerInput.Disable();
   }

   private void UpdateUI()
   {
      GunShot?.Invoke(currentAmmo, ammo);
   }

   private void Shoot()
   {
      if(currentAmmo <= 0) return;
      if(!_canShoot) return;
      ShootProjectile();
   }


   private void ShootProjectile()
   {
      var go = Instantiate(projectile);
      go.transform.position = projectileSpawn.position;
      go.TryGetComponent(out Projectile projectileComp);
      go.TryGetComponent(out Rigidbody rb);
      var velocity = projectileSpawn.forward * shootSpeed * 1000 * Time.deltaTime;
      rb.AddForce(velocity);
      projectileComp.Damage = damage;
      currentAmmo--;
      UpdateUI();
   }

   private void Reload()
   {
      _canShoot = false;
      StartCoroutine(ReloadTimeout());
   }

   private IEnumerator ReloadTimeout()
   {
      yield return new WaitForSeconds(1);
      _canShoot = true;
      currentAmmo = ammo;
      UpdateUI();
   }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintGun : MonoBehaviour
{
   [SerializeField] private GameObject projectile;
   
   [SerializeField] private float damage;
   public int ammo;
   public int shootSpeed;
   [HideInInspector] public int currentAmmo;
   private PlayerInput _playerInput;
   [SerializeField] private Transform projectileSpawn;
   

   private void Awake()
   {
      _playerInput = new PlayerInput();
      
      _playerInput.Player.Fire.performed += tgb => Shoot();
   }

   private void OnEnable()
   {
      _playerInput.Enable();
   }

   private void OnDisable()
   {
      _playerInput.Disable();
   }

   private void Shoot()
   {
      if(ammo <= 0) return;
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

   }
}

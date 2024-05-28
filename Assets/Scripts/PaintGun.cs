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
   [SerializeField] private GameObject mag;
   [SerializeField] private GameObject staticMag;
   [SerializeField] private GameObject throwAwayMag;
   private bool _canShoot;
   private const string ShootAnim = "GunShoot";
   private const string ReloadAnim = "Reload";
   private const string UnloadAnim = "Unload";
   private Animator _animator;
   private static readonly int GunShoot = Animator.StringToHash(ShootAnim);
   private static readonly int Reload1 = Animator.StringToHash(ReloadAnim);
   private static readonly int Unload = Animator.StringToHash(UnloadAnim);

   private void Awake()
   {
      mag.SetActive(false);
      TryGetComponent(out _animator);
      _playerInput = new PlayerInput();
      currentAmmo = ammo;
      _canShoot = true;
      _playerInput.Player.Fire.performed += tgb => Shoot();
      _playerInput.Player.Fire.canceled += tgb => StopShoot();
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

   private void StopShoot()
   {
      _animator.SetBool(GunShoot, false);

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
      _animator.SetBool(GunShoot, true);
      UpdateUI();
   }

   private void Reload()
   {
      _canShoot = false;
      StartCoroutine(ReloadTimeout());
   }

   public void ShowMag()
   {
      mag.SetActive(true);
   }

   public void HideMag()
   {
      mag.SetActive(false);
      staticMag.SetActive(true);
   }

   public void UnloadMag()
   {
      staticMag.SetActive(false);
      var go = Instantiate(throwAwayMag);
      go.transform.position = staticMag.transform.position;
      Destroy(go, 2.5f);
      
   }

   private IEnumerator ReloadTimeout()
   {
      _animator.SetBool(Unload, true);

      yield return new WaitForSeconds(3.5f);
      _animator.SetBool(Unload, false);
      _animator.SetBool(Reload1, true);
      yield return new WaitForSeconds(6);
      _animator.SetBool(Reload1, false);

      _canShoot = true;
      currentAmmo = ammo;
      UpdateUI();
   }
}

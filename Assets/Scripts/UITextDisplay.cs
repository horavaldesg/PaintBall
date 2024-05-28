using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UITextDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ammoCount;
    
    private void OnEnable()
    {
        PaintGun.GunShot += UpdateAmmoCount;
    }
    
    private void OnDisable()
    {
        PaintGun.GunShot -= UpdateAmmoCount;
    }

    private void UpdateAmmoCount(float ammoCount, float maxAmmo)
    {
        var ammoText = ammoCount + "/" + maxAmmo;
        
        this.ammoCount.SetText(ammoText);
    }
    
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempEnemy : PlayerStats
{
    [SerializeField] private GameObject splashText;
    [SerializeField] private Transform splashTextPos;
    
    public override void TakeDamage(float damageToTake)
    {
        base.TakeDamage(damageToTake);
        var material = GetComponent<MeshRenderer>();
        material.material.color = Color.red;
        var splashInstance = Instantiate(splashText);
        splashInstance.transform.position = splashTextPos.position;
        splashInstance.TryGetComponent(out SplashText splashTextComp);
        splashTextComp.SetText(damageToTake);

    }
}

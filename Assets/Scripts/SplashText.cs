using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class SplashText : MonoBehaviour
{
    private TextMeshPro _textMeshProUGUI;
    private Rigidbody _rb;
    
    
    private void Awake()
    {
        TryGetComponent(out _textMeshProUGUI);
        TryGetComponent(out _rb);
        var text = _textMeshProUGUI.text;
        Debug.Log(text);
    }

    public void SetText(float textToDisplay)
    {
        _textMeshProUGUI.SetText(textToDisplay.ToString("-" + "##"));
        AddForce();
    }

    private void AddForce()
    {
        int[] Angle = {-45,45};   // left hand rule!

        var direction = Random.Range(0, Angle.Length);
        var Rotation = Quaternion.Euler( 0, 0, Angle[direction]); 
        var force = Rotation * Vector2.up * 250 * Time.deltaTime;
        _rb.AddForce(force, ForceMode.Impulse);
        
        Destroy(gameObject,3);
    }
}

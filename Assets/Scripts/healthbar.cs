using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class healthbar : MonoBehaviour
{
    public Slider healthSlider;
    public Slider easeHealthSlider;
    public float maxHealth = 100f;
    private float health;
    public float lerpSpeed = 0.05f;

    void Start()
    {
        health = maxHealth; // Initialize health to maximum at the start
    }

    // Update is called once per frame
    void Update()
    {
        if (healthSlider != null) // Check if the health slider reference is not null
        {
            healthSlider.value = health; // Update the slider value based on current health
        }
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame) // Check if the space key is pressed
        {
            TakeDamage(10f); // Simulate taking damage by reducing health by 10
        }
        if (healthSlider != null)
        {
            easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, health, lerpSpeed); // Smoothly interpolate the health slider value to the current health
        }
    }

    void TakeDamage(float damage)
    {
        health -= damage; 
    }
}

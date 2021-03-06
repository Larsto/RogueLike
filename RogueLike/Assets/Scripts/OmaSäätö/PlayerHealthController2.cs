﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController2 : MonoBehaviour
{
    public static PlayerHealthController2 instance;

    public int currentHealth;
    public int maxHealth;

    public float damageInvincLength = 1f;
    private float invincCount;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;

        UIController.instance.healthSlider.maxValue = maxHealth;
        UIController.instance.healthSlider.value = currentHealth;
        UIController.instance.healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (invincCount > 0)
        {
            invincCount -= Time.deltaTime;

            if (invincCount <= 0)
            {
                PlayerController2.instance.bodySR.color = new Color(PlayerController2.instance.bodySR.color.r, PlayerController2.instance.bodySR.color.g, PlayerController2.instance.bodySR.color.b, 1f);
            }
        }
    }

    public void DamagePlayer()
    {
        if (invincCount <= 0)
        {
            currentHealth--;

            invincCount = damageInvincLength;
            AudioManager.instance.PlaySFX(11);

            PlayerController2.instance.bodySR.color = new Color(PlayerController2.instance.bodySR.color.r, PlayerController2.instance.bodySR.color.g, PlayerController2.instance.bodySR.color.b, 0.5f);
            if (currentHealth <= 0)
            {
                PlayerController2.instance.gameObject.SetActive(false);

                UIController.instance.deathScreen.SetActive(true);

                AudioManager.instance.PlayGameOver();
                AudioManager.instance.PlaySFX(9);
            }

            UIController.instance.healthSlider.value = currentHealth;
            UIController.instance.healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
        }

    }
    public void MakeInvincible(float length)
    {
        invincCount = length;
        PlayerController2.instance.bodySR.color = new Color(PlayerController2.instance.bodySR.color.r, PlayerController2.instance.bodySR.color.g, PlayerController2.instance.bodySR.color.b, 0.5f);
    }

    public void HealPlayer(int healAmount)
    {
        currentHealth += healAmount;
        AudioManager.instance.PlaySFX(7);
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        UIController.instance.healthSlider.value = currentHealth;
        UIController.instance.healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
    }
}
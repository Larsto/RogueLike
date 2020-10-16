using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup2 : MonoBehaviour
{
    public int healAmount = 1;

    public float waitToBeCollected = .5f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (waitToBeCollected > 0)
        {
            waitToBeCollected -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && waitToBeCollected <= 0 && PlayerHealthController2.instance.currentHealth < PlayerHealthController2.instance.maxHealth)
        {
            PlayerHealthController2.instance.HealPlayer(healAmount);
            AudioManager.instance.PlaySFX(8);

            Destroy(gameObject);
        }
    }
}

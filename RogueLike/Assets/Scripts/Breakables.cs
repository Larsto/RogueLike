using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakables : MonoBehaviour
{
    public GameObject[] brokenPiece;
    public int maxPieces = 5;

    public bool shouldDropItem;
    public GameObject[] itemToDrop;
    public float itemDropPercent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Smash()
    {
        Destroy(gameObject);

        AudioManager.instance.PlaySFX(0);
        // show broken pieces
        int piecesToDrop = Random.Range(2, maxPieces);

        for (int i = 0; i < piecesToDrop; i++)
        {

            int randomPiece = Random.Range(0, brokenPiece.Length);

            Instantiate(brokenPiece[randomPiece], transform.position, transform.rotation);
        }
        // drop item
        if (shouldDropItem)
        {
            float dropChance = Random.Range(0f, 100f);
            if (dropChance < itemDropPercent)
            {
                int randomItem = Random.Range(0, itemToDrop.Length);
                Instantiate(itemToDrop[randomItem], transform.position, transform.rotation);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            if(PlayerController.instance.dashCounter > 0)
            {
                Smash();
            }
            
        }
        if(other.tag == "PlayerBullet")
        {
            Smash();
        }
    }
}

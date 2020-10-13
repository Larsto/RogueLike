using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenPieces : MonoBehaviour
{
    public float moveSpeed = 3f;
    private Vector3 moveDierction;

    public float deceleration = 5f;

    public float lifeTime = 3f;

    public SpriteRenderer theSR;
    public float fadeSpeed = 2.5f;


    // Start is called before the first frame update
    void Start()
    {
        moveDierction.x = Random.Range(-moveSpeed, moveSpeed);
        moveDierction.y = Random.Range(-moveSpeed, moveSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += moveDierction * Time.deltaTime;

        moveDierction = Vector3.Lerp(moveDierction, Vector3.zero, deceleration * Time.deltaTime);

        lifeTime -= Time.deltaTime;

        if(lifeTime < 0)
        {
            theSR.color = new Color(theSR.color.r, theSR.color.g, theSR.color.b, Mathf.MoveTowards(theSR.color.a, 0f, fadeSpeed * Time.deltaTime));
            if(theSR.color.a == 0)
            {
                Destroy(gameObject);
            }
            
        }
    }   
}

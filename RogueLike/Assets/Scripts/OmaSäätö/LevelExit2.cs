using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelExit2 : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(LevelManager2.instance.LevelEnd());
        }
    }
}

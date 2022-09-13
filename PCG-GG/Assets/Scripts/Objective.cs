using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.name.Equals("Player"))
        {
            collision.gameObject.GetComponent<PlayerMovement>().numberOfJumps--;
            gameObject.SetActive(false);
        }
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class HealthPickup : MonoBehaviour
{
    //healing amount
    public float healAmount = 25f;

    private void OnTriggerEnter(Collider other)
    {
        //check if player touches
        if(other.CompareTag("Player"))
        {
            playerMovement playerMovement = other.GetComponent<playerMovement>();

            if (playerMovement != null)
            {
                playerMovement.Heal(healAmount);
                Destroy(gameObject);
            }
        }
    }
}

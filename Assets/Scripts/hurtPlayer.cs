using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

public class hurtPlayer : MonoBehaviour
{
    public float damageAmount = 50;

    private void OnTriggerEnter(Collider other)
    {
        //check if player touches
        if (other.CompareTag("Player"))
        {
            playerMovement playerMovement = other.GetComponent<playerMovement>();

            if (playerMovement != null)
            {
                playerMovement.Damage(damageAmount);
            }
        }
    }
}

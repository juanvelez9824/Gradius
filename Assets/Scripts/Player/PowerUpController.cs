using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum PowerUpType { SpeedBoost, SpreadShot, Shield }

public class PowerUpController : MonoBehaviour
{
    [SerializeField] private PowerUpType powerUpType;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.ApplyPowerUp(powerUpType);
                Destroy(gameObject);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerCrystal : MonoBehaviour
{
    [SerializeField]
    private int chargeAmount = 1;
    [SerializeField]
    private float chargeSpeedSeconds = 1f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Power Crystal: Trigger");
        // Find the GameSession script and start recharge.
        FindObjectOfType<GameSession>().StartRecharge(chargeAmount, chargeSpeedSeconds);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Power Crystal: Trigger Exit");
        // Find the GameSession script and stop recharge.
        FindObjectOfType<GameSession>().StopRecharge();
    }
}

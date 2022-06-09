using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerState : ScriptableObject
{
    public int playerEnergy;

    public int playerMemories;

    public void Reset()
    {
        playerEnergy = 100;
        playerMemories = 0;
    }

    public void SetPlayerState(int energy, int memories)
    {
        playerEnergy = energy;
        playerMemories = memories;
    }
}

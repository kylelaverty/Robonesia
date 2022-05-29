using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ConversationHistory : ScriptableObject
{
    public bool metScientist;

    public bool metAdventurer;

    public bool metReaper;

    public void Reset()
    {
        metScientist = false;
        metAdventurer = false;
        metReaper = false;
    }
}

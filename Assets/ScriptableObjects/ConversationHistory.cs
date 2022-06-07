using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ConversationHistory : ScriptableObject
{
    public bool metScientist;

    public bool metAdventurer;

    public bool metReaper;

    public bool metSmith;

    public void Reset()
    {
        metScientist = false;
        metAdventurer = false;
        metReaper = false;
        metSmith = false;
    }

    public void SetConversationState(bool scientist, bool adventurer, bool reaper, bool smith)
    {
        metScientist = scientist;
        metAdventurer = adventurer;
        metReaper = reaper;
        metSmith = smith;
    }
}

using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Resource", menuName = "AmethystObjects/Resource")]
public class Resource : ScriptableObject
{
    public string title;
    public int min;
    public int max;
    public int value;

    public int delta;

    public string complaint;

    public Quest[] tresholds;

    public int currentTreshold;

    public string[] thoughtProcess;
}

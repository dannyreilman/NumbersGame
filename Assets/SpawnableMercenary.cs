using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewMerc", menuName = "ScriptObjects/SpawnableMercenary", order = 1)]
public class SpawnableMercenary : ScriptableObject
{
    public Mercenary merc;
    public float cost;
    public float atkScaling;
    public float hlthScaling;
    public float spdScaling;
    public float costScaling;
}
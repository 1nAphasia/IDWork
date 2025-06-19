using UnityEngine;

[CreateAssetMenu(fileName = "BuffConfigSO", menuName = "Scriptable Objects/BuffConfigSO")]
public class BuffConfigSO : ScriptableObject
{
    public int buffId;
    public StatType targetStat;
    public StatModType modType;
    public float value;
    public float duration;
}

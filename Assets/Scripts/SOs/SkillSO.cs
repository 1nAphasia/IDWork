using UnityEngine;

[CreateAssetMenu(fileName = "SkillSO", menuName = "Equipment/Skill")]
public class SkillSO : ScriptableObject
{
    public int skillId;
    public int RelatedBuffID;
}
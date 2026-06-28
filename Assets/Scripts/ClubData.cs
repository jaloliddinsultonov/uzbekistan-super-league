using UnityEngine;

[CreateAssetMenu(fileName = "NewClub", menuName = "Football/Club")]
public class ClubData : ScriptableObject
{
    public string clubName;
    public Sprite logo;
    public Color primaryColor;
    public Color secondaryColor;

    [Header("Attack")]
    [Range(0, 100)] public int attackMin;
    [Range(0, 100)] public int attackMax;

    [Header("Defense")]
    [Range(0, 100)] public int defenseMin;
    [Range(0, 100)] public int defenseMax;

    [Header("Morale")]
    [Range(0, 100)] public int moraleMin;
    [Range(0, 100)] public int moraleMax;

    // Called at the start of each match
    public int GetAttack() => Random.Range(attackMin, attackMax + 1);
    public int GetDefense() => Random.Range(defenseMin, defenseMax + 1);
    public int GetMorale() => Random.Range(moraleMin, moraleMax + 1);
}
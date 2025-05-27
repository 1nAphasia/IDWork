using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class PlayerData : ScriptableObject
{
    public float maxHealth;
    public float currentHealth;
    public int coins;
    public bool skillReady;

    public void ResetData()
    {
        currentHealth = maxHealth;
        coins = 0;
        skillReady = false;
    }
}

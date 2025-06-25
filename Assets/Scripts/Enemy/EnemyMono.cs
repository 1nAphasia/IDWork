using UnityEngine;

public class EnemyMono : MonoBehaviour
{
    public EnemyType type;
    public int Health;
    public int Armor;

    public void DamageDelt(int damage)
    {
        if (Armor > 0)
        {
            int armorDamage = Mathf.Min(Armor, damage);
            Armor -= armorDamage;
            damage -= armorDamage;
        }
        if (damage > 0)
        {
            Health -= damage;
        }
        if (Health <= 0)
        {
            Death();
        }
    }

    public void Death()
    {
        Destroy(gameObject);
    }
}
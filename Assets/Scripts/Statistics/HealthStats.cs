using UnityEngine;

public interface IHealthStats
{
    public float Health { get; }
    public float MaxHealth { get; }
    public float HealthGain { get; }

    public abstract void Damage(float ammount);
    public abstract void Heal(float ammount);
}

public abstract class HealthStats : IHealthStats
{
    public float Health { get; private set; }
    public float MaxHealth { get; private set; }
    public float HealthGain { get; private set; }

    protected HealthStats(float maxHealth, float healthGain)
    {
        MaxHealth = maxHealth;
        HealthGain = healthGain;
    }

    public void Damage(float ammount) => Health = Mathf.Clamp(Health - ammount, 0, MaxHealth);

    public void Heal(float ammount) => Health = Mathf.Clamp(Health + ammount, 0, MaxHealth);
}

public class PlayerHealthStats : HealthStats
{
    public PlayerHealthStats(float maxHealth = 100f, float healthGain = 5f)
        : base(maxHealth, healthGain) { }
}

public class EnemyHealthStats : HealthStats
{
    public EnemyHealthStats(float maxHealth = 30f, float healthGain = 0f)
        : base(maxHealth, healthGain) { }
}

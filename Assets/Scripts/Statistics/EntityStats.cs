using System;

public interface IEntityStats
{
    public string Name { get; }
    public IHealthStats HealthStats { get; }
    public IMovementStats MovementStats { get; }
    public abstract void Damage(float ammount);
    public abstract void Heal(float ammount);
}

public abstract class EntityStats : IEntityStats
{
    public string Name { get; protected internal set; }
    public IHealthStats HealthStats { get; protected internal set; }
    public IMovementStats MovementStats { get; protected internal set; }

    protected EntityStats(string name, IHealthStats healthStats, IMovementStats movementStats)
    {
        Name = name;
        HealthStats = healthStats ?? throw new ArgumentNullException(nameof(healthStats));
        MovementStats = movementStats ?? throw new ArgumentNullException(nameof(movementStats));
    }

    public void Damage(float ammount) => HealthStats.Damage(ammount);

    public void Heal(float ammount) => HealthStats.Damage(ammount);

    public void Kill() { }
}

public class PlayerStats : EntityStats
{
    public PlayerStats(
        IHealthStats healthStats = null,
        IMovementStats movementStats = null,
        string name = "Player"
    )
        : base(
            name,
            healthStats ?? new PlayerHealthStats(),
            movementStats ?? new PlayerMovementStats()
        ) { }
}

public class EnemyStats : EntityStats
{
    public EnemyStats(
        IHealthStats healthStats = null,
        IMovementStats movementStats = null,
        string name = "Enemy"
    )
        : base(
            name,
            healthStats ?? new EnemyHealthStats(),
            movementStats ?? new EnemyMovementStats()
        ) { }
}

public interface IMovementStats
{
    public float MovementSpeed { get; }
    public float MovementResponse { get; }
}

public abstract class MovementStats : IMovementStats
{
    public float MovementSpeed { get; protected set; }
    public float MovementResponse { get; protected set; }

    protected MovementStats(float movementSpeed, float movementResponse)
    {
        MovementSpeed = movementSpeed;
        MovementResponse = movementResponse;
    }
}

public class PlayerMovementStats : MovementStats
{
    public PlayerMovementStats(float movementSpeed = 5f, float movementResponse = 0.995f)
        : base(movementSpeed, movementResponse) { }
}

public class EnemyMovementStats : MovementStats
{
    public EnemyMovementStats(float movementSpeed = 5f, float movementResponse = 0.995f)
        : base(movementSpeed, movementResponse) { }
}

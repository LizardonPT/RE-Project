using UnityEngine;

public class FluidPhysics
{
    public float Amount = 5.0f;
    public float SpillRate = 1f;
    public int ParticlesPerLiter = 1000;

    private const float neededForce = 0.05f;

    private Vector3 fluidMomentum;
    private ParticleSystem particleSystem;

    public FluidPhysics(ParticleSystem ps) => this.particleSystem = ps;

    public void Simulate(Vector3 deltaPos, Quaternion containerRotation, float dt)
    {
        if (dt <= 0)
            return;

        Vector3 containerVelocity = deltaPos / dt;

        fluidMomentum += Physics.gravity * dt;
        fluidMomentum = Vector3.Lerp(fluidMomentum, containerVelocity, 0.2f);

        Vector3 relativeMomentum = fluidMomentum - containerVelocity;
        fluidMomentum *= 0.9f;

        relativeMomentum *= 0.7f;

        Vector3 exitDirection = containerRotation * Vector3.up;

        float forceTowardsExit = Vector3.Dot(relativeMomentum, exitDirection);

        if (forceTowardsExit > neededForce && Amount > 0)
        {
            float spill = forceTowardsExit * SpillRate * dt;
            spill = Mathf.Min(spill, Amount);
            Amount -= spill;

            EmitSpill(spill, relativeMomentum);
        }
    }

    private void EmitSpill(float amount, Vector3 velocity)
    {
        int count = Mathf.Max(1, Mathf.RoundToInt(amount * ParticlesPerLiter));
        var emitParams = new ParticleSystem.EmitParams { velocity = velocity };
        particleSystem.Emit(emitParams, count);
    }
}

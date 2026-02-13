using UnityEngine;

public class PotionsObject : MonoBehaviour
{
    [SerializeField]
    ParticleSystem particleSystem;
    FluidPhysics physics;

    private Vector3 lastPosition;

    void Awake()
    {
        lastPosition = transform.position;
        physics = new FluidPhysics(particleSystem);
    }

    void FixedUpdate()
    {
        Vector3 deltaPos = lastPosition - transform.position;
        physics.Simulate(deltaPos, transform.rotation, Time.fixedDeltaTime);
        lastPosition = transform.position;
    }
}

using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Entity : MonoBehaviour
{
    [SerializeField]
    private Transform head;
    public Transform Head => head;
    private Rigidbody rb;
    public Rigidbody Rigidbody => rb;

    [Header("Camera")]
    [SerializeField]
    Camera camera;

    [SerializeField]
    float rotationSpeed = 1f;

    [SerializeField]
    [Range(0, 1)]
    float rotationResponse;

    [SerializeField]
    bool controllingCamera;
    public bool ControllingCamera => controllingCamera;

    IEntityStats entityStats;
    MovementControl movementControl;

    CameraControl cameraControl;
    public CameraControl CameraControl => cameraControl;

    [SerializeField]
    private FastAccess acess;

    void Initialize()
    {
        entityStats = new PlayerStats();
        rb = GetComponent<Rigidbody>();
        cameraControl = new CameraControl(this, camera, rotationSpeed, rotationResponse);
        movementControl = new MovementControl(
            this,
            entityStats.MovementStats.MovementSpeed,
            entityStats.MovementStats.MovementResponse,
            acess
        );
    }

    void Awake() => Initialize();

    void Update()
    {
        movementControl?.Update();
        if (ControllingCamera)
            cameraControl.Update();
    }

    void OnParticleCollision(GameObject obj)
    {
        Debug.Log("Particle collided with: " + obj.name);
    }
}

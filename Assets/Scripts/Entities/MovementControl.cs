using UnityEngine;
using UnityEngine.XR;

public class MovementControl
{
    public enum MovementType
    {
        Player,
        NPC,
    }

    public MovementControl(
        Entity entity,
        float movementSpeed,
        float movementResponse,
        FastAccess acess
    )
    {
        this.entity = entity;
        this.movementSpeed = movementSpeed;
        this.movementResponse = movementResponse;
        this.acess = acess;
    }

    private FastAccess acess;

    float movementSpeed;
    float movementResponse = 1;
    private MovementType movementType;

    Entity entity;
    public Entity Entity => entity;

    private Vector3 velocity = Vector2.zero;

    bool EntityAttributed => entity != null;

    public bool Update() => Move();

    private bool Move()
    {
        if (!EntityAttributed)
            return false;

        Vector2 input = Vector2.zero;

        if (movementType == MovementType.Player)
            input = Utilities.GetKeyboardInputs();
        else if (movementType == MovementType.NPC)
            input = acess.Player.transform.position - entity.transform.position;

        if (input.sqrMagnitude > 1f)
            input.Normalize();
        else
        {
            InputDevice device = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
            if (device.TryGetFeatureValue(CommonUsages.primary2DAxis, out input))
                return false;
        }
        Vector3 forward = entity.CameraControl.Forward;
        forward.y = 0f;
        forward.Normalize();

        Vector3 right = entity.CameraControl.Right;
        right.y = 0f;
        right.Normalize();

        Vector3 targetVelocity = (right * input.y + forward * input.x) * movementSpeed;

        float t = 1f - Mathf.Pow(1f - movementResponse, Time.deltaTime);
        velocity = Vector3.Lerp(velocity, targetVelocity, t);

        entity.Rigidbody.linearVelocity = new Vector3(
            velocity.x,
            entity.Rigidbody.linearVelocity.y,
            velocity.z
        );

        return true;
    }
}

using UnityEngine;
using UnityEngine.XR;

public class MovementControl
{
    public MovementControl(Entity entity, float movementSpeed, float movementResponse)
    {
        this.entity = entity;
        this.movementSpeed = movementSpeed;
        this.movementResponse = movementResponse;
    }

    float movementSpeed;
    float movementResponse = 1;

    Entity entity;
    public Entity Entity => entity;

    private Vector3 velocity = Vector2.zero;

    bool EntityAttributed => entity != null;

    public bool Update() => Move();

    private bool Move()
    {
        if (!EntityAttributed)
            return false;

        Vector2 input = Utilities.GetKeyboardInputs();
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

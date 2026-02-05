using UnityEngine;

public class CameraControl
{
    public CameraControl(Entity entity, Camera camera, float rotationSpeed, float rotationResponse)
    {
        this.entity = entity;
        this.camera = camera;
        this.rotationSpeed = rotationSpeed;
        this.rotationResponse = rotationResponse;
    }

    float rotationSpeed;
    float rotationResponse;

    Entity entity;
    Camera camera;
    public Vector3 Forward => entity.Head.forward;
    public Vector3 Right => entity.Head.right;

    private Transform entityHead => entity.Head;

    public bool Update() => Rotate();

    Vector2 rotation; // x = pitch, y = yaw

    private bool Rotate()
    {
        Vector2 input = Utilities.GetMouseInputs() * rotationSpeed * Time.deltaTime;
        if (input == Vector2.zero)
            return false;

        rotation.x -= input.y; // pitch
        rotation.y += input.x; // yaw

        entityHead.rotation = Quaternion.Euler(rotation);
        return true;
    }

}

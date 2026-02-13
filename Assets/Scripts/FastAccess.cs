using UnityEngine;

public class FastAccess : MonoBehaviour
{
    [SerializeField]
    Entity player;
    public Entity Player => player;
}

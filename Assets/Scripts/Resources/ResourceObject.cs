using Resources;
using UnityEngine;

public class ResourceObject : MonoBehaviour
{
    [SerializeField]
    string resourceName;
    public IResource Resource { get; private set; }

    void Awake() => Resource = GenerateResource();

    IResource GenerateResource()
    {
        IResource resource = null;
        switch (resourceName)
        {
            case "Wood":
                resource = new Wood();
                break;
            case "Iron":
                resource = new Iron();
                break;
            case "Coal":
                resource = new Coal();
                break;
        }

        Debug.Log("Resource type not found!");
        return resource;
    }
}

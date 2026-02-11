using Resources;
using UnityEngine;

[RequireComponent(typeof(ResourceObject))]
[RequireComponent(typeof(ParticleSystem))]
public class CraftableResource : MonoBehaviour
{
    public ResourceObject ResourceObject { get; private set; }
    private ICraftableResource craftable;

    [SerializeField]
    private ParticleSystem particleSystem;

    void Awake()
    {
        ResourceObject = GetComponent<ResourceObject>();
        if (ResourceObject.Resource is ICraftableResource)
            craftable = (ICraftableResource)ResourceObject.Resource;
        else
            Debug.LogError("Resource is not a ICraftableResource");
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (craftable == null)
            return;

        Collision coll = collision;

        bool hit = craftable.Hit(coll, craftable.StrengthNeeded);

        if (hit)
        {
            particleSystem.transform.position = collision.contacts[0].point;
            particleSystem.Play();
        }
    }
}

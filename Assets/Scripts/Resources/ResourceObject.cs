using System.Collections;
using Resources;
using UnityEngine;
using UnityEngine.XR;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(Collider))]
public class ResourceObject : MonoBehaviour
{
    [SerializeField]
    string resourceName;
    Color defaultColor;
    MeshRenderer meshR;
    public IResource Resource { get; private set; }

    void Awake()
    {
        Resource = GenerateResource();
        meshR = GetComponent<MeshRenderer>();
        defaultColor = meshR.material.color;
        Time.timeScale = 5f;
    }

    IResource GenerateResource()
    {
        IResource resource = null;
        switch (resourceName.ToLower())
        {
            case "wood":
                resource = new Wood(gameObject);
                break;
            case "iron":
                resource = new Iron(gameObject, new Mesh[] { });
                break;
            case "coal":
                resource = new Coal(gameObject);
                break;

            default:
                Debug.Log("Resource type not found!");
                break;
        }

        return resource;
    }

    public void AutoDestroy() => StartCoroutine(DestructionProcess(0.2f, 10));

    IEnumerator DestructionProcess(float duration, int intervals)
    {
        float deltaTime = duration / intervals;
        WaitForSeconds delay = new WaitForSeconds(deltaTime);

        for (int i = 0; i < intervals; i++)
        {
            Color c = defaultColor;
            c.a = 1f - (float)i / (intervals - 1);
            meshR.material.color = c;

            yield return delay;
        }

        Destroy(gameObject);
    }
}

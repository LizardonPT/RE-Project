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
    const string resourceName = "None";
    Color defaultColor;
    MeshRenderer meshR;
    public IResource Resource { get; private set; }

    void Awake()
    {
        Resource = Utilities.GenerateResource(resourceName, gameObject);
        meshR = GetComponent<MeshRenderer>();
        defaultColor = meshR.material.color;
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

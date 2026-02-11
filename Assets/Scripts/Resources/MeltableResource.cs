using System.Collections;
using Resources;
using UnityEngine;

public class MeltableResourceObject : MonoBehaviour
{
    MeltableResource resource;
    float Temperature => resource.Temperature;
    const float UPDATE_RATE = 5f; //5s

    void Awake()
    {
        resource = new Iron(gameObject, new Mesh[] { });
        StartCoroutine(DecayTemperature());
    }

    IEnumerator DecayTemperature()
    {
        WaitForSeconds wait = new WaitForSeconds(UPDATE_RATE);

        while (true)
        {
            yield return wait;
            resource.DecayTemperature(UPDATE_RATE);
        }
    }
}

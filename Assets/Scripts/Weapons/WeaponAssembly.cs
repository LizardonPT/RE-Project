using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class WeaponAssembly : MonoBehaviour
{
    [SerializeField]
    GameObject weaponPrefab;

    [SerializeField]
    WeaponPart[] partsNeeded;

    private List<WeaponPart> partsDelivered = new();

    void Awake()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        WeaponPartObject partObj = other.GetComponent<WeaponPartObject>();
        if (!partObj)
            return;

        WeaponPart part = partObj.WeaponPart;

        if (!IsPartNeeded(part))
            return;

        AddPart(part);
        CheckAssemblyComplete();
    }

    void OnTriggerExit(Collider other)
    {
        WeaponPartObject partObj = other.GetComponent<WeaponPartObject>();
        if (!partObj)
            return;

        RemovePart(partObj.WeaponPart);
    }

    bool IsPartNeeded(WeaponPart part)
    {
        return partsNeeded.Contains(part);
    }

    void AddPart(WeaponPart part)
    {
        if (partsDelivered.Contains(part))
            return;

        partsDelivered.Add(part);
        Debug.Log("Weapon part added: " + part.Name);
    }

    void RemovePart(WeaponPart part)
    {
        partsDelivered.Remove(part);
        Debug.Log("Weapon part removed: " + part.Name);
    }

    void CheckAssemblyComplete()
    {
        if (partsDelivered.Count == partsNeeded.Length)
        {
            Debug.Log("Weapon Assembled!");
            Instantiate(weaponPrefab, transform.position, transform.rotation);
        }
    }
}

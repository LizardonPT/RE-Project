using UnityEngine;
using Weapons;

public class WeaponAssembly : MonoBehaviour
{
    [SerializeField]
    GameObject weaponPrefab;

    [SerializeField]
    WeaponParts partsNeeded;

    void OnTriggerEnter(Collision collision) { }
}

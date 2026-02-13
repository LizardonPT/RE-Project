using UnityEngine;

[RequireComponent(typeof(Collider))]
public class WeaponPartObject : MonoBehaviour
{
    [SerializeField]
    private WeaponPart weaponPart;
    public WeaponPart WeaponPart => weaponPart;

    void Awake()
    {
        GetComponent<Collider>().isTrigger = true;
    }
}

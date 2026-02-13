using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Resources;
using Resources;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.GlobalIllumination;

[RequireComponent(typeof(Collider))]
public class HeatSource : MonoBehaviour
{
    [SerializeField]
    Vector2 UpdateRate;

    [SerializeField]
    UnityEvent OnFuelEvent;

    [SerializeField]
    UnityEvent OutFuelEvent;

    [SerializeField]
    ParticleSystem smoke;
    private Color smokeColor;

    [SerializeField]
    Light light;

    List<MeltableResource> melting = new List<MeltableResource>();
    List<MeltableResource> decaying = new List<MeltableResource>();
    float fuelTime = 0f;
    float MAX_FUEL_TIME = 300;
    float MIN_FUEL_TIME = 0.00001f;
    Coroutine clockCoroutine;

    bool burning = true;
    public bool Burning => burning;

    private bool HasFuel => FuelLeft > MIN_FUEL_TIME;
    private float FuelLeft => Mathf.Clamp(fuelTime - Time.time, MIN_FUEL_TIME, MAX_FUEL_TIME);
    private float Efficiency => FuelLeft / MAX_FUEL_TIME;

    void Awake()
    {
        smokeColor = smoke.main.startColor.color;
        UpdateVisualComponents();
        StartCoroutine(DecayTemperatures());
        SetState(false);
    }

    public void OnTriggerEnter(Collider coll)
    {
        ResourceObject resourceObj = coll.gameObject.GetComponent<ResourceObject>();

        if (!resourceObj)
            return; // not resource

        IResource resource = resourceObj.Resource;

        if (resource is FuelResource) // coal
        {
            FuelResource fuel = (FuelResource)resource;
            if (fuel.Burn(coll.gameObject))
                AddFuel(fuel);
        }
        else if (resource is MeltableResource) // iron
        {
            MeltableResource meltable = (MeltableResource)resource;
            AddMeltable(meltable);
        }
    }

    public void OnTriggerExit(Collider coll)
    {
        ResourceObject resourceObj = coll.gameObject.GetComponent<ResourceObject>();

        if (!resourceObj)
            return; // not resource

        IResource resource = resourceObj.Resource;
        if (resource is MeltableResource) // iron
        {
            MeltableResource meltable = (MeltableResource)resource;
            RemoveMeltable(meltable);
        }
    }

    void StartClock()
    {
        // more objects = slower update rate
        if (clockCoroutine != null)
            StopCoroutine(clockCoroutine);

        float rate = GetRate(Efficiency);
        clockCoroutine = StartCoroutine(Clock(rate));
    }

    void AddFuel(FuelResource rResource)
    {
        fuelTime = Time.time + FuelLeft + rResource.FuelTime;
        StartClock();
    }

    void AddMeltable(MeltableResource mResource)
    {
        if (!melting.Contains(mResource))
        {
            melting.Add(mResource);
            if (!decaying.Contains(mResource))
                decaying.Add(mResource);
            StartClock();
        }
    }

    void RemoveMeltable(MeltableResource mResource)
    {
        if (melting.Contains(mResource))
            melting.Remove(mResource);
    }

    IEnumerator Clock(float time, float efficiency = 1)
    {
        SetState(true);
        UpdateVisualComponents();
        WaitForSeconds wait = new WaitForSeconds(time);
        float rate = time;
        float eTmp = efficiency;
        while (HasFuel)
        {
            yield return wait;
            if (Mathf.Abs(eTmp - Efficiency) > 0.05f) // MAX_FUEL_TIME / 50
                RateUpdate();

            UpdateHeat(rate);
            UpdateVisualComponents();
        }
        SetState(false);
        UpdateVisualComponents();

        yield break;

        void RateUpdate()
        {
            float lRate = GetRate(Efficiency);
            eTmp = Efficiency;
            rate = lRate;

            wait = new WaitForSeconds(rate);
        }
    }

    IEnumerator DecayTemperatures()
    {
        float updateRate = UpdateRate.x;
        WaitForSeconds wait = new WaitForSeconds(updateRate);

        while (true)
        {
            yield return wait;
            for (int i = 0; i < decaying.Count; i++)
                decaying[i].DecayTemperature(updateRate);
        }
    }

    float GetRate(float efficiency) => Mathf.Lerp(UpdateRate.x, UpdateRate.y, efficiency);

    public void UpdateHeat(float time, bool exponentialBehaviour = true)
    {
        float riseMult = exponentialBehaviour ? FuelLeft / MAX_FUEL_TIME : 0;
        // float decayMult = exponentialBehaviour ? 1 - (FuelLeft / MAX_FUEL_TIME) : 0;

        for (int i = 0; i < melting.Count; i++)
            melting[i].RiseTemperature(time, riseMult); // more heat = faster temperature rise

        // for (int i = 0; i < decaying.Count; i++)
        //     melting[i].DecayTemperature(time);
    }

    #region Visual

    public void SetState(bool burn)
    {
        if (burning == burn)
            return;

        burning = burn;
        if (burn)
        {
            OnFuelEvent.Invoke();
            smoke.Play();
        }
        else
        {
            OutFuelEvent.Invoke();
            smoke.Pause();
        }

        UpdateVisualComponents();
    }

    void UpdateVisualComponents()
    {
        UpdateSmoke();
        UpdateLight();
    }

    void UpdateSmoke()
    {
        var main = smoke.main; // copy
        Color newColor = smokeColor;
        float alpha;
        if (Efficiency > 0)
            alpha = Mathf.Lerp(0.5f, 1, Efficiency);
        else
            alpha = 0;

        newColor.a = alpha;
        main.startColor = newColor;
    }

    void UpdateLight() => light.intensity = Efficiency;

    #endregion
}

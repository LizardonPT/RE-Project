using System;
using System.Collections;
using System.Collections.Generic;
using Resources;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class HeatSource : MonoBehaviour
{
    private Collider collider;

    List<HotResource> source = new List<HotResource>();
    List<HotResource> melting = new List<HotResource>();

    void Update()
    {
        for (int i = 0; i < source.Count; i++)
            source[i].RiseTemperature(Time.deltaTime);
    }
}

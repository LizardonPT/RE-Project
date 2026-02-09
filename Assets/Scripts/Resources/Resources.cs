using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Resources
{
    #region Basics

    public interface IResource
    {
        public string Name { get; }
    }

    public abstract class Resource : IResource
    {
        public string Name { get; }

        protected Resource(string name) => Name = name;
    }

    #region + Hot

    public interface IFuelResource
    {
        public float FuelTime { get; }
    }

    public interface IHotResource
    {
        public float Temperature { get; }
        public float MaxTemperature { get; }
        public float TemperatureRise { get; }
        public float TemperatureDecay { get; }
    }

    public abstract class FuelResource : Resource, IFuelResource
    {
        public float FuelTime { get; }

        protected FuelResource(string name, float fuelTime)
            : base(name) => FuelTime = fuelTime;

        public bool Burn(GameObject resourceObject)
        {
            ResourceObject resourceObj = resourceObject.GetComponent<ResourceObject>();
            if (!resourceObj)
            {
                Debug.LogError("Object name " + resourceObject.name + " is not a resource object.");
                return false;
            }
            IResource resource = resourceObj.Resource;
            if (!(resource is FuelResource))
            {
                Debug.LogError("Object is not a fuel resource");
                return false;
            }
            resourceObj.AutoDestroy();
            return true;
        }
    }

    public abstract class HotResource : Resource, IHotResource
    {
        public float Temperature { get; private set; }
        public float MaxTemperature { get; }
        public float MeltTemperature { get; }
        public float TemperatureRise { get; }
        public float TemperatureDecay { get; }

        protected HotResource(
            string name,
            float initialTemperature,
            float maxTemperature,
            float temperatureRise,
            float temperatureDecay,
            float meltTemperature
        )
            : base(name)
        {
            Temperature = initialTemperature;
            MaxTemperature = maxTemperature;
            MeltTemperature = meltTemperature;
            TemperatureRise = temperatureRise;
            TemperatureDecay = temperatureDecay;
        }

        public void RiseTemperature(float time, float temperatureMultiplier = 1f) =>
            Temperature = Mathf.Clamp(
                Temperature + (TemperatureRise * time * (1 + temperatureMultiplier)),
                0,
                MaxTemperature
            );

        public void DecayTemperature(float time, float temperatureMultiplier = 0f) =>
            Temperature = Mathf.Clamp(
                Temperature - (TemperatureRise * time * (1 + temperatureMultiplier)),
                0,
                MaxTemperature
            );
    }

    #endregion
    #endregion
    #region Advanced

    public abstract class SculptableResource : Resource
    {
        protected SculptableResource(string name)
            : base(name) { }

        public abstract void Sculpt();
    }

    public abstract class MeltableResource : HotResource
    {
        protected MeltableResource(
            string name,
            float initialTemperature = 0f,
            float maxTemperature = 500f,
            float meltTemperature = 400f,
            float temperatureRise = 8f,
            float temperatureDecay = 4f
        )
            : base(
                name,
                initialTemperature,
                maxTemperature,
                meltTemperature,
                temperatureRise,
                temperatureDecay
            ) { }

        public abstract void Melt();
    }

    #endregion
    #region Custom Resources

    public class Wood : SculptableResource
    {
        public Wood()
            : base("Wood") { }

        public override void Sculpt() { }
    }

    public class Iron : MeltableResource
    {
        public Iron()
            : base("Iron") { }

        public override void Melt() { }
    }

    public class Coal : FuelResource
    {
        public Coal()
            : base("Coal", 60) { }
    }

    #endregion
}

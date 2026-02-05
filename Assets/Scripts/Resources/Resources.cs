using System;
using UnityEngine;

namespace Resources
{
    #region Basics
    public enum ResourceType
    {
        Sculptable,
        Meltable,
        Placeable,
    }

    public interface IResource
    {
        public string Name { get; }
        public ResourceType Type { get; }
        public abstract void Interact();
    }

    public interface IHotResource
    {
        public float Temperature { get; }
        public float MaxTemperature { get; }
        public float TemperatureRise { get; }
        public float TemperatureDecay { get; }
    }

    public abstract class Resource : IResource
    {
        public string Name { get; }
        public ResourceType Type { get; }

        protected Resource(string name, ResourceType type)
        {
            Name = name;
            Type = type;
        }

        public abstract void Interact();
    }

    public abstract class HotResource : Resource, IHotResource
    {
        public float Temperature { get; private set; }
        public float MaxTemperature { get; }
        public float TemperatureRise { get; }
        public float TemperatureDecay { get; }

        protected HotResource(
            string name,
            ResourceType type = ResourceType.Meltable,
            float initialTemperature = 0f,
            float maxTemperature = 500f,
            float temperatureRise = 8f,
            float temperatureDecay = 4f
        )
            : base(name, type)
        {
            Temperature = initialTemperature;
            MaxTemperature = maxTemperature;
            TemperatureRise = temperatureRise;
            TemperatureDecay = temperatureDecay;
        }

        public void RiseTemperature(float time) =>
            Temperature = Mathf.Clamp(Temperature + (TemperatureRise * time), 0, MaxTemperature);

        public void DecayTemperature(float time) =>
            Temperature = Mathf.Clamp(Temperature - (TemperatureRise * time), 0, MaxTemperature);
    }
    #endregion

    #region Custom Resources

    public class Wood : Resource
    {
        public Wood()
            : base("Wood", ResourceType.Sculptable) { }

        public override void Interact() => Sculpt();

        private void Sculpt() { }
    }

    public class Iron : HotResource
    {
        public Iron()
            : base("Iron") { }

        public override void Interact() => Melt();

        private void Melt() { }
    }

    public class Coal : Resource
    {
        public Coal()
            : base("Coal", ResourceType.Placeable) { }

        public override void Interact() => Place();

        private void Place() { }
    }

    #endregion
}

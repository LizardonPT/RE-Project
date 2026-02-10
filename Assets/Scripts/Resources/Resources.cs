using UnityEngine;

namespace Resources
{
    #region Basics

    public interface IResource
    {
        public string Name { get; }
        public GameObject GameObject { get; }
    }

    public abstract class Resource : IResource
    {
        public string Name { get; }
        public GameObject GameObject { get; }

        protected Resource(string name, GameObject obj)
        {
            Name = name;
            GameObject = obj;
        }
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

        protected FuelResource(string name, GameObject obj, float fuelTime)
            : base(name, obj) => FuelTime = fuelTime;

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

        public Color DefaultColor { get; }
        public Color MeltedColor { get; }

        public void VisualUpdate(MeshRenderer renderer)
        {
            renderer.material.EnableKeyword("_EMISSION");
            float progress = Mathf.Clamp(Temperature / MeltTemperature, 0, 1);
            Debug.Log("Temperature: " + Temperature + "/" + MeltTemperature);
            renderer.material.SetColor(
                "_EmissionColor",
                Color.Lerp(Color.black, MeltedColor, progress)
            );
        }

        protected HotResource(
            string name,
            GameObject obj,
            float initialTemperature,
            float maxTemperature,
            float meltTemperature,
            float temperatureRise,
            float temperatureDecay,
            Color defaultColor,
            Color meltedColor
        )
            : base(name, obj)
        {
            Temperature = initialTemperature;
            MaxTemperature = maxTemperature;
            MeltTemperature = meltTemperature;
            TemperatureRise = temperatureRise;
            TemperatureDecay = temperatureDecay;

            DefaultColor = defaultColor;
            MeltedColor = meltedColor;
        }

        public void RiseTemperature(float time, float temperatureMultiplier = 1f)
        {
            Temperature = Mathf.Clamp(
                Temperature + (TemperatureRise * time * (1 + temperatureMultiplier)),
                0,
                MaxTemperature
            );
            VisualUpdate(GameObject.GetComponent<MeshRenderer>());
        }

        public void DecayTemperature(float time, float temperatureMultiplier = 0f)
        {
            Temperature = Mathf.Clamp(
                Temperature - (TemperatureDecay * time * (1 + temperatureMultiplier)),
                0,
                MaxTemperature
            );
            VisualUpdate(GameObject.GetComponent<MeshRenderer>());
        }
    }

    #endregion

    #region + Craftable
    public interface ICraftableResource
    {
        public int Progress { get; set; }
        public Mesh[] Meshes { get; }
        public MeshFilter Filter { get; }
        public float StrengthNeeded { get; }
        public abstract bool Hit(Collider coll, float strengthNeeded);
    }

    public abstract class CraftableResource : HotResource, ICraftableResource
    {
        public int Progress { get; set; }
        public Mesh[] Meshes { get; }
        public MeshFilter Filter { get; }
        public float StrengthNeeded { get; }

        protected CraftableResource(
            string name,
            GameObject obj,
            Mesh[] meshes,
            MeshFilter meshFilter,
            float initialTemperature,
            float maxTemperature,
            float meltTemperature,
            Color defaultColor,
            Color meltedColor,
            float strengthNeeded,
            float temperatureRise = 2f,
            float temperatureDecay = 1f
        )
            : base(
                name,
                obj,
                initialTemperature,
                maxTemperature,
                meltTemperature,
                temperatureRise,
                temperatureDecay,
                defaultColor,
                meltedColor
            )
        {
            Meshes = meshes;
            Filter = meshFilter;
            Progress = 0;
            StrengthNeeded = strengthNeeded;
        }

        public bool Hit(Collider coll, float minVelocity = 2f)
        {
            if (Progress >= Meshes.Length)
                return false;

            float velocity = coll.gameObject.GetComponent<Rigidbody>().linearVelocity.magnitude;
            if (velocity < minVelocity)
            {
                Debug.LogError("Too weak!");
                return false;
            }

            if (Temperature < MeltTemperature)
            {
                Debug.LogError("Too cold!");
                return false;
            }

            Progress++;
            Filter.mesh = Meshes[Progress];
            return true;
        }
    }

    #endregion
    #endregion
    #region Advanced

    public abstract class SculptableResource : Resource
    {
        protected SculptableResource(string name, GameObject obj)
            : base(name, obj) { }

        public abstract void Sculpt();
    }

    public abstract class MeltableResource : CraftableResource
    {
        protected MeltableResource(
            string name,
            GameObject obj,
            Mesh[] meshes,
            MeshFilter meshFilter,
            float strengthNeeded,
            float initialTemperature,
            float maxTemperature,
            float meltTemperature,
            Color defaultColor,
            Color meltedColor,
            float temperatureRise = 8f,
            float temperatureDecay = 4f
        )
            : base(
                name,
                obj,
                meshes,
                meshFilter,
                initialTemperature,
                maxTemperature,
                meltTemperature,
                defaultColor,
                meltedColor,
                strengthNeeded,
                temperatureRise,
                temperatureDecay
            ) { }
    }

    #endregion
    #region Custom Resources

    public class Wood : SculptableResource
    {
        public Wood(GameObject obj)
            : base("Wood", obj) { }

        public override void Sculpt() { }
    }

    public class Iron : MeltableResource
    {
        public Iron(GameObject obj, Mesh[] meshes)
            : base(
                "Iron",
                obj,
                meshes,
                obj.GetComponent<MeshFilter>(),
                5f,
                0f,
                500f,
                400f,
                Color.black,
                Color.yellow * 3f
            ) { }
    }

    public class Coal : FuelResource
    {
        public Coal(GameObject obj)
            : base("Coal", obj, 60) { }
    }

    #endregion
}

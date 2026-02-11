using UnityEngine;

namespace Tools
{
    public interface ITool
    {
        public string Name { get; }
        public GameObject GameObject { get; }
    }

    public abstract class Tool : ITool
    {
        public string Name { get; }
        public GameObject GameObject { get; }

        public Tool(string name, GameObject obj)
        {
            Name = name;
            GameObject = obj;
        }
    }

    public class CraftingTool : Tool
    {
        protected CraftingTool(string name, GameObject obj)
            : base(name, obj) { }
    }

    public class Hammer : CraftingTool
    {
        public Hammer(GameObject obj)
            : base("Hammer", obj) { }
    }

    public class Axe : CraftingTool
    {
        public Axe(GameObject obj)
            : base("Axe", obj) { }
    }
}

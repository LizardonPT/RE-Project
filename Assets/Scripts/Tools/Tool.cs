namespace Tools
{
    public interface ITool
    {
        public string Name { get; }
    }

    public abstract class Tool : ITool
    {
        public string Name { get; }

        public Tool(string name) => Name = name;
    }

    public class CraftingTool : Tool
    {
        protected CraftingTool(string name)
            : base(name) { }
    }

    public class Hammer : CraftingTool
    {
        public Hammer()
            : base("Hammer") { }
    }
}

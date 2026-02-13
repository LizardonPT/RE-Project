namespace Weapons
{
    public enum WeaponParts
    {
        Blade,
        Handle,
    }

    public interface IWeaponPart
    {
        public string Name { get; set; }
        public WeaponParts Part { get; set; }
    }

    public class WeaponPart : IWeaponPart
    {
        public string Name { get; set; }
        public WeaponParts Part { get; set; }

        public WeaponPart(string name, WeaponParts part)
        {
            Name = name;
            Part = part;
        }
    }
}

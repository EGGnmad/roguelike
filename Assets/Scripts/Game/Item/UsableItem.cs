public abstract class UsableItem : Item
{
    public UsableItem(string name, string description) : base(name, description) {}

    public abstract void Use();
}
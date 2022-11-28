using Code.Interaction;

namespace Code
{
    public class Key : Pickupable
    {
        public override string name => "Key";

        protected override void Pickup(InteractionHandler handler)
        {
            if (!handler.inventory.NoDuplicateAdd(this)) { return; }

            Destroy(gameObject);
        }
    }
}
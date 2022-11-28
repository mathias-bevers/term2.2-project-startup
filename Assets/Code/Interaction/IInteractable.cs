

namespace Code.Interaction
{
	public interface IInteractable
	{
		void Interact(InteractionHandler handler);

		void OnHover(InteractionHandler handler);
	}
}
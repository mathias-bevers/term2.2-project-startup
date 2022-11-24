using UnityEngine;

namespace Code
{
	[RequireComponent(typeof(CharacterController))]
	public class ControlableEntity : MoveableEntity
	{
		public CharacterController characterController { get; private set; }
	}
}
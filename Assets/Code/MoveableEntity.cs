using UnityEngine;

namespace Code
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(MovementModule))]
    public class MoveableEntity : Entity
	{
        CharacterController controller;
        internal MovementModule movementModule;
        protected override void OnStart()
        {
            controller = GetComponent<CharacterController>();
            movementModule = GetComponent<MovementModule>();
            movementModule.controller = controller;
            base.OnStart();
        }

        protected override void Tick()
        {
            base.Tick();
        }
    }
}
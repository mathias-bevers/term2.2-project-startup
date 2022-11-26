using UnityEngine;

namespace Code
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(MovementModule))]
    public class MoveableEntity : Entity
	{
        CharacterController controller;
        public CharacterController getController { get => controller; }
        internal MovementModule movementModule;
        public Vector3 getPosition { get => controller.transform.position; }

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

        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }
    }
}
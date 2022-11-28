using UnityEngine;

namespace Code
{
	[RequireComponent(typeof(InputModule))]
    [RequireComponent(typeof(MovementModuleControlled))]

	public class ControlableEntity : MoveableEntity
	{
		[SerializeField] protected CameraRig _cameraRig;

        public CameraRig cameraRig { get => _cameraRig; }

        protected InputModule _inputModule;
        public InputModule inputModule { get => _inputModule; }

        [SerializeField] MouseSettings mouseSettings;

        new MovementModuleControlled movementModule { get => (MovementModuleControlled)((MoveableEntity)this).movementModule; set => ((MoveableEntity)this).movementModule = value; }


        protected override void OnStart()
        {
            base.OnStart();
            _inputModule = GetComponent<InputModule>();
            movementModule = GetComponent<MovementModuleControlled>();
            movementModule.inputModule = _inputModule;

        }

        protected override void Tick()
        {
            base.Tick();


            HandleCameraMovement();
        }


        void HandleCameraMovement()
        {
            if (_cameraRig == null) return;
            _cameraRig.PassThroughInput(_inputModule.mouseInput, mouseSettings);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }

        protected override void OnDeath()
        {
            base.OnDeath();
        }
    }
}
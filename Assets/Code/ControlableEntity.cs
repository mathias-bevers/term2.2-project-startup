using UnityEngine;

namespace Code
{
	[RequireComponent(typeof(InputModuleBase))]
    [RequireComponent(typeof(MovementModuleControlled))]

	public class ControlableEntity : MoveableEntity
	{
		[SerializeField] protected CameraRig _cameraRig;

        public CameraRig cameraRig { get => _cameraRig; }

        protected InputModuleBase inputModule;

        [SerializeField] MouseSettings mouseSettings;

        new MovementModuleControlled movementModule { get => (MovementModuleControlled)((MoveableEntity)this).movementModule; set => ((MoveableEntity)this).movementModule = value; }


        protected override void OnStart()
        {
            base.OnStart();
            inputModule = GetComponent<InputModuleBase>();
            movementModule = GetComponent<MovementModuleControlled>();
            movementModule.inputModule = inputModule;

        }

        protected override void Tick()
        {
            base.Tick();


            HandleCameraMovement();
        }


        void HandleCameraMovement()
        {
            if (_cameraRig == null) return;
            _cameraRig.PassThroughInput(inputModule.mouseInput, mouseSettings);
        }
    }
}
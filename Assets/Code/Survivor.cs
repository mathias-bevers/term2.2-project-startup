using UnityEngine;

namespace Code
{
	public class Survivor : ControlableEntity 
	{
        [SerializeField] Transform _cameraFollowPoint;
        public Transform cameraFollowPoint { get => _cameraFollowPoint; }
        [SerializeField] float _hoverUnderWater = 1;
        public float hoverUnderWater { get => _hoverUnderWater; }
        protected override void OnStart()
        {
            base.OnStart();
        }

        protected override void Tick()
        {
            base.Tick();
        }
    }
}
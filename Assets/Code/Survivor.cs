using UnityEngine;

namespace Code
{
	public class Survivor : ControlableEntity 
	{
        [SerializeField] Transform _cameraFollowPoint;
        public Transform cameraFollowPoint { get => _cameraFollowPoint; }
        [SerializeField] float _hoverUnderWater = 1;
        public float hoverUnderWater { get => _hoverUnderWater; }
        public bool targeted = false;
        public bool mainTarget = false;

        protected override void OnStart()
        {
            base.OnStart();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            FindObjectOfType<SurvivorHandler>()?.RegisterSurvivor(this);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            FindObjectOfType<SurvivorHandler>()?.DeregisterSurvivor(this);
        }

        public void SetGrabbedTarget()
        {
            Debug.Log("I get grabbed");
        }

        protected override void Tick()
        {
            base.Tick();
        }
    }
}
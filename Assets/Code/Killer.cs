using UnityEngine;

namespace Code
{
	public class Killer : ControlableEntity 
	{
        [SerializeField] float _maxUnderwater = 2;
        public float maxUnderwater { get => _maxUnderwater; }
        [SerializeField] float _maxDetectionDistance = 20;
        public float maxDetectionDistance { get => _maxDetectionDistance; }

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
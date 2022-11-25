using UnityEngine;

namespace Code
{
	public class Killer : ControlableEntity 
	{
        [SerializeField] float _maxUnderwater = 2;
        public float maxUnderwater { get => _maxUnderwater; }

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
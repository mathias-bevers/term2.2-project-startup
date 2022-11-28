using Code.Interaction;
using UnityEngine;

namespace Code
{
    [RequireComponent(typeof(Oxygen))]
    [RequireComponent(typeof(InteractionHandler))]
	public class Survivor : ControlableEntity 
	{
        Oxygen oxygen;
        public Oxygen getOxygen { get => oxygen; }
        [SerializeField] Transform _cameraFollowPoint;
        public Transform cameraFollowPoint { get => _cameraFollowPoint; }
        [SerializeField] float _hoverUnderWater = 1;
        public float hoverUnderWater { get => _hoverUnderWater; }


        public bool targeted = false;
        public bool mainTarget = false;
        public bool eateth = false;

        float oldCamDistance = 0;
        LayerMask oldLayerMask;

        InteractionHandler interaction;

        Killer killer = null;

        protected override void OnStart()
        {
            oxygen = GetComponent<Oxygen>();
            interaction = GetComponent<InteractionHandler>();
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

        public void SetGrabbedTarget(Killer killer)
        {
            movementModule.enabled = false;
            //inputModule.enabled = false;
            interaction.enabled = false;
            eateth = true;
            oldCamDistance = cameraRig.setMaxCamDistance;
            oldLayerMask = cameraRig.collidesLayerMask;
            cameraRig.collidesLayerMask &= ~(1 << LayerMask.NameToLayer("Player1"));
            cameraRig.setMaxCamDistance = 10;
            this.killer = killer;
        }

        public void SetUngrabbedTarget(Killer killer)
        {
            movementModule.enabled = true;
            //inputModule.enabled = true;
            interaction.enabled = true;
            eateth = false;
            cameraRig.setMaxCamDistance = oldCamDistance;
            cameraRig.collidesLayerMask = oldLayerMask;
            getTransform.rotation = Quaternion.identity;
            this.killer = null;
        }

        protected override void Tick()
        {
            base.Tick();
        }


        protected override void OnDeath()
        {
            base.OnDeath();
        }
    }
}
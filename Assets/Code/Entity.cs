using Code.Interaction;
using System;
using UnityEngine;

namespace Code
{
    [RequireComponent(typeof(InteractionHandler))]
	public class Entity : MonoBehaviour
	{

        InteractionHandler _interactionHandler;
        PerkHandler _perkHandler;

        public InteractionHandler getInteractionHandler { get => _interactionHandler; }
        public PerkHandler getPerkHandler { get => _perkHandler; }

        private bool hasTicked = false;
        private bool hasStarted = false;

        bool _died = false;
        public bool hasDied { get => _died; }

        private void Start()
        {
            _interactionHandler = GetComponent<InteractionHandler>();
            _perkHandler = GetComponent<PerkHandler>();
            OnStart();
            if(!hasStarted) Debug.LogError("Make sure to call base on OnStart() ");
        }

        private void Update()
        {
            hasTicked = false;
            Tick();
            if (!hasTicked) Debug.LogError("Make sure to call base on Tick()");
        }

        protected virtual void OnStart()
        {
            hasStarted = true;
        }

        protected virtual void Tick()
        {
            hasTicked = true;
        }

        protected virtual void OnEnable()
        {

        }

        protected virtual void OnDisable()
        {

        }


        public void KillEntity()
        {
            _died = true;
            OnDeath();
        }

        protected virtual void OnDeath()
        {

        }
    }
}
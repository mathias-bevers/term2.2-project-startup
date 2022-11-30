using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code
{
    public class Killer : ControlableEntity, ICollisionInteraction
    {
        [SerializeField] float _maxUnderwater = 2;
        public float maxUnderwater { get => _maxUnderwater; }
        [SerializeField] float _maxDetectionDistance = 20;
        public float maxDetectionDistance { get => _maxDetectionDistance; }

        [SerializeField]
        Transform eatPoint;
        Survivor currentlyTargeted = null;
        [SerializeField] float dinnerTime = 4;
        float eatTimer = 0;

        [Header("Collision Interaction Interface")]
        [SerializeField] CollisionInteraction _collisionInteraction;
        public CollisionInteraction CollisionInteraction { get => _collisionInteraction; set => _collisionInteraction = value; }
        [SerializeField] LayerMask _collidesWithLayers;
        public LayerMask collidesWithLayers { get => _collidesWithLayers; set => _collidesWithLayers = value; }

        [SerializeField] List<Survivor> targetedSurvivors = new List<Survivor>();
        [SerializeField] float biteOxygenAmount = 30;

        [SerializeField] float cooldown = 30;
        float cooldownTimer = 0;
        public InputSettingsEnum buttonToPress;

        protected override void OnStart()
        {
            base.OnStart();
        }

        protected override void Tick()
        {
            base.Tick();
            HandleTargeting();
            HandleTargeted();


            if (cooldownTimer > 0) { cooldownTimer -= Time.deltaTime; return; }
            if (!inputModule.settings.IsDown(buttonToPress)) return;

            cooldownTimer = cooldown;
            RegisterSpeedboost(new Speedboost(1.5f, 1.5f));
        }

        void HandleTargeted()
        {
            if (currentlyTargeted == null) return;

            currentlyTargeted.transform.position = eatPoint.transform.position;
            currentlyTargeted.transform.rotation = eatPoint.transform.rotation;
            eatTimer += Time.deltaTime;

            if (eatTimer <= dinnerTime) return;

            currentlyTargeted.RegisterSpeedboost(new Speedboost(2f, 4f));
            currentlyTargeted.getOxygen.RemoveChunk(biteOxygenAmount);
            currentlyTargeted.SetUngrabbedTarget(this);
            currentlyTargeted = null;
            eatTimer = 0;
        }

        void HandleTargeting()
        {
            if (currentlyTargeted != null) return;
            float lastDist = float.MaxValue;
            int last = -1;

            for (int i = 0; i < targetedSurvivors.Count; i++)
            {
                float dist = Vector3.Distance(targetedSurvivors[i].getPosition, getPosition);
                if (lastDist > dist)
                {
                    last = i;
                    lastDist = dist;
                }
            }
            for (int i = 0; i < targetedSurvivors.Count; i++)
            {
                targetedSurvivors[i].mainTarget = false;
                if (i != last) continue;

                targetedSurvivors[i].mainTarget = true;
                if (inputModule.OnButtonDown(InputType.ActionButton2))
                {
                    targetedSurvivors[i].SetGrabbedTarget(this);
                    currentlyTargeted = targetedSurvivors[i];
                    eatTimer = 0;
                }
            }
        }

        public void OnCollisionEnter(Collision collision) { }
        public void OnCollisionExit(Collision collision) { }
        public void OnCollisionStay(Collision collision) { }

        public void OnTriggerEnter(Collider collider)
        {
            Survivor s = collider.GetComponent<Survivor>();
            if (s == null) return;
            s.targeted = true;
            if (!targetedSurvivors.Contains(s)) targetedSurvivors.Add(s);
        }

        public void OnTriggerExit(Collider collider)
        {
            Survivor s = collider.GetComponent<Survivor>();
            if (s == null) return;
            s.targeted = false;
            s.mainTarget = false;
            if (targetedSurvivors.Contains(s)) targetedSurvivors.Remove(s);
        }

        public void OnTriggerStay(Collider collider)
        {


        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _collisionInteraction.RegisterCallback(this);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _collisionInteraction.DeregisterCallback(this);
        }
    }
}
using System.Collections.Generic;
using UnityEngine;

namespace Code
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(MovementModule))]
    public class MoveableEntity : Entity
	{
        CharacterController controller;
        //public CharacterController getController { get => controller; }
        public Transform getTransform { get => controller.transform; }
        internal MovementModule movementModule;
        public Vector3 getPosition { get => controller.transform.position; }

        protected override void OnStart()
        {
            controller = GetComponent<CharacterController>();
            movementModule = GetComponent<MovementModule>();
            movementModule.setController = controller;
            base.OnStart();
        }

        protected override void Tick()
        {
            HandleSpeedboosts();
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

        protected override void OnDeath()
        {
            base.OnDeath();
        }

        public float movementSpeedPerc { get => currentPercentage; }
        float currentPercentage = 1;
        List<Speedboost> activeSpeedboosts = new List<Speedboost>();

        public void RegisterSpeedboost(Speedboost speedboost)
        {
            activeSpeedboosts.Add(speedboost);
        }

        void HandleSpeedboosts()
        {
           
            if (activeSpeedboosts.Count == 0) currentPercentage = 1;

            float highest = 1;
            for (int i = activeSpeedboosts.Count - 1; i >= 0; i--)
            {
                activeSpeedboosts[i].TickTimer();
                if (activeSpeedboosts[i].timer <= 0)
                {
                    activeSpeedboosts.RemoveAt(i);
                    continue;
                }
                if (activeSpeedboosts[i].speedPercentage > highest)
                    highest = activeSpeedboosts[i].speedPercentage;
                
            }
            currentPercentage = highest;
        }
    }
    public class Speedboost
    {
        public float speedPercentage { get; private set; }
        public float time { get; private set; }
        public float timer { get; private set; }

        public Speedboost(float speedBoostPercentage, float time)
        {
            this.speedPercentage = speedBoostPercentage;
            this.time = time;
            this.timer = time;
        }

        public void TickTimer()
        {
            timer -= Time.deltaTime;
        }

    }
}
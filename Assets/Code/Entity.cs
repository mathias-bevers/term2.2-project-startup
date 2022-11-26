using System;
using UnityEngine;

namespace Code
{
	public class Entity : MonoBehaviour
	{
        private bool hasTicked = false;
        private bool hasStarted = false;

        private void Start()
        {
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
    }
}
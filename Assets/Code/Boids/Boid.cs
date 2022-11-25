﻿using UnityEngine;

namespace Code.Boids
{
	public class Boid : MonoBehaviour
	{
		[HideInInspector] public int numPerceivedFlockmates;
		[HideInInspector] public Vector3 avgAvoidanceHeading;
		[HideInInspector] public Vector3 avgFlockHeading;
		[HideInInspector] public Vector3 centreOfFlockmates;
		[HideInInspector] public Vector3 forward;

		// State
		[HideInInspector] public Vector3 position;

		private BoidSettings settings;

		// Cached
		private Material material;
		private Transform cachedTransform;
		private Transform target;

		// To update:
		private Vector3 acceleration;
		private Vector3 velocity;

		private void Awake()
		{
			material = transform.GetComponentInChildren<MeshRenderer>().material;
			cachedTransform = transform;
		}

		bool canUpdate = true;

        void OnEnable()
        {
			canUpdate = true;
		}

		void OnDisable()
        {
			canUpdate = false;
		}

        public void Initialize(BoidSettings settings, Transform target)
		{
			this.target = target;
			this.settings = settings;

			position = cachedTransform.position;
			forward = cachedTransform.forward;

			float startSpeed = (settings.minSpeed + settings.maxSpeed) / 2;
			velocity = transform.forward * startSpeed;
		}

		public void SetColour(Color col)
		{
			if (material != null) { material.color = col; }
		}

		public void UpdateBoid()
		{
			if (!canUpdate) return;
			Vector3 acceleration = Vector3.zero;

			if (target != null)
			{
				Vector3 offsetToTarget = target.position - position;
				acceleration = SteerTowards(offsetToTarget) * settings.targetWeight;
			}

			if (numPerceivedFlockmates != 0)
			{
				centreOfFlockmates /= numPerceivedFlockmates;

				Vector3 offsetToFlockmatesCentre = centreOfFlockmates - position;

				Vector3 alignmentForce = SteerTowards(avgFlockHeading) * settings.alignWeight;
				Vector3 cohesionForce = SteerTowards(offsetToFlockmatesCentre) * settings.cohesionWeight;
				Vector3 seperationForce = SteerTowards(avgAvoidanceHeading) * settings.seperateWeight;

				acceleration += alignmentForce;
				acceleration += cohesionForce;
				acceleration += seperationForce;
			}

			if (IsHeadingForCollision())
			{
				Vector3 collisionAvoidDir = ObstacleRays();
				Vector3 collisionAvoidForce = SteerTowards(collisionAvoidDir) * settings.avoidCollisionWeight;
				acceleration += collisionAvoidForce;
			}

			velocity += acceleration * Time.deltaTime;
			float speed = velocity.magnitude;
			Vector3 dir = velocity / speed;
			speed = Mathf.Clamp(speed, settings.minSpeed, settings.maxSpeed);
			velocity = dir * speed;

			cachedTransform.position += velocity * Time.deltaTime;
			cachedTransform.forward = dir;
			position = cachedTransform.position;
			forward = dir;
		}

		private bool IsHeadingForCollision() => Physics.SphereCast(position,
			settings.boundsRadius,
			forward,
			out RaycastHit _,
			settings.collisionAvoidDst,
			settings.obstacleMask);

		private Vector3 ObstacleRays()
		{
			Vector3[] rayDirections = BoidHelper.directions;

			for (int i = 0; i < rayDirections.Length; i++)
			{
				Vector3 dir = cachedTransform.TransformDirection(rayDirections[i]);
				Ray ray = new(position, dir);
				if (!Physics.SphereCast(ray, settings.boundsRadius, settings.collisionAvoidDst, settings.obstacleMask)) { return dir; }
			}

			return forward;
		}

		private Vector3 SteerTowards(Vector3 vector)
		{
			Vector3 v = (vector.normalized * settings.maxSpeed) - velocity;
			return Vector3.ClampMagnitude(v, settings.maxSteerForce);
		}
	}
}
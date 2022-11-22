﻿using System.Collections;
using System.Collections.Generic;
using Code.Boids;
using UnityEngine;

namespace Code.Boids
{
	public class BoidManager : MonoBehaviour
	{
		const int threadGroupSize = 1024;

		public BoidSettings settings;
		public ComputeShader compute;
		Boid[] boids;

		void Start()
		{
			boids = FindObjectsOfType<Boid>();
			foreach (Boid b in boids) { b.Initialize(settings, null); }

		}

		void Update()
		{
			if (boids != null)
			{

				int numBoids = boids.Length;
				var boidData = new BoidData[numBoids];

				for (int i = 0; i < boids.Length; i++)
				{
					boidData[i].position = boids[i].position;
					boidData[i].direction = boids[i].forward;
				}

				var boidBuffer = new ComputeBuffer(numBoids, BoidData.Size);
				boidBuffer.SetData(boidData);

				compute.SetBuffer(0, "boids", boidBuffer);
				compute.SetInt("numBoids", boids.Length);
				compute.SetFloat("viewRadius", settings.perceptionRadius);
				compute.SetFloat("avoidRadius", settings.avoidanceRadius);

				int threadGroups = Mathf.CeilToInt(numBoids / (float)threadGroupSize);
				compute.Dispatch(0, threadGroups, 1, 1);

				boidBuffer.GetData(boidData);

				for (int i = 0; i < boids.Length; i++)
				{
					boids[i].avgFlockHeading = boidData[i].flockHeading;
					boids[i].centreOfFlockmates = boidData[i].flockCenter;
					boids[i].avgAvoidanceHeading = boidData[i].avoidanceHeading;
					boids[i].numPerceivedFlockmates = boidData[i].flockMatesCount;

					boids[i].UpdateBoid();
				}

				boidBuffer.Release();
			}
		}
	}
}
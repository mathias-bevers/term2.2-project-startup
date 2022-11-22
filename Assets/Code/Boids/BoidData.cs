using UnityEngine;

namespace Code.Boids
{
	public struct BoidData
	{
		public Vector3 position;
		public Vector3 direction;

		public Vector3 flockHeading;
		public Vector3 flockCenter;
		public Vector3 avoidanceHeading;

		public int flockMatesCount;

		public static int Size => ((sizeof(float) * 3) * 5) + sizeof(int);
	}
}
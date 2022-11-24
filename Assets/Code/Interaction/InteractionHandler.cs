using System.Collections.Generic;
using UnityEngine;

namespace Code.Interaction
{
	[RequireComponent(typeof(ControlableEntity))]
	//Idea: make split up interaction / unclockers. 
	public class InteractionHandler : MonoBehaviour
	{
		[SerializeField] private float reach = 5.0f;
		[SerializeField] private LayerMask interactableMask;

		public bool hasUnlocker { get; protected set; }

		public List<Pickupable> inventory { get; } = new();
		private ControlableEntity controlableEntity;
		private Transform cachedTransform;

		private void Awake()
		{
			cachedTransform = transform;
			controlableEntity = GetComponent<ControlableEntity>();
		}

		public void FixedUpdate()
		{
			Ray ray = new(cachedTransform.position, controlableEntity.cameraRig.forward);

			if (!Physics.Raycast(ray, out RaycastHit hit, reach, interactableMask)) { return; }

			if (!Input.GetKeyDown(KeyCode.E)) { return; } //TODO: integrate with InputModules.

			IInteractable interactable = hit.transform.GetComponent<IInteractable>();
			interactable?.Interact(this);
		}
	}
}
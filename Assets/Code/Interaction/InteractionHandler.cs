using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Code.Interaction
{
	[RequireComponent(typeof(ControlableEntity))]
	//Idea: make split up interaction / unclockers. 
	public class InteractionHandler : MonoBehaviour
	{
		[SerializeField] private float reach = 3f;

		public bool hasKey => inventory.Any(item => item.GetType() == typeof(Key));

		public List<Pickupable> inventory { get; } = new();
		private bool canInteract = false;
		private ControlableEntity controlableEntity;
		private Transform cachedTransform;

		private void Awake()
		{
			cachedTransform = transform;
			controlableEntity = GetComponent<ControlableEntity>();
		}

		private void Update() { InteractionScan(); }

		private void InteractionScan()
		{
			canInteract = false;

			Ray ray = new(controlableEntity.cameraRig.followPoint.position, controlableEntity.cameraRig.forward);
			if (!Physics.Raycast(ray, out RaycastHit hit, reach)) { return; }

			IInteractable interactable = hit.transform.GetComponent<IInteractable>();
			if (interactable == null) { return; }

			canInteract = true;

			if (!controlableEntity.inputModule.OnButtonDown(InputType.Dive)) { return; }

			interactable.Interact(this);
		}
	}
}
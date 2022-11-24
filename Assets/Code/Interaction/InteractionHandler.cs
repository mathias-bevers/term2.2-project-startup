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
		private Camera cam; //TODO: remove when survivor has cameraRig.
		private ControlableEntity controlableEntity;
		private Transform cachedTransform;

		private void Awake()
		{
			cachedTransform = transform;
			controlableEntity = GetComponent<ControlableEntity>();
			cam = GetComponentInChildren<Camera>();
		}

		public void FixedUpdate()
		{
			canInteract = false;

			Debug.DrawRay(cam.transform.position, cam.transform.forward * reach, Color.cyan);

			Ray ray = new(cam.transform.position, cam.transform.forward); //TODO: replace with cameraRig.Forward
			if (!Physics.Raycast(ray, out RaycastHit hit, reach)) { return; }

			canInteract = true;

			if (!Input.GetKey(KeyCode.E)) { return; } //TODO: integrate with InputModules.


			IInteractable interactable = hit.transform.GetComponent<IInteractable>();
			interactable?.Interact(this);
		}

		private void OnGUI()
		{
			GUI.Label(new Rect(10, 10, 500, 100), "Can interact " + (canInteract ? "yes".Color(Color.green) : "no".Color(Color.red)));
		}
	}
}
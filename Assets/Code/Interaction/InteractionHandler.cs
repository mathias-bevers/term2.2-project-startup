using System.Collections.Generic;
using UnityEngine;

namespace Code.Interaction
{
    //Idea: make split up interaction / unclockers. 
    [RequireComponent(typeof(ControlableEntity))]
    public class InteractionHandler : MonoBehaviour
    {
        [SerializeField] InteractableWith interactableWith;
        [SerializeField] private float reach = 3f;

        public Inventory inventory { get; private set; }

        private ControlableEntity controlableEntity;
        private Transform cachedTransform;

        private void Awake()
        {
            cachedTransform = transform;
            controlableEntity = GetComponent<ControlableEntity>();

            if (controlableEntity.GetType() != typeof(Survivor)) { return; }

            inventory = new Inventory();
        }

        private void Update() { InteractionScan(); }

        private void InteractionScan()
        {
            Ray ray = new(controlableEntity.cameraRig.followPoint.position, controlableEntity.cameraRig.forward);
            Debug.DrawRay(ray.origin, ray.direction * reach);
            if (!Physics.Raycast(ray, out RaycastHit hit, reach)) { return; }

            if (!hit.transform.TryGetComponent(out IInteractable interactable)) { return; }

            interactable.OnHover(this);

            if (!controlableEntity.inputModule.OnButtonDown(InputType.ActionButton1)) { return; }

            interactable.Interact(this);
        }
    }
}

[System.Serializable]
public class InteractableWith
{
    [SerializeReference]
    public List<string> canInteractWith = new List<string>();


    public void AddElement(string element)
    {
        if(!Contains(element))
            canInteractWith.Add(element);
    }

    public void RemoveElement(string element)
    {
        if (Contains(element))
            canInteractWith.Remove(element);
    }

    public bool Contains(string element)
    {
        return canInteractWith.Contains(element);
    }
}
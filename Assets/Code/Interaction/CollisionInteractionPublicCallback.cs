using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CollisionInteraction))]
public class CollisionInteractionPublicCallback : MonoBehaviour, ICollisionInteraction
{
    CollisionInteraction _collisionInteraction;
    [SerializeField] LayerMask _layerMask;
    public CollisionInteraction CollisionInteraction { get => _collisionInteraction; set => _collisionInteraction = value; }
    public LayerMask collidesWithLayers { get => _layerMask; set => _layerMask = value; }

    void Awake() => _collisionInteraction = GetComponent<CollisionInteraction>();
    void OnEnable() => _collisionInteraction.RegisterCallback(this);
    void OnDisable() => _collisionInteraction.DeregisterCallback(this);

    public UnityEvent<Collision> _OnCollisionEnter;
    public UnityEvent<Collision> _OnCollisionExit;
    public UnityEvent<Collision> _OnCollisionStay;
    public UnityEvent<Collider> _OnTriggerEnter;
    public UnityEvent<Collider> _OnTriggerExit;
    public UnityEvent<Collider> _OnTriggerStay;

    public void OnCollisionEnter(Collision collision) => _OnCollisionEnter?.Invoke(collision);
    public void OnCollisionExit(Collision collision) => _OnCollisionExit?.Invoke(collision);
    public void OnCollisionStay(Collision collision) => _OnCollisionStay?.Invoke(collision);
    public void OnTriggerEnter(Collider collider) => _OnTriggerEnter?.Invoke(collider);
    public void OnTriggerExit(Collider collider) => _OnTriggerExit?.Invoke(collider);
    public void OnTriggerStay(Collider collider) => _OnTriggerStay?.Invoke(collider);
}

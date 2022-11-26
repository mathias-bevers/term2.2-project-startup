using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICollisionInteraction
{

    CollisionInteraction CollisionInteraction { get; set; }
    LayerMask collidesWithLayers { get; set; }

    public void OnCollisionEnter(Collision collision);
    public void OnCollisionExit(Collision collision);
    public void OnCollisionStay(Collision collision);

    public void OnTriggerEnter(Collider collider);
    public void OnTriggerExit(Collider collider);
    public void OnTriggerStay(Collider collider);
}

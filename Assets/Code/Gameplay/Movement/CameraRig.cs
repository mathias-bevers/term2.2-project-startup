using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CameraRigData))]
public class CameraRig : MonoBehaviour
{
    CameraRigData rigData;

#if UNITY_EDITOR
    [SerializeField] bool drawCollision;
    Vector3 hitPoint;
#endif

    [Header("Point To Follow")]
    public Transform followPoint;

    [Header("Camera Settings")]
    [SerializeField] float maxCamDistance = 10;
    [SerializeField] float minDistance = 3;
    [SerializeField] LayerMask renderInNormal;
    [SerializeField] LayerMask renderWhenClose;

    [Header("Camera Collision Settings")]
    [SerializeField] float collisionSize = 1;
    [SerializeField] LayerMask cameraCollidesWith;


    float currentDistance = 10;

    private void Start()
    {
        rigData= GetComponent<CameraRigData>();
    }

    void Update()
    {
        HandleFollow();

        HandleCamLayers(HandleCamDistance());
    }
    bool HandleCamDistance()
    {
        currentDistance = maxCamDistance;

        rigData.farPoint.localPosition = new Vector3(0, 0, -maxCamDistance);

        Vector3 direction = rigData.farPoint.position - transform.position;
        direction.Normalize();
        Ray ray = new Ray(transform.position, direction);
        RaycastHit hitInfo;
        if (Physics.SphereCast(ray, collisionSize, out hitInfo, maxCamDistance, cameraCollidesWith, QueryTriggerInteraction.UseGlobal))
            currentDistance = hitInfo.distance;
        
        Collider[] colliders = new Collider[10];
        if(Physics.OverlapSphereNonAlloc(transform.position, collisionSize, colliders, cameraCollidesWith, QueryTriggerInteraction.UseGlobal) > 0)
            currentDistance = 0;

        Vector3 hitPoint = transform.position + direction * currentDistance;

#if UNITY_EDITOR
        this.hitPoint = hitPoint;
#endif

        rigData.camera.transform.position = hitPoint;
        return currentDistance < minDistance;
    }

    void HandleCamLayers(bool tooClose = false)
    {
        if (rigData.camera == null) return;

        rigData.camera.cullingMask = tooClose ? renderWhenClose : renderInNormal;
    }

    void HandleFollow()
    {
        if (followPoint == null) return; 
        transform.position = followPoint.position;
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!drawCollision) return;
        if (hitPoint == Vector3.zero) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(hitPoint, collisionSize);
    }
#endif
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CameraRigData))]
public class CameraRig : MonoBehaviour
{
    CameraRigData _rigData;

    public Camera getCamera => _rigData.camera;

#if UNITY_EDITOR
    [SerializeField] bool drawCollision;
    Vector3 hitPoint;
#endif

    [Header("Point To Follow")]
    public Transform followPoint;


    public float setMaxCamDistance { get => maxCamDistance;  set => maxCamDistance = value; }
    [Header("Camera Settings")]
    [SerializeField] float maxCamDistance = 10;
    [SerializeField] float minDistance = 3;
    [SerializeField] LayerMask renderInNormal;
    [SerializeField] LayerMask renderWhenClose;

    [Header("Camera Collision Settings")]
    [SerializeField] float collisionSize = 1;
    [SerializeField] LayerMask cameraCollidesWith;
    [HideInInspector] public Vector3 forward => transform.forward;

    public LayerMask collidesLayerMask { get => cameraCollidesWith; set => cameraCollidesWith = value; }

    float currentDistance = 10;

    public void PassThroughInput(Vector2 cameraInput, MouseSettings mouseSettings)
    {
        Vector2 input = new Vector2(cameraInput.x * mouseSettings.sensitivityX * mouseSettings.invertX, cameraInput.y * mouseSettings.sensitivityY * mouseSettings.invertY) * Time.deltaTime;
        float xRot = transform.rotation.eulerAngles.x;
        if (xRot >= 200 && xRot <= 360 && xRot < 315) if (input.y < 0) input.y = 0;
        if (xRot >= 0 && xRot < 200 && xRot > 80) if (input.y > 0) input.y = 0;
        transform.Rotate(new Vector2(input.y, 0), Space.Self);
        transform.Rotate(new Vector2(0, input.x), Space.World);
    }

    private void Start()
    {
        _rigData= GetComponent<CameraRigData>();
    }

    void Update()
    { 
        HandleFollow();

        HandleCamLayers(HandleCamDistance());
    }
    bool HandleCamDistance()
    {
        currentDistance = maxCamDistance;

        _rigData.farPoint.localPosition = new Vector3(0, 0, -maxCamDistance);

        Vector3 direction = _rigData.farPoint.position - transform.position;
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

        _rigData.cameraHolder.position = hitPoint;
        return currentDistance < minDistance;
    }

    void HandleCamLayers(bool tooClose = false)
    {
        if (_rigData.camera == null) return;

        _rigData.camera.cullingMask = tooClose ? renderWhenClose : renderInNormal;
    }

    public void LookAt(Transform transform)
    {
        this.transform.LookAt(transform);
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

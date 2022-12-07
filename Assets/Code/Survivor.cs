using Code.Interaction;
using UnityEngine;
using Code;
using UnityEngine.Events;

[RequireComponent(typeof(Oxygen))]
[RequireComponent(typeof(InteractionHandler))]
[RequireComponent(typeof(BleedHandler))]
public class Survivor : ControlableEntity
{
    BleedHandler bleed;
    Oxygen oxygen;
    public Oxygen getOxygen { get => oxygen; }
    [SerializeField] Transform _cameraFollowPoint;
    public Transform cameraFollowPoint { get => _cameraFollowPoint; }
    [SerializeField] float _hoverUnderWater = 1;
    public float hoverUnderWater { get => _hoverUnderWater; }
    [SerializeField] float timeForBleed = 8;

    [SerializeField] Transform playerModel;

    public bool targeted = false;
    public bool mainTarget = false;
    public bool eateth = false;

    float oldCamDistance = 0;
    LayerMask oldLayerMask;
    [SerializeField] float smoothingPower = 100;
    [SerializeField] Animator animator;

    InteractionHandler interaction;



    Killer killer = null;

    protected override void OnStart()
    {
        bleed = GetComponent<BleedHandler>();
        bleed.enabled = false;
        oxygen = GetComponent<Oxygen>();
        interaction = GetComponent<InteractionHandler>();
        base.OnStart();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        FindObjectOfType<SurvivorHandler>()?.RegisterSurvivor(this);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        FindObjectOfType<SurvivorHandler>()?.DeregisterSurvivor(this);
    }

    public void SetGrabbedTarget(Killer killer)
    {
        movementModule.enabled = false;
        //inputModule.enabled = false;
        interaction.enabled = false;
        eateth = true;
        oldCamDistance = cameraRig.setMaxCamDistance;
        oldLayerMask = cameraRig.collidesLayerMask;
        cameraRig.collidesLayerMask &= ~(1 << LayerMask.NameToLayer("Player1"));
        cameraRig.setMaxCamDistance = 10;
        bleed.enabled = true;
        getController.enabled = false;
        getInteractionHandler.inventory.Drop<Key>(getTransform.position);
        this.killer = killer;
    }

    public void SetUngrabbedTarget(Killer killer)
    {
        bloodTimer = timeForBleed;
        // bleed.enabled = false;
        movementModule.enabled = true;
        //inputModule.enabled = true;
        interaction.enabled = true;
        eateth = false;
        cameraRig.setMaxCamDistance = oldCamDistance;
        cameraRig.collidesLayerMask = oldLayerMask;
        getTransform.rotation = Quaternion.identity;
        getController.enabled = true;
        
        this.killer = null;
    }

    float bloodTimer = 0;
    float xLevel = 0;
    float smoothXLevel = 0;

    

    protected override void Tick()
    {
        base.Tick();
        if (bloodTimer >= 0)
        {
            bloodTimer -= Time.deltaTime;
            if (bloodTimer <= 0)
            {
                bleed.enabled = false;
            }
        }




       
        xLevel = 0;
        if (getTransform.position.y < WaterHandler.Instance.waterLevel - hoverUnderWater - 1)
        {
            xLevel = Utils.Map(Vector3.Dot(cameraRig.forward, Vector3.up), -1, 1, 179, 0);
        }
        if (xLevel > smoothXLevel)
            smoothXLevel += smoothingPower * Time.deltaTime;
        else if (xLevel < smoothXLevel)
            smoothXLevel -= smoothingPower * Time.deltaTime;

        float animSpeed = movementModule.getSpeed;
        if (animSpeed <= 0.01f)
            animSpeed = 0.01f;
        animator.SetFloat("Speed", animSpeed);

        if (eateth) smoothXLevel = 0;

        playerModel.localRotation = Quaternion.Euler(smoothXLevel, 0, 0);
    }


    protected override void OnDeath()
    {
        base.OnDeath();
    }

}
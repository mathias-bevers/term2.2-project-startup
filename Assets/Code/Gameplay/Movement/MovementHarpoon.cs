using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementPlayer))]
[RequireComponent(typeof(Harpoonable))]

public class MovementHarpoon : MonoBehaviour
{
    [SerializeField] GameObject harpoonPrefab;

    MovementPlayer player;
    Harpoonable _harpoonable;
    [SerializeField] float chargeUpTime = 1.5f;

    CameraMovement cam => player.cameraMovement;
    InputModuleBase input => player.inputModule;
    public Harpoonable harpoonable { get => _harpoonable; }

    float timer = 0;

    bool shot = false;

    GameObject harpoonObject;
    HarpoonInfo info;


    float harpoonTimer = 0;

    private void Start()
    {
        player = GetComponent<MovementPlayer>();
        _harpoonable = GetComponent<Harpoonable>();
    }

    Harpoon lastShotHarpoon = null;

    private void OnDisable()
    {
        timer = 0;
        if (harpoonObject != null)
            harpoonObject.GetComponent<Harpoon>()?.BreakHarpoon();
    }

    private void Update()
    {
        if (HandleOnHarpoon()) return;
        HandleChargeup();
        if (timer < chargeUpTime) return;
        ShootHarpoon();
    }

    void ShootHarpoon()
    {
        if (!Input.GetButtonDown(input.actionButton2)) return;

        shot = true;
        timer = 0;
        harpoonObject = Instantiate(harpoonPrefab, cam.playerCamera.transform.position, cam.playerCamera.transform.rotation);
        lastShotHarpoon = harpoonObject.GetComponent<Harpoon>();
        if (lastShotHarpoon != null)
            lastShotHarpoon.shotFrom = this;
    }

    bool HandleOnHarpoon()
    {
        if (info == null) return false;
        if (info.getType == HarpoonType.Break)
            return BreakHarpoon();

        harpoonTimer += Time.deltaTime;
        if (harpoonTimer > info.entityHarpooned.canSurviveHarpoonTime)
        {
            harpoonTimer = 0;
            info.harpoon.BreakHarpoon();
            return true;
        }
        player.enabled = false;
        player.LookAtProper(info.harpoon.getShootPoint);

        if (Input.GetButtonDown(input.actionButton1))
            return BreakHarpoon();

        if (Vector3.Distance(info.shooterPos, info.targetPos) < 3)
            return BreakHarpoon();

        if (!Input.GetButton(input.actionButton2))
            return true;

        float harpoonStrenth = (info.harpoon.harpoonStrength - info.entityHarpooned.getResistance) * Time.deltaTime;

        bool positive = harpoonStrenth >= 0;

        if (info.getType == HarpoonType.Player)
        {
            MovementPlayer cc = info.entityHarpooned.GetComponent<MovementPlayer>();
            if (cc == null) return true;

            cc.controller.Move(cc.cameraMovement.playerCamera.transform.forward * harpoonStrenth);
            if (cc.transform.position.y > WaterHandler.Instance.waterLevel)
                cc.controller.Move(new Vector3(0, -9.8f * Time.deltaTime, 0));

        }
        else if (info.getType == HarpoonType.Other)
        {
            if (positive)
                info.targetTransform.position = Vector3.MoveTowards(info.targetPos, info.shooterPos, harpoonStrenth);
            else //for now we will break the harpoon
                return BreakHarpoon();
        }
        return true;

    }

    bool BreakHarpoon()
    {
        if (info == null) return false;
        if (info.harpoon == null) return false;
        info.harpoon.BreakHarpoon();

        return true;
    }

    void HandleChargeup()
    {
        cam.zoomInLevel = Utils.Map(timer, 0, chargeUpTime, 0, 1);
        if (!HandleHarpoonMode())
        {
            timer = 0;
            return;
        }


        timer += Time.deltaTime;
        cam.MoveCamera(input.mouseInput * 0.2f);
    }


    public HarpoonInfo OnHarpoonCallback(Harpoonable harpoonable)
    {
        harpoonTimer = 0;
        if (harpoonable == null)
        {
            shot = false;
            timer = 0;
            info = null;
            return null;
        }

        return info = new HarpoonInfo(lastShotHarpoon, this.harpoonable, harpoonable);
    }

    bool HandleHarpoonMode()
    {
        if (!shot) player.enabled = !Input.GetButton(input.actionButton1);
        else player.enabled = true;
        return !player.enabled;
    }
}

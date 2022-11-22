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

        if (Input.GetButtonDown(input.harpoonShoot))
        {
            shot = true;
            timer = 0;
            harpoonObject = Instantiate(harpoonPrefab, cam.playerCamera.transform.position, cam.playerCamera.transform.rotation);
            lastShotHarpoon = harpoonObject.GetComponent<Harpoon>();
            if (lastShotHarpoon != null)
            {
                lastShotHarpoon.shotFrom = this;
            }
            Debug.Log("Shoot!");
        }
    }

    bool HandleOnHarpoon()
    {
        if (info == null) return false;

        harpoonTimer += Time.deltaTime;
        if (harpoonTimer > info.entityHarpooned.canSurviveHarpoonTime)
        {
            harpoonTimer = 0;
            info.harpoon.BreakHarpoon();
            return true;
        }
        player.enabled = false;
        player.LookAtProper(info.harpoon.getShootPoint);

        if (info.entityHarpooned.harpoonType == HarpoonType.Player)
        {
            if (Input.GetButtonDown(input.harpoonInput))
                info.harpoon.BreakHarpoon();
            if (Input.GetButton(input.harpoonShoot))
            {
                MovementPlayer cc = info.entityHarpooned.GetComponent<MovementPlayer>();
                if (cc == null) return true;

                cc.controller.Move(cc.cameraMovement.playerCamera.transform.forward * info.harpoon.harpoonStrength * Time.deltaTime);
                if (cc.transform.position.y > WaterHandler.Instance.waterLevel)
                    cc.controller.Move(new Vector3(0, -9.8f * Time.deltaTime, 0));
                if (Vector3.Distance(info.entityHarpooned.transform.position, info.harpoonShooter.transform.position) < 3)
                    info.harpoon.BreakHarpoon();

            }
        }
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
        if (!shot) player.enabled = !Input.GetButton(input.harpoonInput);
        else player.enabled = true;
        return !player.enabled;
    }
}

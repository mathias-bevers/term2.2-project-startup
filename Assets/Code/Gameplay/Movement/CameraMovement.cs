using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CameraMovement : MonoBehaviour
{
    CharacterController controller;

    [SerializeField] Camera _playerCamera;
    public Camera playerCamera { get => _playerCamera; }

    [SerializeField] float fieldOfViewNormal = 80;
    [SerializeField] float fieldOfViewAimed = 50;
    public float zoomInLevel = 0;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    public void MoveCamera(Vector2 movement)
    {
        movement *= Time.deltaTime * 100;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Vector2 mouse = movement;

        float xRot = playerCamera.transform.rotation.eulerAngles.x;
        if (xRot >= 200 && xRot <= 360 && xRot < 285) if (mouse.y < 0) mouse.y = 0;
        if (xRot >= 0 && xRot < 200 && xRot > 75) if (mouse.y > 0) mouse.y = 0;

        controller.transform.Rotate(new Vector2(0, mouse.x), Space.Self);
        playerCamera.transform.Rotate(new Vector2(mouse.y, 0), Space.Self);
    }

    private void Update()
    {
        playerCamera.fieldOfView = Utils.Map(Mathf.Clamp01(zoomInLevel), 0, 1, fieldOfViewNormal, fieldOfViewAimed);
    }
}

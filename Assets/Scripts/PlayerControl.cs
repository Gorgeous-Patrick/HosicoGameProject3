using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerControl : MonoBehaviour
{
    public UnityEvent<Vector2> OnRotateFlashLight = new UnityEvent<Vector2>();
    public UnityEvent<Vector2> OnMovePlayerCharacter = new UnityEvent<Vector2>();
    public UnityEvent OnMine = new UnityEvent();
    public UnityEvent OnSwapItem = new UnityEvent();
    public UnityEvent OnChangeForm = new UnityEvent();

    private Camera playerCam;
    private bool isBirb = false;
    // Update is called once per frame
    void Start()
    {
        playerCam = Camera.main;
    }

    void Update()
    {
        // get movement vectors: horizontal, vertical
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector2 moveVec = new Vector2(x, y);
        // call OnMoveHullEvent and invoke it passing the movement Vector
        OnMovePlayerCharacter?.Invoke(moveVec.normalized);

        //==============================================================================//
        /*
        Get mouse position in 3D; then get main camera;
        then convert to 2D using nearClipPlane and ScreenToWOrldPoint
        */
        if (playerCam == null)
        {
            Debug.Log("Camera is null; something's wrong");
            return;
        }
        Vector3 MPos = Input.mousePosition;
        MPos.z = playerCam.nearClipPlane;
        Vector2 MPos2D = playerCam.ScreenToWorldPoint(MPos);
        OnRotateFlashLight?.Invoke(MPos2D);

        //==============================================================================//
        if (Input.GetMouseButtonDown(0))
        {
            OnMine?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            OnSwapItem?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            EventBus.Publish(new EventDidPopSmoke());
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            EventBus.Publish(new EventStopTank());
            OnChangeForm?.Invoke();
        }
    }
}

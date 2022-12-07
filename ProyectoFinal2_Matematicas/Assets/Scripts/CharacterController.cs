using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;
public class CharacterController : MonoBehaviour
{


    [Header("MOVEMENT")]

    public float MoveSpeed;
    private Vector2 CurMovementInput;
    public float JumpForce;
    public LayerMask GroundLayerMask;
    public float Runspeed;

    [Header("LOOK")]

    public Transform CameraContainer;
    public float minXLook;
    public float MaxXLook;
    private float CAmCurXrot;
    public float LookSensitivity;
    private Vector2 MouseDelta;


    //componets
    private Rigidbody Rig;
    private void Awake()
    {
        Rig = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void FixedUpdate()
    {
        Move();
    }
    private void LateUpdate()
    {
        CameraLook();
    }
    void Move()
    {
        Vector3 dir = transform.forward * CurMovementInput.y + transform.right * CurMovementInput.x;
        dir *= MoveSpeed;
        dir.y = Rig.velocity.y;
        Rig.velocity = dir;
    }
    void CameraLook()
    {
        CAmCurXrot += MouseDelta.y * LookSensitivity;
        CAmCurXrot = Mathf.Clamp(CAmCurXrot, minXLook, MaxXLook);
        CameraContainer.localEulerAngles = new Vector3(-CAmCurXrot, 0, 0);
        transform.eulerAngles += new Vector3(0, MouseDelta.x * LookSensitivity, 0);
    }
    public void OnlookInput(InputAction.CallbackContext context)
    {
        MouseDelta = context.ReadValue<Vector2>();
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            CurMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            CurMovementInput = Vector2.zero;
        }
    }
    public void OnRunInput(InputAction.CallbackContext context)
    {
        if (MoveSpeed >= 7)
        {
            MoveSpeed = 5f;
        }
        else if (MoveSpeed <= 7)
        {
            MoveSpeed = Runspeed * 2f;

        }

    }
    public void OnJumpIput(InputAction.CallbackContext context)
    {
        Rig.AddForce(new Vector3(0, JumpForce), ForceMode.Impulse);
    }
    /*  bool IsGrounded()
      {
          Ray[] rays = new Ray[4]
          {
              new Ray (transform.position+(transform.forward*.2f)+(Vector3.up*.01f),Vector3.down ),
              new Ray (transform.position+(-transform.forward*.2f)+(Vector3.up*.01f),Vector3.down ),
              new Ray (transform.position+(transform.right*.2f)+(Vector3.up*.01f),Vector3.down ),
              new Ray (transform.position+(-transform.right*.2f)+(Vector3.up*.01f),Vector3.down ),
          };
          for (int i = 0; i < rays.Length; i++)
          {
              if (Physics.Raycast(rays[i], 0.1f, GroundLayerMask))
              {
                  return true;
              }

          }

          return false;
      }
      private void OnDrawGizmos()
      {
          Gizmos.color = Color.red;
          Gizmos.DrawRay(transform.position + (transform.forward * .2f), Vector3.down);
          Gizmos.DrawRay(transform.position + (-transform.forward * .2f), Vector3.down);
          Gizmos.DrawRay(transform.position + (transform.right * .2f), Vector3.down);
          Gizmos.DrawRay(transform.position + (-transform.right * .2f), Vector3.down);
      }*/
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private CharacterController player_ChaCon = null;
    private bool is_Grounded = true;
    public bool IsGrounded { get{ return is_Grounded; }}
    private bool is_Moving = false;
    public bool IsMoving { get { return is_Moving; } }

    [SerializeField] string horizontal_InputName = "", vertical_InputName = "";
    private float movement_Speed = 0.0f;
    [SerializeField] private float speed_Walk = 1.0f , speed_Run = 2.0f;
    [SerializeField] private float speed_BuildUp = 1.0f;
    [SerializeField] private KeyCode run_Key = KeyCode.None;

    [SerializeField] private AnimationCurve jump_FallOff = null;
    [SerializeField] private float jump_Multiplyer = 1.0f;
    [SerializeField] private KeyCode jump_Key = KeyCode.None;

    [SerializeField] private float slope_Force = 10.0f;
    [SerializeField] private float slope_Force_RayLength = 0.0f;

    void Awake()
    {
        player_ChaCon = GetComponent<CharacterController>();
    }

    void Update()
    {
        PlayerMovement();
    }

    private void PlayerMovement()
    {
        float horizontal_Input = Input.GetAxis(horizontal_InputName);
        float vertical_Input = Input.GetAxis(vertical_InputName);

        Vector3 forwardMovement = transform.forward * vertical_Input;
        Vector3 sidewaysMovement = transform.right * horizontal_Input;

        player_ChaCon.SimpleMove(Vector3.ClampMagnitude(forwardMovement + sidewaysMovement, 1.0f) * movement_Speed);

        if (horizontal_Input != 0.0f || vertical_Input != 0.0f)
            is_Moving = true;
        else
            is_Moving = false;

        if((horizontal_Input != 0.0f || vertical_Input != 0.0f)  &&  OnSlope())
        {
            player_ChaCon.Move(Vector3.down * player_ChaCon.height / 2.0f * slope_Force);
        }

        SetMovementSpeed();
        JumpInput();
    }

    private void SetMovementSpeed()
    {
        if(Input.GetKey(run_Key))
        {
            movement_Speed = Mathf.Lerp(movement_Speed, speed_Run, speed_BuildUp * Time.deltaTime);
        }
        else
        {
            movement_Speed = Mathf.Lerp(movement_Speed, speed_Walk, speed_BuildUp * Time.deltaTime);
        }
    }

    private void JumpInput()
    {
        if(Input.GetKeyDown(jump_Key) && is_Grounded)
        {
            is_Grounded = false;
            StartCoroutine(JumpEvent());
        }
    }

    private IEnumerator JumpEvent()
    {
        player_ChaCon.slopeLimit = 90.0f;
        float time = 0.0f;

        do
        {
            float jumpForce = jump_FallOff.Evaluate(time);
            player_ChaCon.Move(Vector3.up * jumpForce * jump_Multiplyer * Time.deltaTime);

            time += Time.deltaTime;

            yield return null;
        }
        while (!player_ChaCon.isGrounded && player_ChaCon.collisionFlags != CollisionFlags.Above);

        player_ChaCon.slopeLimit = 45.0f;
        is_Grounded = true;
    }

    private bool OnSlope()
    {
        if(!is_Grounded)
        {
            return false;
        }

        RaycastHit hit;

        if(Physics.Raycast(transform.position , Vector3.down , out hit , player_ChaCon.height/2.0f * slope_Force_RayLength))
        {
            if(hit.normal != Vector3.up)
            {
                return true;
            }
        }

        return false;
    }
}

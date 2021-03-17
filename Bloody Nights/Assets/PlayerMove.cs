using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] string horizontal_InputName = "", vertical_InputName = "";
    [SerializeField] float movement_Speed = 10.0f;

    private CharacterController player_ChaCon = null;

    private bool is_Grounded = true;

    [SerializeField] private AnimationCurve jump_FallOff = null;
    [SerializeField] private float jump_Multiplyer = 1.0f;
    [SerializeField] private KeyCode jump_Key = KeyCode.None;

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
        float horizontal_Input = Input.GetAxis(horizontal_InputName) * movement_Speed;
        float vertical_Input = Input.GetAxis(vertical_InputName) * movement_Speed;

        Vector3 forwardMovement = transform.forward * vertical_Input;
        Vector3 sidewaysMovement = transform.right * horizontal_Input;

        player_ChaCon.SimpleMove(forwardMovement + sidewaysMovement);

        JumpInput();
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
}

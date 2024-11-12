using Fusion;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    // 참고한 코드 - 집게사장의 꿈 님 : https://krapboss.tistory.com/61

    [SerializeField] CharacterController controller;
    [Space(10)]
    [SerializeField] float moveSpeed;
    [Space(10)]
    [SerializableType] bool isJumped;
    [SerializeField] float jumpSpeed;
    [SerializeField] float ogStepOffset;

    private float ySpeed;
    private Vector3 inputDir;

    private void Update()
    {
        inputDir.x = Input.GetAxisRaw("Horizontal");
        inputDir.z = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Space)) { isJumped = true; }
    }


    public override void FixedUpdateNetwork()
    {
        if (HasStateAuthority == false) return;
        if (controller.isGrounded) { inputDir.y = ogStepOffset; ySpeed = 0f; }
        if (inputDir == Vector3.zero) return;

        Vector3 move = new Vector3(inputDir.x, 0, inputDir.z) * Runner.DeltaTime * moveSpeed;

        //inputDir.y = Physics.gravity.y * Runner.DeltaTime;
        ySpeed += Physics.gravity.y * Runner.DeltaTime;

        if (isJumped && controller.isGrounded)
        {
            Debug.Log("점프함");
            //inputDir.y += jumpSpeed;
            ySpeed += jumpSpeed;
        }
        isJumped = false;
        // +
        inputDir.y = ySpeed;

        Debug.Log("움직임");
        controller.Move(move + inputDir * Runner.DeltaTime);
        transform.forward = move;
    }


    public override void Spawned()
    {
        isJumped = false;
        ogStepOffset = controller.stepOffset;

        if (HasStateAuthority == true)
        {
            CameraController camController = Camera.main.GetComponent<CameraController>();
            camController.target = transform;
        }
    }
}

using UnityEngine;
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour{
    private CharacterController controller;
    [Header("Movement Settings")]
    public float speed = 4f;
    public float acceleration = 10f;
    [Header("Gravity Settings")]
    public float gravity = -9.81f;
    private Vector3 currentVelocity;
    private float verticalVelocity;
    void Start(){
        controller = GetComponent<CharacterController>();
    }
    void Update(){
        float horizontal = 0f;
        float vertical = 0f;
        if (Input.GetKey(KeyCode.Q)) horizontal = -1f;
        if (Input.GetKey(KeyCode.D)) horizontal = 1f;
        if (Input.GetKey(KeyCode.Z)) vertical = 1f;
        if (Input.GetKey(KeyCode.S)) vertical = -1f;
        Vector3 direction = new Vector3(horizontal, 0f, vertical);
        direction = Vector3.ClampMagnitude(direction, 1f);
        Vector3 targetVelocity = direction * speed;
        currentVelocity = Vector3.MoveTowards(
            currentVelocity,
            targetVelocity,
            acceleration * Time.deltaTime
        );
        if (controller.isGrounded && verticalVelocity < 0)
            verticalVelocity = -2f;

        verticalVelocity += gravity * Time.deltaTime;
        Vector3 movement = new Vector3(
            currentVelocity.x,
            verticalVelocity,
            currentVelocity.z
        );
        controller.Move(movement * Time.deltaTime);
    }
}

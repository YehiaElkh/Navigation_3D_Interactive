using UnityEngine;
public class MouseLook : MonoBehaviour{
    [Header("Camera Reference")]
    public Transform cameraPivot;
    [Header("Mouse Settings")]
    public float mouseSensitivity = 150f;
    public float smoothTime = 0.05f;
    [Header("Camera Limits")]
    public float minVerticalAngle = -45f;
    public float maxVerticalAngle = 45f;
    private float yaw;
    private float pitch;
    private float yawVelocity;
    private float pitchVelocity;
    void Start(){
        yaw = transform.eulerAngles.y;
        pitch = cameraPivot.localEulerAngles.x;
        // Verrouiller le curseur au centre de l'écran
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void Update(){
        // Récupérer les mouvements de la souris
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        // Calculer les rotations
        yaw += mouseX;
        pitch -= mouseY;
        // Limiter la rotation verticale
        pitch = Mathf.Clamp(pitch, minVerticalAngle, maxVerticalAngle);
        // Appliquer un mouvement fluide
        float smoothYaw = Mathf.SmoothDampAngle(
            transform.eulerAngles.y,
            yaw,
            ref yawVelocity,
            smoothTime
        );
        float smoothPitch = Mathf.SmoothDamp(
            cameraPivot.localEulerAngles.x > 180f
                ? cameraPivot.localEulerAngles.x - 360f
                : cameraPivot.localEulerAngles.x,
            pitch,
            ref pitchVelocity,
            smoothTime
        );
        // Appliquer les rotations
        transform.rotation = Quaternion.Euler(0f, smoothYaw, 0f);
        cameraPivot.localRotation = Quaternion.Euler(smoothPitch, 0f, 0f);
        // Libérer le curseur avec Escape
        if (Input.GetKeyDown(KeyCode.Escape)){
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // Reverrouiller le curseur avec un clic gauche
        if (Input.GetMouseButtonDown(0)){
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}

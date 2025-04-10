using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    private float rotationSpeedPlayer = 10f;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        Vector3 cameraForward = this.transform.forward;

        // Remove any vertical component
        cameraForward.y = 0f;
        cameraForward.Normalize();

        if (cameraForward.magnitude > 0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(cameraForward);
            playerObj.rotation = Quaternion.Lerp(playerObj.rotation, targetRotation, rotationSpeedPlayer * Time.deltaTime);
            orientation.rotation = Quaternion.Lerp(orientation.rotation, targetRotation, rotationSpeedPlayer * Time.deltaTime);
        }
    }
}

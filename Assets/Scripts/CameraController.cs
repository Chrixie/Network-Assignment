using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class CameraController : NetworkBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientation;

    float xRotation;
    float yRotation;

    public GameObject cameraHolder;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void Update()
    {
        if (SceneManager.GetActiveScene().name == "SampleScene")
        {
            cameraHolder.transform.position = transform.position;
            CameraLookAround();
        }
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        cameraHolder.SetActive(IsOwner);
    }


    private void CameraLookAround()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }



}

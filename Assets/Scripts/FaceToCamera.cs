using UnityEngine;

public class FaceToCamera : MonoBehaviour
{
    private void Update()
    {
        FaceCamera();
    }

    private void FaceCamera()
    {
        Vector3 directionToCamera = Camera.main.transform.position - transform.position;
        directionToCamera.y = 0;
        transform.rotation = Quaternion.LookRotation(directionToCamera);
    }
}

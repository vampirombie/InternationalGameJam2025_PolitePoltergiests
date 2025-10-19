using Unity.Cinemachine;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    [Header("Lista de cámaras Cinemachine")]
    public CinemachineCamera[] cameras;

    private int currentCameraIndex = 0;

    void Start()
    {
        // Activar solo la primera cámara al inicio
        SwitchCamera(currentCameraIndex);
    }

    void Update()
    {
        //// Avanzar con tecla D
        //if (Input.GetKeyDown(KeyCode.D))
        //{
        //    NextCamera();
        //}

        //// Retroceder con tecla A
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    PreviousCamera();
        //}
    }

    // Método para avanzar a la siguiente cámara
    public void NextCamera()
    {
        currentCameraIndex++;
        if (currentCameraIndex >= cameras.Length)
            currentCameraIndex = 0;

        SwitchCamera(currentCameraIndex);
    }

    // Método para retroceder a la cámara anterior
    public void PreviousCamera()
    {
        currentCameraIndex--;
        if (currentCameraIndex < 0)
            currentCameraIndex = cameras.Length - 1;

        SwitchCamera(currentCameraIndex);
    }

    // Activar la cámara actual y desactivar las demás
    private void SwitchCamera(int index)
    {
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].gameObject.SetActive(i == index);
        }

        Debug.Log($"Cámara activa: {cameras[index].name}");
    }
}

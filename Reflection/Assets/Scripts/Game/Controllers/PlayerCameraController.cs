using Reflection.Utils;
using UnityEngine;

namespace Reflection.Controllers
{

    internal class PlayerCameraController : MonoBehaviour
    {


        [SerializeField] private Transform playerBodyTr; // TODO: no need for direct inspector reference
        [SerializeField] private float sensitivity;

        private float upDownRotation;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked; // TODO: shouldn't be here
        }

        private void Update()
        {
            var lookInput =
                new Vector2
                (
                    Input.GetAxisRaw(InputUtils.AxisName.MouseX), 
                    Input.GetAxisRaw(InputUtils.AxisName.MouseY)
                )
                * sensitivity
                * Time.deltaTime;

            upDownRotation = Mathf.Clamp(upDownRotation - lookInput.y, -90f, 90f);
            transform.localRotation = Quaternion.Euler(upDownRotation, 0f, 0f);
            
            playerBodyTr.Rotate(Vector3.up, lookInput.x, Space.World);
        }

    }

}
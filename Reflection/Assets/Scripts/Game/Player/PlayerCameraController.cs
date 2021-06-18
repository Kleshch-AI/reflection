using Reflection.Utils;
using UnityEngine;

namespace Reflection.Game.Player
{

    internal class PlayerCameraController : MonoBehaviour
    {

        [SerializeField] private Transform playerBodyTr;
        [SerializeField] private PlayerSettings settings;

        private float upDownRotation;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            var lookInput =
                new Vector2
                (
                    Input.GetAxisRaw(InputUtils.AxisName.MouseX), 
                    Input.GetAxisRaw(InputUtils.AxisName.MouseY)
                )
                * settings.Sensitivity
                * Time.deltaTime;

            upDownRotation = Mathf.Clamp(upDownRotation - lookInput.y, -90f, 90f);
            transform.localRotation = Quaternion.Euler(upDownRotation, 0f, 0f);
            
            playerBodyTr.Rotate(Vector3.up, lookInput.x, Space.World);
        }

    }

}
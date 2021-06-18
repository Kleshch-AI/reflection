using Reflection.Utils;
using UnityEngine;
using UniRx;
using System.Collections;
using DG.Tweening;

namespace Reflection.Game.Player
{

    internal class PlayerCameraController : MonoBehaviour
    {

        [SerializeField] private Transform playerBodyTr;
        [SerializeField] private Transform playerHeadTr;
        [SerializeField] private PlayerSettings settings;

        private float upDownRotation;
        private Sequence stepSequence;
        private int tilt = 1;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;

            // PlayerContext.MoveType.Subscribe(OnMoveTypeChange).AddTo(this);
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

        // private void OnMoveTypeChange(MoveType moveType)
        // {
        //     switch (moveType)
        //     {
        //         case MoveType.None: Stop(); break;
        //         case MoveType.Walking: StartCoroutine(Tilt(settings.WalkingStepSettings)); break;
        //         case MoveType.Running: StartCoroutine(Tilt(settings.RunningStepSettings)); break;
        //     }
        // }

        // private IEnumerator Tilt(PlayerSettings.StepSettings s)
        // {
        //     tilt = -tilt;

        //     var startRot = playerHeadTr.rotation.eulerAngles.z;
        //     var endRot = s.HeadRotation * tilt;

        //     while()
        // }

        // private void Tilt(PlayerSettings.StepSettings s)
        // {


        //     // playerHeadTr
        //     //     .DOLocalRotate(new Vector3(0, 0, s.HeadRotation * tilt), s.StepDuration)
        //     //     .SetLoops(-1)
        //     //     .SetTarget(this);


        //     stepSequence = DOTween.Sequence()
        //         .Append(playerHeadTr.DOLocalRotate(new Vector3(0, 0, s.HeadRotation * 2f * tilt), s.StepDuration))
        //         .OnStepComplete(() => tilt = -tilt)
        //         .SetLoops(-1)
        //         .SetTarget(this);

        //     playerHeadTr.DOLocalRotate(new Vector3(0, 0, s.HeadRotation * -tilt), s.StepDuration)
        //         .OnComplete(() => stepSequence.Play())
        //         .SetTarget(this);
        //     // tilt = -tilt;

        //     // await s.StepDuration;
        //     // stepSequence.Play().SetDelay(s.StepDuration);

        // }

        private void Stop()
        {
            // stepSequence.Kill(true);
            StopAllCoroutines();
        }


    }

}
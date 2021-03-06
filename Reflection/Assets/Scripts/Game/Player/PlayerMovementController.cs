using UnityEngine;
using Reflection.Utils;
using System.Collections;

namespace Reflection.Game.Player
{

    internal class PlayerMovementController : MonoBehaviour
    {

        [SerializeField] private CharacterController cc;
        [SerializeField] private Transform groundCheck;
        [SerializeField] private PlayerSettings settings;

        private bool isMoving;
        private float stepStartTime;
        private float stepDuration;
        private float jumpPrepareTime;
        private bool isReadyToJump;
        private Vector3 direction;

        private bool isGrounded => Physics.CheckSphere(groundCheck.position, settings.GroundDistance,
            settings.GroundLayerMask, QueryTriggerInteraction.Ignore);

        private void Update()
        {
            if (Input.GetButtonDown(InputUtils.ButtonName.Jump))
            {
                PrepareJump();
            }

            if (Input.GetButtonUp(InputUtils.ButtonName.Jump))
            {
                TryJump();
            }

            if (isMoving) return;

            if (!isGrounded)
            {
                StopAllCoroutines();
                StartCoroutine(Fall());
                return;
            }

            var motionInput = new Vector2
                (
                    Input.GetAxisRaw(InputUtils.AxisName.Horizontal),
                    Input.GetAxisRaw(InputUtils.AxisName.Vertical)
                );

            if (motionInput.x != 0 || motionInput.y != 0)
            {
                if (Input.GetKey(KeyCode.LeftShift)) PlayerContext.MoveType.Value = MoveType.Running;
                else PlayerContext.MoveType.Value = MoveType.Walking;

                direction = motionInput.x * transform.right + motionInput.y * transform.forward;
                StartCoroutine(TakeStep());
            }
            else
            {
                PlayerContext.MoveType.Value = MoveType.None;
            }
        }

        private IEnumerator TakeStep()
        {
            if (PlayerContext.MoveType.Value == MoveType.None) yield break;

            isMoving = true;

            var stepSettings = PlayerContext.MoveType.Value == MoveType.Walking ? settings.WalkingStepSettings : settings.RunningStepSettings;

            var randomSign = Random.Range(0, 2) * 2 - 1;
            stepDuration = stepSettings.StepDuration + randomSign * stepSettings.StepDuration * stepSettings.StepRandomPercent;
            stepStartTime = Time.time;

            var position = transform.position;
            var endPostion = position + direction * stepSettings.StepLength;
            while (Time.time - stepStartTime < stepDuration)
            {
                var timeFrac = (Time.time - stepStartTime) / stepDuration;
                var percent = stepSettings.StepEasing.Evaluate(timeFrac);

                var newPosition = Vector3.Lerp(position, endPostion, percent);
                var movement = newPosition - transform.position;
                cc.Move(movement);

                yield return new WaitForEndOfFrame();
            }

            isMoving = false;
        }

        private IEnumerator Fall()
        {
            isMoving = true;

            PlayerContext.MoveType.Value = MoveType.Falling;

            var fallVelocity = Vector3.zero;
            while (!isGrounded)
            {
                fallVelocity.y -= settings.Gravity * Time.deltaTime;
                cc.Move(fallVelocity);

                yield return new WaitForEndOfFrame();
            }

            isMoving = false;
        }

        private void PrepareJump()
        {
            isReadyToJump = PlayerContext.MoveType.Value == MoveType.Walking || PlayerContext.MoveType.Value == MoveType.Running;

            if (isReadyToJump) jumpPrepareTime = Time.time;
        }

        private void TryJump()
        {
            if (!isReadyToJump) return;

            if (!isGrounded) return;

            var jumpWaitTime = Time.time - jumpPrepareTime;
            var stepDuration = PlayerContext.MoveType.Value == MoveType.Walking ? settings.WalkingStepSettings.StepDuration :
                settings.RunningStepSettings.StepDuration;

            var jumpForce = jumpWaitTime / stepDuration;
            if (jumpForce > 1) return;

            StopAllCoroutines();

            StartCoroutine(Jump(jumpForce, stepDuration));
        }

        private IEnumerator Jump(float jumpForce, float stepDuration)
        {
            isMoving = true;

            var risePos = transform.position;
            var setPos = transform.position + direction * jumpForce * settings.JumpLength;
            var center = (risePos + setPos) * 0.5f;

            var startTime = Time.time;
            var jumpDuration = stepDuration * 2f * jumpForce;

            while (Time.time - startTime < jumpDuration)
            {
                center -= new Vector3(0, 1f / settings.JumpHeight, 0);
                var relativeRisePos = risePos - center;
                var relativeSetPos = setPos - center;
                var timeFrac = (Time.time - startTime) / jumpDuration;
                var newPos = Vector3.Slerp(relativeRisePos, relativeSetPos, timeFrac);

                var movement = newPos + center - transform.position;
                cc.Move(movement);

                yield return new WaitForEndOfFrame();
            }

            isMoving = false;
        }

    }

}
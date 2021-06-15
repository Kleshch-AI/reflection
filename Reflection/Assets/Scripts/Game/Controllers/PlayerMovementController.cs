using UnityEngine;
using Reflection.Utils;
using System.Collections;

namespace Reflection.Controllers
{

    public class PlayerMovementController : MonoBehaviour
    {

        [System.Serializable]
        public struct StepSettings
        {
            [SerializeField] private float stepLength;
            [SerializeField] private float stepDuration;
            [SerializeField] [Range(0f, 1f)] private float stepRandomPercent;
            [SerializeField] private AnimationCurve stepEasing;

            public float StepLength => stepLength;
            public float StepDuration => stepDuration;
            public float StepRandomPercent => stepRandomPercent;
            public AnimationCurve StepEasing => stepEasing;
        }

        public enum MoveType
        {
            None,
            Walking,
            Running,
            Falling
        }

        [SerializeField] private CharacterController cc;

        [Header("Walking Step")]
        [SerializeField] StepSettings walkingStepSettings;

        [Header("Running Step")]
        [SerializeField] StepSettings runningStepSettings;

        [Header("Jump")]
        [SerializeField] private float jumpHeight;
        [SerializeField] private float jumpLength;

        [Header("Physics")]
        [SerializeField] private float gravity;
        [SerializeField] private float groundDistance;
        [SerializeField] private LayerMask groundLayerMask;
        [SerializeField] private Transform groundCheckTransform;

        private bool isMoving;
        private MoveType moveType;
        private float stepStartTime;
        private float stepDuration;
        private float jumpPrepareTime;
        private bool isReadyToJump;
        private Vector3 direction;

        private bool isGrounded => Physics.CheckSphere(groundCheckTransform.position, groundDistance,
            groundLayerMask, QueryTriggerInteraction.Ignore);

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
                if (Input.GetKey(KeyCode.LeftShift)) moveType = MoveType.Running;
                else moveType = MoveType.Walking;

                direction = motionInput.x * transform.right + motionInput.y * transform.forward;
                StartCoroutine(TakeStep());
            }
            else
            {
                moveType = MoveType.None;
            }
        }

        private IEnumerator TakeStep()
        {
            if (moveType == MoveType.None) yield break;

            isMoving = true;

            var stepSettings = moveType == MoveType.Walking ? walkingStepSettings : runningStepSettings;

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

            moveType = MoveType.Falling;

            var fallVelocity = Vector3.zero;
            while (!isGrounded)
            {
                fallVelocity.y -= gravity * Time.deltaTime;
                cc.Move(fallVelocity);

                yield return new WaitForEndOfFrame();
            }

            isMoving = false;
        }

        private void PrepareJump()
        {
            isReadyToJump = moveType == MoveType.Walking || moveType == MoveType.Running;

            if (isReadyToJump) jumpPrepareTime = Time.time;
        }

        private void TryJump()
        {
            if (!isReadyToJump) return;

            if (!isGrounded) return;

            var jumpWaitTime = Time.time - jumpPrepareTime;
            var stepDuration = moveType == MoveType.Walking ? walkingStepSettings.StepDuration :
                runningStepSettings.StepDuration;

            var jumpForce = jumpWaitTime / stepDuration;
            if (jumpForce > 1) return;

            StopAllCoroutines();

            StartCoroutine(Jump(jumpForce, stepDuration));
        }

        private IEnumerator Jump(float jumpForce, float stepDuration)
        {
            isMoving = true;

            var risePos = transform.position;
            var setPos = transform.position + direction * jumpForce * jumpLength;
            var center = (risePos + setPos) * 0.5f;

            var startTime = Time.time;
            var jumpDuration = stepDuration * 2f * jumpForce;

            while (Time.time - startTime < jumpDuration)
            {
                center -= new Vector3(0, 1f / jumpHeight, 0);
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
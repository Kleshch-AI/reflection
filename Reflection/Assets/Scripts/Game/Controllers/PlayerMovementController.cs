using UnityEngine;
using Reflection.Utils;
using DG.Tweening;
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
            Running
        }

        [SerializeField] private CharacterController cc;

        [Header("Walking Step")]
        [SerializeField] StepSettings walkingStepSettings;

        [Header("Running Step")]
        [SerializeField] StepSettings runningStepSettings;

        private bool isMoving;
        private MoveType moveType;
        private float stepStartTime;
        private float stepDuration;

        private void Update()
        {
            if (isMoving) return;

            var motionInput = new Vector2
                (
                    Input.GetAxisRaw(InputUtils.AxisName.Horizontal),
                    Input.GetAxisRaw(InputUtils.AxisName.Vertical)
                );

            if (motionInput.x != 0 || motionInput.y != 0)
            {
                if (Input.GetKey(KeyCode.LeftShift)) moveType = MoveType.Running;
                else moveType = MoveType.Walking;

                StartCoroutine(TakeStep(motionInput.x * transform.right + motionInput.y * transform.forward));
            }
            else
            {
                moveType = MoveType.None;
            }
        }

        private IEnumerator TakeStep(Vector3 direction)
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

    }

}
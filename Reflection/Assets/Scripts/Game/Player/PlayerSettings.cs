using UnityEngine;

namespace Reflection.Game.Player
{

    internal class PlayerSettings : MonoBehaviour
    {

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

        [Header("Camera")]
        [SerializeField] private float sensitivity = 1000;


        internal StepSettings WalkingStepSettings => walkingStepSettings;

        internal StepSettings RunningStepSettings => runningStepSettings;

        internal float JumpHeight => jumpHeight;
        internal float JumpLength => jumpLength;

        internal float Gravity => gravity;
        internal float GroundDistance => groundDistance;
        internal LayerMask GroundLayerMask => groundLayerMask;

        internal float Sensitivity => sensitivity;


        [System.Serializable]
        internal struct StepSettings
        {
            [SerializeField] private float stepLength;
            [SerializeField] private float stepDuration;
            [SerializeField] [Range(0f, 1f)] private float stepRandomPercent;
            [SerializeField] private AnimationCurve stepEasing;
            [SerializeField] private float headRotation;

            public float StepLength => stepLength;
            public float StepDuration => stepDuration;
            public float StepRandomPercent => stepRandomPercent;
            public AnimationCurve StepEasing => stepEasing;
            public float HeadRotation => headRotation;
        }

    }

}
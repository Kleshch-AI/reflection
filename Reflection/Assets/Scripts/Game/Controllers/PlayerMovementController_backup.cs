using UnityEngine;
using Reflection.Utils;

namespace Reflection.Controllers
{

    public class PlayerMovementController_backup : MonoBehaviour
    {

        [SerializeField] private CharacterController cc;
        [SerializeField] private const float gravity = 9.81f;
        [SerializeField] private float speed;
        [SerializeField] private float maxSpeed;
        [SerializeField] private float groundDistance;
        [SerializeField] private float jumpHeight;
        [SerializeField] private Transform groundCheck;
        [SerializeField] private LayerMask groundMask;
         [SerializeField] private float stopDuration;
         [SerializeField] private float startDuration;


        // private float fallVelocity;
        private Vector3 velocity;
        private Vector3 movement;
        private float stopTime;
        private float currentSpeed;
        private bool isMoving;
        private float startTime;

        private bool IsGrounded => Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        private void Update()
        {
            var motionInput = new Vector2
                (
                    Input.GetAxisRaw(InputUtils.AxisName.Horizontal),
                    Input.GetAxisRaw(InputUtils.AxisName.Vertical)
                );
            // var localMotion = motionInput.y * transform.forward ;
            //+ motionInput.x * transform.right;



            if (motionInput.sqrMagnitude != 0f)
            {
                if (!isMoving) 
                {
                    startTime = Time.time;
                    isMoving = true;
                }
                currentSpeed = Mathf.Lerp(currentSpeed, maxSpeed, (Time.time - startTime) / startDuration);
                movement = motionInput.y * transform.forward + motionInput.x * transform.right;

                // currentSpeed+=maxSpeed * Time.deltaTime;
                // velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
                // stopTime = 0f;
            }
            else if (currentSpeed > 0.1f)
            {
                if (isMoving)
                {
                    stopTime = Time.time;
                    isMoving = false;
                }
                // if (stopTime <= 0f) stopTime = Time.time;
                currentSpeed = Mathf.Lerp(currentSpeed, 0f, (Time.time - stopTime) / stopDuration);
                // currentSpeed-=maxSpeed * Time.deltaTime;


                // velocity = movement * currentSpeed;
            
                // velocity = Vector3.Lerp(velocity, Vector3.zero, (Time.time - stopTime) / stopDuration);
                // velocity = Vector3.zero;
            }

                currentSpeed = (currentSpeed <= 0.1f) ? 0f : Mathf.Clamp(currentSpeed, 0f, maxSpeed);

                velocity = movement * currentSpeed * Time.deltaTime;


            // if (localMotion.z != 0f)
            // {
            //     velocity.z += localMotion.z * speed * Time.deltaTime;
            //     velocity.z = Mathf.Clamp(velocity.z, -maxSpeed, maxSpeed);
            //     stopTime = 0f;
            // }
            // else if (velocity.z > 0f)
            // {
            //     if (stopTime <= 0f) stopTime = Time.time;
            //     velocity.z = Mathf.Lerp(velocity.z, 0f, (Time.time - stopTime) / 1.5f);
            // }

            Debug.Log($"{currentSpeed}");
            // velocity += (motionInput.y * transform.forward + motionInput.x * transform.right) * speed * Time.deltaTime;
            // velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

            // if (IsGrounded)
            // {
            //     // Debug.Log("grounded");
            //     if (Input.GetButtonDown("Jump"))
            //     {
            //         // Debug.Log("jump");
            //         // fallVelocity = Mathf.Sqrt(jumpHeight * 2f * gravity);
            //         fallVelocity = jumpHeight;
            //     }
            //     else
            //         fallVelocity = 0;
            // }
            // else
            // {
            //     fallVelocity -= gravity;
            // }
            // // Debug.Log($"{velocity.y}");

            // velocity.y = fallVelocity;
            cc.Move(velocity);
        }

    }

}
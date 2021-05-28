using UnityEngine;

namespace Reflection.Controllers
{

    public class MovementTracker : MonoBehaviour
    {

        [SerializeField] private Transform trackedObject;
        [SerializeField] private int updateSpeed;
        [SerializeField] private Vector2 offset2D;

        private Vector3 offset;
        
        private void Start()
        {
            offset = offset2D;
            offset.z = transform.position.z - trackedObject.position.z;
        }

        private void LateUpdate()
        {
            transform.position = Vector3.MoveTowards
            (
                transform.position, 
                trackedObject.position + offset, 
                updateSpeed * Time.deltaTime
            );
        }

    }

}
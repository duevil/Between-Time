using UnityEngine;

namespace BetweenTime._Scripts
{
    /// <summary>
    ///     Simple script to rotate an object on a randomly generated orbit path set by a given radius and speed
    ///     and based on the object's initial position and rotation
    /// </summary>
    public class Orbit : MonoBehaviour
    {
        [Tooltip("The speed of the orbit")] [SerializeField] [Range(0.1f, 10f)]
        private float speed;

        [Tooltip("The radius of the orbit, i.e. the distance from the center")] [SerializeField] [Range(1f, 100f)]
        private float radius;

        private GameObject _parent; // Newly created parent object to rotate around
        private int direction; // Randomize the direction of the orbit

        /// <summary>
        ///     Initializes the orbit path;
        ///     creates a parent object to rotate around and sets the object's position to be on the orbit path
        ///     and the object's rotation to be at a random starting point
        /// </summary>
        private void Start()
        {
            gameObject.SetActive(true); // make sure the object is active

            // Randomly set whether the object should orbit clockwise or counterclockwise
            direction = Random.value < 0.5f ? 1 : -1;

            // slightly randomize the object initial rotation to create randomized orbit paths
            transform.rotation *= Random.rotationUniform;

            // create a parent object to rotate around
            _parent = new GameObject($"{gameObject.name} OrbitParent")
            {
                transform =
                {
                    parent = transform.parent,
                    position = transform.position,
                    rotation = transform.rotation
                }
            };
            transform.SetParent(_parent.transform);

            // set the object's position to be on the orbit path
            transform.localPosition = Vector3.right * radius;
            // set the object's rotation to be at random starting point
            _parent.transform.Rotate(Vector3.forward, Random.Range(0, 360));
        }

        /// <summary>
        ///     Update the orbit path
        /// </summary>
        private void Update()
        {
            var angle = Time.deltaTime * speed * direction;
            // rotate the parent object around the center
            _parent.transform.Rotate(Vector3.forward, angle);
            // slightly rotate orbit path to create more interesting orbits
            _parent.transform.Rotate(Vector3.up, angle * 0.1f);
            _parent.transform.Rotate(Vector3.right, angle * 0.1f);
            // slightly rotate the object itself to have a more dynamic effect
            transform.Rotate(Vector3.up, angle * 0.5f);
        }
    }
}
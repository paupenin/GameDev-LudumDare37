using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour {

    private Rigidbody rb;

    [SerializeField]
    private Camera cam;

	[SerializeField]
	private float jumpForce = 5;

    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private Vector3 cameraRotation = Vector3.zero;

    // 
    void Start()
    {
        // Load Motor
        rb = GetComponent<Rigidbody>();

    }

    // 
    void FixedUpdate()
    {
        Move();

        Rotate();
    }


    // Set direction
    public void SetDirection(Vector3 _velocity)
    {
        velocity = _velocity;
    }

    // Perform Movement
    void Move()
    {
        if(velocity != Vector3.zero)
        {
            rb.MovePosition(transform.position + velocity * Time.fixedDeltaTime);
        }

    }

    // Sets Orientation
    public void SetRotation(Vector3 _rotation)
    {
        rotation = _rotation;
    }

    // Sets Aim
    public void SetCameraRotation(Vector3 _cameraRotation)
    {
        cameraRotation = _cameraRotation;
    }

    // Perform Rotation
    void Rotate()
    {
        if (rotation != Vector3.zero)
        {
            rb.MoveRotation(transform.rotation * Quaternion.Euler(rotation));

        }
        if (cameraRotation != Vector3.zero && cam != null)
        {
			cam.transform.parent.transform.Rotate(cameraRotation);
        }

    }

	public void Jump(){
		// Debug.DrawRay(rb.transform.position, Vector3.down, Color.green, 3f);

		bool grounded = Physics.Raycast (rb.transform.position, Vector3.down, 3f);

		if (grounded) {
			rb.AddForce (new Vector3 (0, jumpForce * 10, 0), ForceMode.Impulse);
		}
	}

}

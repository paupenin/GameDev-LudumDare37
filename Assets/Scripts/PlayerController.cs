using UnityEngine;


[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float speed = 15f;

    [SerializeField]
    private float sensitivity = 5f;

    private PlayerMotor motor;

	
	void Start ()
    {
        // Load Motor
        motor = GetComponent<PlayerMotor>();

		#if ! UNITY_EDITOR
			Cursor.visible = false;
			Screen.lockCursor = true;
		#endif
	}

    void Update()
    {

		calcMovement();

		calcOrientation();

		calcAngle();

		// Check Jump key
		if (Input.GetKeyDown ("space")){
			motor.Jump();
		}

		// Check Escape key
		if (Input.GetKeyDown ("escape")){
			( new LevelLoader() ).menu();
		}

    }

	// Calculates and sets Movement velocity as Vector3
	void calcMovement(){
		float _xMov = Input.GetAxisRaw("Horizontal");
		float _yMov = Input.GetAxisRaw("Vertical");

		Vector3 _movHorizontal = transform.right * _xMov;
		Vector3 _movVertical = transform.forward * _yMov;

		Vector3 _velocity = (_movHorizontal + _movVertical).normalized * speed;

		motor.SetDirection(_velocity);
	}

	// Calculates and sets Orientation as Vector3
	void calcOrientation(){
		float _yRot = Input.GetAxisRaw("Mouse X");

		Vector3 _orientation = new Vector3(0f, _yRot, 0f) * sensitivity;

		motor.SetRotation(_orientation);
	}

	// Calculates and sets Aim as Vector3
	void calcAngle(){
		float _xRot = Input.GetAxisRaw("Mouse Y");

		Vector3 _cameraRotation = new Vector3(- _xRot, 0f, 0f) * sensitivity;

		motor.SetCameraRotation(_cameraRotation);
	}

}

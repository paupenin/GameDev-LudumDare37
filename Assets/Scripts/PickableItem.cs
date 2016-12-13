using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableItem : MonoBehaviour {


	[SerializeField]
	public int points = 10;

	[SerializeField]
	public bool isPickable = true;

	public bool hasBeenPicked = false;

	private Rigidbody rb;
	public Rigidbody rigidbody{
		get { return rb; }
	}


	// Burnable audio
	private AudioSource burnAudio;
	private float burnAudioOriginalVolume;

	// Burnable animation
	private Vector3 bSourceScale;
	private Vector3 bDestination = Vector3.zero;
	private float bSpeed = 0.5f;


	void Start(){
		rb = GetComponent<Rigidbody> ();
		burnAudio = GameObject.Find ("FireSound").GetComponent<AudioSource>();
		burnAudioOriginalVolume = burnAudio.volume;

		bSourceScale = transform.localScale;
	}

	void FixedUpdate(){
		
		if (bDestination != Vector3.zero) {

			// If we are close enought we destroy item and reset audio component
			if ( Vector3.Distance(transform.position, bDestination) < 0.5f) {
				removeFromSpace ();
			}

			// Move item towards fire
			transform.position = Vector3.MoveTowards(transform.position, bDestination, Time.deltaTime * bSpeed);

			// Rescale item

			transform.localScale -= (bSourceScale * Time.deltaTime * bSpeed);

			if (transform.localScale.x < 0 || transform.localScale.y < 0 || transform.localScale.z < 0) {
				removeFromSpace ();
			}

		}

	}


	public void burn(Vector3 endPoint){
		// Debug.Log (endPoint);

		isPickable = false;

		bDestination = endPoint;

		deactivatePhysics (true);

		if (burnAudio != null) {
			burnAudio.volume = 0.8f;
		}
	}

	public void activatePhysics(){

		if (rb != null) {
			rb.useGravity = true;
			rb.isKinematic = false;
			rb.constraints = RigidbodyConstraints.None;
		}

	}

	public void deactivatePhysics(bool isKinematic = false){
		
		if (rb != null) {
			rb.useGravity = false;
			rb.isKinematic = isKinematic;
			rb.constraints = RigidbodyConstraints.FreezeRotation;
		}

	}


	private void removeFromSpace (){
		if (burnAudio != null) {
			burnAudio.volume = burnAudioOriginalVolume;
		}
		Destroy (gameObject);
	}

}

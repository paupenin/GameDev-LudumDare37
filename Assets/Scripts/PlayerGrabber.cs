using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerGrabber : MonoBehaviour {


	[SerializeField]
	private Camera cam;

	[SerializeField]
	private GameObject hand;

	[SerializeField]
	private Text actionPick;
	[SerializeField]
	private Text actionThrow;

	[SerializeField]
	private float throwForce = 30;

	[SerializeField]
	private float grabbedObjectSpeed = 5f;

	private PickableItem currentItem;


	// Time counter
	[SerializeField]
	private Text timerCounter;

	[SerializeField]
	private float extraTime = 3f;

	private bool gameStarted = false;
	private float timeEnd = 12;

	// Points achived by player
	[SerializeField]
	private Text scoreCounter;

	private int points = 0;

	[SerializeField]
	private Text burnerCounter;

	private int burnedItems = 0;


	void Start(){
		actionPick.enabled = false;
		actionThrow.enabled = false;

		// Reset points
		PlayerPrefs.SetInt("Player Score", 0);
		PlayerPrefs.SetInt("Player Burned", 0);
	}

	void Update()
	{
		if (timeEnd < 0) {
			endGame ();
		}


		if (currentItem != null) {

			actionPick.enabled = false;
			actionThrow.enabled = true;

			if (Input.GetMouseButtonDown (0)) {
				throwItem ();
			}

			return;
		}

		actionThrow.enabled = false;


		// find item to grab
		PickableItem item = findItemByScreenCentre ();

		if (item != null) {

			actionPick.enabled = true;

			if (Input.GetMouseButtonDown (0)) {
				grabItem (item);
			}
		} else {
			actionPick.enabled = false;
		}
	}

	void FixedUpdate(){

		updateUI ();

		carryItem();

	}


	private void carryItem() {
		if (currentItem != null) {
			currentItem.transform.position = Vector3.MoveTowards(currentItem.transform.position, hand.transform.position, Time.deltaTime * grabbedObjectSpeed);
		}

	}

	private PickableItem findMouseItem(){
		// Debug.DrawRay(cam.transform.position, ray.direction * 15f, Color.green);

		RaycastHit hit;
		Ray ray = cam.ScreenPointToRay (Input.mousePosition);

		if (Physics.Raycast (ray, out hit, 15f)) {
			if (hit.collider != null && hit.collider.transform.parent != null) {
				PickableItem item = hit.collider.GetComponent<PickableItem> ();
				if (item && item.isPickable) {
					return item;
				}
			}
		}
		return null;
	}

	private PickableItem findItemByScreenCentre(){
		// Debug.DrawRay(cam.transform.position, cam.transform.forward * 15f, Color.green, 10f);

		RaycastHit hit;

		if(Physics.Raycast (cam.transform.position, cam.transform.forward, out hit, 15f)){
			if (hit.collider != null && hit.collider.transform.parent != null) {
				PickableItem item = hit.collider.GetComponent<PickableItem> ();
				if (item && item.isPickable) {
					return item;
				}
			}
		}
		return null;
	}

	private void grabItem(PickableItem item) {
		// Debug.Log ("Object Grabbed");

		gameStarted = true;


		// first time picked items
		if (!item.hasBeenPicked) {
			timeEnd += extraTime;
		}

		currentItem = item;

		currentItem.hasBeenPicked = true;

		currentItem.deactivatePhysics();
	}


	/**
	 * Throws the item to the ground
	 */
	private void throwItem() {
		// Debug.Log ("throw item");

		currentItem.activatePhysics ();

		currentItem.rigidbody.AddForce (gameObject.transform.forward * throwForce, ForceMode.Impulse);

		currentItem = null;
	}


	public void itemBurned (PickableItem item){

		gameStarted = true;
		
		if (item == currentItem) {
			currentItem = null;
		}

		burnedItems += 1;
		points += item.points;

		PlayerPrefs.SetInt("Player Score", points);
		PlayerPrefs.SetInt("Player Burned", burnedItems);

	}

	void updateUI(){

		int points = PlayerPrefs.GetInt("Player Score");
		int burnedItems = PlayerPrefs.GetInt("Player Burned");

		if (gameStarted) {
			
			// Count down
			if (timerCounter != null) {
				timeEnd -= Time.deltaTime;
				timerCounter.text = string.Format ("{0:0}", timeEnd);
			}

			// items burned counter	
			if (burnerCounter != null) {
				if (burnedItems > 0) {
					burnerCounter.text = string.Format ("Burned\n{0}", burnedItems);
				}
			}

			// Score
			if (scoreCounter != null) {
				scoreCounter.text = "SCORED\n"+points;
				if (points > 0) {
					scoreCounter.text = string.Format ("SCORE\n{0}", points);
				} else {
					scoreCounter.text = "";
				}
			}
		}

	}

	void endGame(){

		// Debug.Log("GAME ENDED! Sooorrry");

		( new LevelLoader() ).menu();

	}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burner : MonoBehaviour {

	[SerializeField]
	private PlayerGrabber player;


	void OnCollisionEnter(Collision collision) {

		if (collision.gameObject != null) {
			
			PickableItem item = collision.gameObject.GetComponent<PickableItem> ();

			if (item != null) {

				player.itemBurned (item);

				item.burn (transform.position);

			}

		}

	}

}

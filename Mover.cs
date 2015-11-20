/* Fly-by camera, shoots bullets
 * Requires a bullet component in the Resources folder
**/

using UnityEngine;
using System.Collections;

public class Mover : MonoBehavior {
	void Start() {
		// Lock cursor to middle of screen
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	void Update(){
		/* UE4-ish Fly-by camera */
		
		// Space.Self - relative to ourselves
		transform.Translate(Input.GetAxis("Horizontal")*Time.deltaTime*10f, 0, Input.GetAxis("Vertical")*Time.deltaTime*10f, Space.Self);
		// Rotate around the world space
		transform.Rotate (0f, Input.GetAxis("Mouse X"), 0f, Space.World);
		transform.Rotate (Input.GetAxis("Mouse X"), 0f, 0f, Space.Self);

		if(Input.GetKeyDown(KeyCode.E)){
			/* Shoot bullet */
			GameObject createdBullet = (GameObject) GameObject.Instantiate(Resources.Load("bullet"));
			createdBullet.transform.position = this.transform.position;
			Rigidbody rb = createdBullet.GetComponent<Rigidbody> (); // rigidbody reference towards the one of the bullet established
			rb.AddForce(transform.forward * 42000f);
		}

		if(Input.GetKeyDown (KeyCode.Escape)) {
			// Hit Esc to exit game
			Application.Quit();
		}
	}
}
/* Pseudo-singleton 
 * Block component must be in Resources folder
 * All blocks need tag "block item" to be 
 * recognized and used by script
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlockControl : MonoBehavior {
	// GameObject as key, class instance with all relevant data as value                                                                             
	Dictionary<GameObject, Rigidbody_rotation_and_positionStack> blocksDictionary = new Dictionary<GameObject, Rigidbody_rotation_and_positionStack>();
	public bool isRecording = true;

	[RuntimeInitializeOnLoadMethod] // Run static method at runtime start
	static void Init() { // Static means it can be called w/ no instance o the class instantiated
		/* Stick Game Object at center of world */
		GameObject instanceGO = new GameObject();
		instanceGO.transform.position = Vector3.zero;
		instanceGO.name = "BlockControl-singleton GO";
		instanceGO.AddComponent<BlockControl>();
	}

	void Start() {
		GameObject[] blocks = GameObject.FindGameObjectsWithTag("block item");
		foreach(GameObject b in blocks){
			// Start game off w/ rigidbodies asleep
			Rigidbody r = b.GetComponent<Rigidbody>();
			r.Sleep();
			blocksDictionary.Add(b, new Rigidbody_rotation_and_positionStack(b.GetComponent<Rigidbody> (), b.transform.position, b.transform.rotation));
		}
		Debug.Log("Singleton initialized, dictionary populated. Dictionary's length is " + blocksDictionary.Count);
	}

	void Update() {
		if(Input.GetKeyDown(KeyCode.R)){
			isRecording = !isRecording; // Opposite of what it was
		}
	}

	void FixedUpdate() { // Updates alongside phyics calculations, not frames
		Record();
		switch(isRecording) {
			case true:
				Record();
				break;
			case fase:
				Rewind();
				break;
		}
	}

	private class Rigidbody_rotation_and_positionStack{
		/* Hold all the data of a given GameObject */
		public Rigidbody rb;
		public Stack<Vector 3> trace = new Stack<Vector3> (); // Store positions as the cubes are falling
		public Stack<Quaternion> rotation = new Stack<Quaternion> (); // Same for rotation. Put the newer positions on top of older ones

		public Rigidbody_rotation_and_positionStack(Rigidbody r, Vector3 pos, Quaternion rot) {
			rb = r;
			position.Push(pos);
			rotation.Push(rot);
		}
	}

	void Record() {
		Debug.Log("We are recording");
		/* Iterate over each Dictionary entry and */
		foreach(KeyValuePair<GameObject, Rigidbody_rotation_and_positionStack> kvp in blocksDictionary) {
			kvp.Value.rb.isKinematic = false;
			if(kvp.Value.rb.IsSleeping() || kvp.Value.position.Count == 0) {
				kvp.Value.position.Push(kvp.Key.transform.position); // Push the GameObject's(key's) position to the Stack(Value) position
				kvp.Value.rotation.Push(kvp.Key.transform.rotation); // Ditto for rotation
			}
		}
	}

	void Rewind() {
		Debug.Log("We are rewinding");
		bool noneMoved = true;

		foreach(KeyValuePair<GameObject, Rigidbody_rotation_and_positionStack> kvp in blocksDictionary) {
			kvp.Value.rb.Sleep(); // Put rigidbody/physics to sleep
			kvp.Value.rb.isKinematic = true;
			if(kvp.Value.position.Count > 0){
				kvp.Key.transform.position = kvp.Value.position.Pop();
				kvp.Key.transform.rotation = kvp.Value.rotation.Pop();
				noneMoved = false;
			}
		}

		if(noneMoved){
			isRecording = true;
			foreach(KeyValuePair<GameObject, Rigidbody_rotation_and_positionStack kvp in blocksDictionary) {
				kvp.Value.rb.isKinematic = false;
				kvp.Value.rb.Sleep();
			}
		}
	}


}
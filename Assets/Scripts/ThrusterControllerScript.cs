using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrusterControllerScript : MonoBehaviour {

	private GameObject hub1;
	private GameObject hub2;
	private GameObject thruster1;
	private GameObject thruster2;
	private Rigidbody hubBody1;
	private Rigidbody hubBody2;

	private RectTransform partialNukeBar;
	private const float PARTIAL_NUKE_BAR_MAX_WIDTH = 380f;

	private float stopCooldown = 5.0f;


	//public float thrusterPower = 100.0f;

	public const string MOUSE_TRACKING = "mousetracking";
	public const string MOUSE_PARALLEL = "mouseparallel";
	public const string MOUSE_INVERSE_TRACKING = "mouseinversetracking";
	public const string SNAP_TO_QUARTER = "snaptoquarter";
	private const float ANGLE_OFFSET = 90;
	private const float STOP_COOLDOWN = 5f;

	public string currentControl = MOUSE_TRACKING;


	// Get Player game objects
	void Start () {
		hub1 = GameObject.Find("Hub1");
		hub2 = GameObject.Find("Hub2");
		thruster1 = GameObject.Find("Thruster1");
		thruster2 = GameObject.Find("Thruster2");
		hubBody1 = hub1.GetComponent(typeof(Rigidbody)) as Rigidbody;
		hubBody2 = hub2.GetComponent(typeof(Rigidbody)) as Rigidbody;

		partialNukeBar = (RectTransform)(GameObject.Find("PartialNukeBar").GetComponent(typeof(Image)) as Image).transform;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 mousePos = Input.mousePosition;
		//Set thruster angles
		if (thruster1) angleThruster(thruster1.transform, mousePos);
		if (thruster2) angleThruster(thruster2.transform, mousePos);
		if (Input.GetMouseButtonDown(1) && stopCooldown >= STOP_COOLDOWN) {
			stopCooldown -= STOP_COOLDOWN;
			hubBody1.velocity = new Vector3(0,0,0);
			hubBody2.velocity = new Vector3(0,0,0);
		}
		if (stopCooldown < STOP_COOLDOWN){
			stopCooldown += Time.deltaTime;
			partialNukeBar.sizeDelta = new Vector2((stopCooldown / STOP_COOLDOWN) * PARTIAL_NUKE_BAR_MAX_WIDTH, 12f);
		}
	}
	void FixedUpdate() {
		// Use thrusters
		if (Input.GetMouseButton(0)) {
			if (thruster1) useThruster(hub1, thruster1.transform);
			if (thruster2) useThruster(hub2, thruster2.transform);
		}
	}

	private void angleThruster(Transform thr, Vector3 mousePos) {
		var screenPoint = Camera.main.WorldToScreenPoint(thr.position);
		switch (currentControl) {
			case MOUSE_TRACKING:
				float xDiff = mousePos.x - screenPoint.x;
				float yDiff = mousePos.y - screenPoint.y;
				float angle = Mathf.Atan2(yDiff, xDiff) * Mathf.Rad2Deg + ANGLE_OFFSET;
				thr.rotation = Quaternion.Euler(new Vector3 (0,0, angle));
				break;
		}
	}
	private void useThruster(GameObject hub, Transform thr) {
		Rigidbody hubBody = hub.GetComponent(typeof(Rigidbody)) as Rigidbody;
		float xComponent = Mathf.Cos(Mathf.Deg2Rad * (thr.rotation.eulerAngles.z + ANGLE_OFFSET));
		float yComponent = Mathf.Sin(Mathf.Deg2Rad * (thr.rotation.eulerAngles.z + ANGLE_OFFSET));
		Vector3 newForce = new Vector3(xComponent,yComponent,0);
		hubBody.AddForce(newForce/* * thrusterPower*/, ForceMode.Impulse);
	}
}

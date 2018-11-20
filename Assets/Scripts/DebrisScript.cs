using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebrisScript : MonoBehaviour {

	private const float SPAWN_DELAY = 3f;
	private const float TEXTURE_PAINT_RADIUS = 55f;

	private float spawnSpeed = 0.08f;
	private bool active = false;
	private float timeElapsed = 0;
	private float dirRad;

	private GameObject arrow;
	private Collider gcollider;
	private Rigidbody grigidbody;
	private GameObject stage;
	private GameOverScript gameOverScript;
	private SpriteRenderer arrowRenderer;
	private Texture2D backgroundOverlay;
	private Camera mainCamera;
	public Sprite[] debrisSprite;



	void Start () {
		gcollider = gameObject.GetComponent(typeof(Collider)) as Collider;
		grigidbody = gameObject.GetComponent(typeof(Rigidbody)) as Rigidbody;
		stage = GameObject.Find("Stage");
		gameOverScript = stage.GetComponent("GameOverScript") as GameOverScript;
		dirRad = Random.Range(-Mathf.PI, Mathf.PI);
		gameObject.GetComponent<SpriteRenderer>().sprite = debrisSprite[(int)Mathf.Floor(Random.Range(0,3.99f))];
		mainCamera = GameObject.Find("Main Camera").GetComponent(typeof(Camera)) as Camera;

		arrow = Instantiate(globalPrefabs.getPrefab("Arrow"), gameObject.transform.position, Quaternion.Euler(new Vector3(0,0, dirRad * Mathf.Rad2Deg - 90)));
		arrowRenderer =  arrow.GetComponent<SpriteRenderer>()as SpriteRenderer;
		gcollider.enabled = false;
		grigidbody.detectCollisions = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (!active) {
			timeElapsed += Time.deltaTime; 
		}
		if (timeElapsed >= SPAWN_DELAY) {
			activateDebris();
			timeElapsed -= SPAWN_DELAY;
			active = true;
		}
		if (arrow) {
			float ratio = ((timeElapsed / SPAWN_DELAY));
			arrowRenderer.color = new Color(1f,1f,1f,ratio); 
		}
	}

	void activateDebris() {
		gcollider.enabled = true;
		grigidbody.detectCollisions = true;
		Destroy(arrow);

		float xComponent = Mathf.Cos(dirRad) * Mathf.Rad2Deg * Random.Range(spawnSpeed/2, spawnSpeed);
		float yComponent = Mathf.Sin(dirRad) * Mathf.Rad2Deg * Random.Range(spawnSpeed/2, spawnSpeed);
		grigidbody.velocity = new Vector3(xComponent, yComponent, 0);
		grigidbody.AddTorque(transform.forward * Random.Range(-3, 3) * 100);
	}

	void OnCollisionEnter (Collision col)
    {
        if(col.gameObject.name == "Lightning")
        {
        	gameOverScript.addDebrisScore();
        	backgroundOverlay = (GameObject.Find("Background").GetComponent(typeof(Renderer)) as Renderer).material.mainTexture as Texture2D;
        	paintTransparency();
            Destroy(gameObject);
        } else if (col.gameObject.name == "Hub1" || col.gameObject.name == "Hub2") {
        	gameOverScript.triggerGameOver();
        	Destroy(col.gameObject);
        }
    }
    void paintTransparency () {

    	RaycastHit hit;
    	Vector3 screenPos = mainCamera.WorldToScreenPoint(gameObject.transform.position);
    	screenPos.z = 0;
    	screenPos.y = Screen.height - screenPos.y;
        if (!Physics.Raycast(mainCamera.ScreenPointToRay(screenPos), out hit)) return;
        Vector2 pixelUV = hit.textureCoord;
    	float centerX = pixelUV.x * backgroundOverlay.width;
    	float centerY = pixelUV.y * backgroundOverlay.height;
        for (float y = centerY - TEXTURE_PAINT_RADIUS; y < centerY + TEXTURE_PAINT_RADIUS; y++)
        {
        	float yLength = Mathf.Abs(centerY - y);
            for (float x = centerX - TEXTURE_PAINT_RADIUS; x < centerX + TEXTURE_PAINT_RADIUS; x++)
            {
            	float xLength = Mathf.Abs(centerX - x);
                if (x >= 0 && x < backgroundOverlay.width && y >= 0 && y < backgroundOverlay.height &&
                	(Mathf.Pow(yLength, 2) + Mathf.Pow(xLength, 2) <= Mathf.Pow(TEXTURE_PAINT_RADIUS, 2))) {
                	Color color = Color.clear;
                	backgroundOverlay.SetPixel((int)Mathf.Round(x), (backgroundOverlay.height - (int)Mathf.Round(y)), color);
                }
            }
        }
        backgroundOverlay.Apply();
    }
}
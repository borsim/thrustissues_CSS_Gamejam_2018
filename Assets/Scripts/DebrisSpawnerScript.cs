using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebrisSpawnerScript : MonoBehaviour {

	private float timeElapsed = 0f;
	private float spawningFrequency = 6f;
	private float additionalSpawningFrequency = 30f;
	private const float SPAWN_FREQUENCY_SCALING = 0.4f;

	private const float STAGE_LEFT = 0.5f;
	private const float STAGE_WIDTH = 10f;
	private const float STAGE_TOP = 0.5f;
	private const float STAGE_HEIGHT = 5f;


	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		timeElapsed += Time.deltaTime; 
		float currentSpawnTime = Mathf.Log(spawningFrequency + Mathf.Max(additionalSpawningFrequency, 0));
		additionalSpawningFrequency -= Time.deltaTime * SPAWN_FREQUENCY_SCALING;

		if (timeElapsed >= currentSpawnTime) {
			timeElapsed -= currentSpawnTime;
			spawnDebris();
		}
	}

	void spawnDebris() {
		float spawnX = Random.Range(STAGE_LEFT, STAGE_WIDTH);
		float spawnY = Random.Range(STAGE_TOP, STAGE_HEIGHT);

		Instantiate(globalPrefabs.getPrefab("Debris"), new Vector3(spawnX, spawnY, 0), Quaternion.identity);
	}
}

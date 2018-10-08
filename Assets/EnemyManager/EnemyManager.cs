using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

	public bool isOn = false;
	public bool isDone = false;
	public GameObject enemyPrefab;
    public BeeManager bManager;

	public List<GameObject> existingHoles;
	public List<GameObject> activeHoles;
	
	public int enemiesLeft = 0;

	void Start() {
		for(int b = 0; b < transform.childCount; b++) 	{
			existingHoles.Add(transform.GetChild(b).gameObject);
			transform.GetChild(b).gameObject.SetActive(false);
		}
	}
	public void ActivateHoles(int holeQuantity) {
		activeHoles.Clear();
		for(int a = 0; a < holeQuantity; a++){
			int inx = Random.Range(0,existingHoles.Count);
			existingHoles[inx].SetActive(true);
			activeHoles.Add(existingHoles[inx]);
		}
		
	}

	public IEnumerator  ReleaseEnemies(int howManyTimes, int howManyEnemies, int delay) {
		FindObjectOfType<GameManager>().Rebake();
		int inx = Random.Range(0,activeHoles.Count);
		for(int d = 0; d< howManyTimes; d++){
			for(int e = 0; e< howManyEnemies; e++){
				Instantiate(enemyPrefab,activeHoles[inx].transform.position+new Vector3(0,2,5),Quaternion.identity);
				enemiesLeft ++;
            }
		FindObjectOfType<SoundManager>().PlayTrack("Enemy Sound");
        yield return new WaitForSeconds(delay);
		}
		StartCoroutine(CheckIfThereAreEnemiesLeft());
		yield return null;
	}

	IEnumerator CheckIfThereAreEnemiesLeft() {
		while(enemiesLeft>0)
			yield return null;
		isDone = true;
		foreach(GameObject o in activeHoles)
			o.SetActive(false);
		yield break;
	}


	void Update()
	{
		if(Input.GetKeyDown(KeyCode.R))
			FindObjectOfType<SoundManager>().PlayTrack("Buraco Aberto");
	}
}

	using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour {

	public string ofType;

	public int hp = 100;
	public int maxHp = 100;

	public GameObject empty, goop, potHoney, potJelly,potPropolis;

	public void UpdateType() {
		Debug.Log("Clearing cell type");
		empty.SetActive (false);
		goop.SetActive (false);
		potHoney.SetActive (false);
		potJelly.SetActive (false);
		potPropolis.SetActive (false);
		
		Debug.Log("Choosing the cell right type");
		if (ofType == "empty") {
		maxHp = 0;
            transform.tag = "Untagged";
            empty.SetActive (true);
		} else if (ofType == "goop") {
		maxHp = 100;
            transform.tag = "Goop";
            goop.SetActive (true);
		} else if (ofType == "potHoney") {
		maxHp = 50;
            transform.tag = "Mel";
		potHoney.SetActive (true);
		//FindObjectOfType<PlayerStats>().stats.honeyCellCount--;
		} else if (ofType == "potJelly") {
		maxHp = 80;
            transform.tag = "Geleia";
            potJelly.SetActive (true);
		//FindObjectOfType<PlayerStats>().stats.royalCellCount--;
		} else if (ofType == "potPropolis") {
		maxHp = 50;
            transform.tag = "Propolis";
            potPropolis.SetActive (true);
		//FindObjectOfType<PlayerStats>().stats.propolisCellCount--;
		} 
	}

	



}

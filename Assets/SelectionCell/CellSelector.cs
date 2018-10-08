using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellSelector : MonoBehaviour {
	public GameManager gM;
	public PlayerStats pStats;
	public Cell currentCell = new Cell();
	public bool selecting = false;
	public GameObject UIgoop, UIhoney, UIjelly,UIpropolis,UIcanvas,selectedUI;
    public Vector3 cellpos;

	public bool isOn = true;




    void Start () {
		
	}
	void Update () {
		
			if (Input.GetMouseButtonDown (0)) {
				RaycastHit hitInfo = new RaycastHit ();
				if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hitInfo)) {
					if (hitInfo.transform.tag == "Cell") {
                        
                        TurnOffUI();

                        if(hitInfo.transform.parent.transform.GetComponent<Cell>() != null)
                         selectedUI.transform.position = hitInfo.transform.parent.transform.position;
                        else
                             selectedUI.transform.position = hitInfo.transform.position;
                        cellpos = hitInfo.point;
                        MoveCanvas(hitInfo.point);
                        Select(hitInfo.transform.parent.transform.GetComponent<Cell>());
                        selecting = true;
                     
					}
				}
			} else if (Input.GetMouseButtonDown (1)) {
			
				TurnOffUI ();
				selecting = false;
			}
		
		
	}
	void MoveCanvas(Vector3 cellpos) {

//		print(cellpos);
		//UIcanvas.transform.position = cellpos + new Vector3(0,50,0);
		if(cellpos.x > 70){
			if(cellpos.z < 40){
				UIcanvas.transform.position = cellpos + new Vector3(-25,30,15);
			}else {
				UIcanvas.transform.position = cellpos + new Vector3(-25,30,-15);
		}
		}else {
			if(cellpos.z < 40){
				UIcanvas.transform.position = cellpos + new Vector3(25,30,15);
			}else {
				UIcanvas.transform.position = cellpos + new Vector3(25,30,-15);
			}
		}
	}

    public void Select(Cell selectedCell)
    {
        //Activate GUI
        selectedUI.SetActive(true);
        currentCell = selectedCell;
        if (isOn && selectedCell.ofType != "polen")
        {
            UIcanvas.SetActive(true);
            Debug.Log("Activating the right UI");
            if (selectedCell.ofType == "empty")
            {
                UIgoop.SetActive(true);
				if(pStats.stats.propolis >= 3)
					UIgoop.GetComponent<Button>().interactable = true;
				else
					UIgoop.GetComponent<Button>().interactable = false;
            }
            else
            {
                //Check if the selected cell is a royal jelly cell
                //Also check if the player has only one royal cell
                //If that is met it means that the player selected its only royal jelly cell and cannot modify it
                if (selectedCell.ofType == "potJelly" && pStats.stats.royalCellCount <= 1)
                {
                    TurnOffUI();
                        UIhoney.SetActive(true);
							UIhoney.GetComponent<Button>().interactable = false;
                        UIpropolis.SetActive(true);
							UIpropolis.GetComponent<Button>().interactable = false;
                        UIgoop.SetActive(true);
							UIgoop.GetComponent<Button>().interactable = false;
                }
                //Else he can edit any he wants
                else
                {
                    //Only activates the right GUI
                    
						if(pStats.stats.propolis >= 5)
							UIhoney.GetComponent<Button>().interactable = true;
						else
							UIhoney.GetComponent<Button>().interactable = false;
					if (currentCell.ofType != "potHoney")
                        UIhoney.SetActive(true);
                   
						if(pStats.stats.propolis >= 15)
							UIjelly.GetComponent<Button>().interactable = true;
						else
							UIjelly.GetComponent<Button>().interactable = false;
					 if (currentCell.ofType != "potJelly")
                        UIjelly.SetActive(true);
                   
						if(pStats.stats.propolis >= 7)
							UIpropolis.GetComponent<Button>().interactable = true;
						else
							UIpropolis.GetComponent<Button>().interactable = false;
					 if (currentCell.ofType != "potPropolis")
                        UIpropolis.SetActive(true);

						if(pStats.stats.propolis >= 3)
							UIgoop.GetComponent<Button>().interactable = true;
						else
							UIgoop.GetComponent<Button>().interactable = false;
					if (currentCell.ofType != "goop")
                        UIgoop.SetActive(true);
                }
            }
            Debug.Log("Waiting for input");
        }
    }

	 public void TurnOffUI() {

		 //Turn off all the GUI elements
		selectedUI.SetActive(false);
		UIcanvas.SetActive(false);
		UIgoop.SetActive (false);
		UIhoney.SetActive (false);
		UIjelly.SetActive (false);
		UIpropolis.SetActive (false);
	  }

	 public void ChangeState(string newState) {
		//TurnOffUI();
		//Check if the modifying cell is a royal jelly cell, if it is down the count in the player stats
		if(currentCell.ofType == "potJelly")
		    pStats.stats.royalCellCount --;
        else if (currentCell.ofType == "potHoney")
            pStats.stats.honeyCellCount--;
        else if (currentCell.ofType == "potPropolis")
            pStats.stats.propolisCellCount--;
        //If the new cell is a royal jelly cell, up the royalJellyCount in the player stats
        if (newState == "potJelly")	
			pStats.stats.royalCellCount ++;
        else if (newState == "potHoney")
            pStats.stats.honeyCellCount++;
        else if (newState == "potPropolis")
            pStats.stats.propolisCellCount++;

		if(newState == "potJelly") {
			pStats.stats.propolis -= 15;
		} else if(newState == "potHoney") {
			pStats.stats.propolis -= 5;
		} else if(newState == "potPropolis") {
			pStats.stats.propolis -= 7;
		} else if(newState == "goop") {
			pStats.stats.propolis -= 3;
		}
        //Change cell info update it and add some time to the nav baker
        currentCell.ofType = newState; 
		currentCell.UpdateType();
		gM.AddBakeTime(1);

		//Turn off GUI and select the new cell
		TurnOffUI();
		Select(currentCell);
	}

	
}

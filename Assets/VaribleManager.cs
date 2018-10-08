using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VaribleManager : MonoBehaviour {
	public Text royalJelly, polen, honey, propolis;

	public PlayerStats pS;

	void Update() {
		UpdateUI ();
	}

	void UpdateUI() {
		royalJelly.text ="G " + pS.stats.royalJelly.ToString() ;
		polen.text ="P " + pS.stats.polen.ToString() ;
		honey.text ="M " + pS.stats.honey.ToString() ;
		propolis.text ="C " + pS.stats.propolis.ToString() ;
	}
}

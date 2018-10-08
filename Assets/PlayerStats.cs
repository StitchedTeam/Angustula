using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

	[System.Serializable]
	public struct Stats {

		//Quantity the player ACTUALLY has
		public int royalJelly;
		public int polen;
		public int honey;
		public int propolis;
		[Space(5)]
		// Max quantity the player can hold of each variable
		public int maxroyalJelly;

		//There is no limit for polen production ?
		//public int maxpolen;
		public int maxhoney;
		public int maxpropolis;

		// Quantity the player loses or gets of each variable every "turn"
		//Royal jelly is equivalent to the quantity of royal jelly cells in the hive
		//public int producingroyalJelly;
		//polen is only get on trips to the outside
		//public int producingpolen;
		//honey is only gotten when a bee is producing it
		//public int producinghoney;
		//public int producingpropolis;


		// Other stuffs
		public int royalCellCount, honeyCellCount, propolisCellCount;
	}
	[SerializeField]
	public Stats stats;
    float timer;
    public int rainhaComendo = 1;

    void Start()
    {
        stats.honeyCellCount = 1;
        stats.propolisCellCount = 1;
        stats.royalJelly = stats.maxroyalJelly;
    }
    void Update()
    {
        AtualizaMaximo();
        NaoPassaDoMaximo();
        RainhaComendo();
    }

    void NaoPassaDoMaximo()
    {
        if (stats.honey > stats.maxhoney)
        {
            stats.honey = stats.maxhoney;
        }
        if (stats.royalJelly > stats.maxroyalJelly)
        {
            stats.royalJelly = stats.maxroyalJelly;
        }
        if (stats.propolis > stats.maxpropolis)
        {
            stats.propolis = stats.maxpropolis;
        }

        if (stats.honey < 0)
        {
            stats.honey = 0;
        }
        if (stats.royalJelly < 0)
        {
            stats.royalJelly = 0;
        }
        if (stats.propolis < 0)
        {
            stats.propolis = 0;
        }
    }

    void AtualizaMaximo()
    {
        stats.maxhoney = stats.honeyCellCount * 10 + 10;
        stats.maxroyalJelly = stats.royalCellCount * 10 + 10;
        stats.maxpropolis = stats.propolisCellCount * 10;
    }

    void RainhaComendo()
    {
        timer += Time.deltaTime;
        if (timer > 10)
        {
            stats.royalJelly -= rainhaComendo;
            timer = 0;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BeeStatistics : MonoBehaviour {
    public bool construindo, mel, geleia, defesa, propolis, polem, fome;
    public Sprite[] icon = new Sprite[6];
    public Image trabalho;
    public GameObject fomeIcon;
    public Squad esquadrao;
    public CellSelector cellSelector;

    // Use this for initialization
    void Start () {
        cellSelector = GameObject.FindGameObjectWithTag("Player").GetComponent<CellSelector>();
    }
	
	// Update is called once per frame
	void Update () {
        VerificaTrabalho();
        if (fome)
            fomeIcon.SetActive(true);
        else
            fomeIcon.SetActive(false);
    }

    public void VerificaTrabalho()
    {
        if (construindo)
        {
            trabalho.sprite = icon[0];
            trabalho.color = new Color(1, 1, 1, 1);
        }
        else if (mel)
        {
            trabalho.sprite = icon[1];
            trabalho.color = new Color(1, 1, 1, 1);
        }
        else if (geleia)
        {
            trabalho.sprite = icon[2];
            trabalho.color = new Color(1, 1, 1, 1);
        }
        else if (defesa)
        {
            trabalho.sprite = icon[3];
            trabalho.color = new Color(1, 1, 1, 1);
        }
        else if (propolis)
        {
            trabalho.sprite = icon[4];
            trabalho.color = new Color(1, 1, 1, 1);
        }
        else if (polem)
        {
            trabalho.sprite = icon[5];
            trabalho.color = new Color(1, 1, 1, 1);
        }
        else
        {
            trabalho.sprite = null;
            trabalho.color = new Color(1, 1, 1, 0);
        }
    }

    public void RecebeTrabalho()
    {
        //Recebe informações sobre a célula selecionada
        esquadrao.localTrabalho = cellSelector.cellpos;

        //Atualiza o tipo de trabalho
        if (cellSelector.currentCell.ofType == "potHoney")
        {
            esquadrao.trabalho = 1;
            construindo = false;
            mel = true;
            geleia = false;
            defesa = false;
            propolis = false;
            polem = false;
        }
        else if (cellSelector.currentCell.ofType == "potJelly")
        {
            esquadrao.trabalho = 2;
            construindo = false;
            mel = false;
            geleia = true;
            defesa = false;
            propolis = false;
            polem = false;
        }
        else if (cellSelector.currentCell.ofType == "potPropolis")
        {
            esquadrao.trabalho = 3;
            construindo = false;
            mel = false;
            geleia = false;
            defesa = false;
            propolis = true;
            polem = false;
        }
        else if (cellSelector.currentCell.ofType == "goop")
        {
            esquadrao.trabalho = 4;
            construindo = false;
            mel = false;
            geleia = false;
            defesa = true;
            propolis = false;
            polem = false;
        }
        else if (cellSelector.currentCell.ofType == "polen")
        {
            esquadrao.trabalho = 5;
            construindo = false;
            mel = false;
            geleia = false;
            defesa = false;
            propolis = false;
            polem = true;
        }
        else
        {
            esquadrao.trabalho = 0;
            construindo = false;
            mel = false;
            geleia = false;
            defesa = false;
            propolis = false;
            polem = false;
        }

        //Chama a função pra ir ao local de trabalho
        if (esquadrao.trabalho != 5)
            esquadrao.VaiTrabalhar();
        else
            StartCoroutine(esquadrao.BuscarPolen());
            
    }
}

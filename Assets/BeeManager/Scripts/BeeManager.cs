using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeeManager : MonoBehaviour {
    public GameObject[] esquadroes;
    public RectTransform lista;
    public GameObject listaCellPrefab;
    public GameObject[] listCell;
    public float timerTrabalho, timerBercario, coolDownBercario = 5;
    public bool bercario;
    public Button btnBercario;
    public int[] abelhasTrabalhando = new int[7];
    /*Trabalhos:
    0 - A toa
    1 - Mel
    2 - Geléia
    3 - Própolis
    4 - Defesa
    5 - Pólen
    6 - Comendo
    */
    public PlayerStats playerS;
    public GameObject esquadraoPreFab, saida;

	// Use this for initialization
	void Start () {
        playerS = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        AtualizaSquad();
    }
	
	// Update is called once per frame
	void Update () {
        timerTrabalho += Time.deltaTime;
        if (timerTrabalho > 1)
        {
            AtualizaTrabalho();
            AtualizaPlayerStats();
            timerTrabalho = 0;
        }
        Bercario();
    }

    public void AtualizaLista()
    {
        foreach (Transform child in lista)
        {
            GameObject.Destroy(child.gameObject);
        }
        listCell = new GameObject[esquadroes.Length];
        for (int x = 0; x < esquadroes.Length; x++)
        {
            listCell[x] = Instantiate(listaCellPrefab, lista);
            listCell[x].transform.GetChild(0).gameObject.GetComponent<Text>().text = esquadroes[x].name;
            listCell[x].GetComponent<BeeStatistics>().esquadrao = esquadroes[x].GetComponent<Squad>();

            esquadroes[x].GetComponent<Squad>().id = x;
            esquadroes[x].GetComponent<Squad>().bStatistics = listCell[x].GetComponent<BeeStatistics>();
        }
    }

    public void AtualizaSquad()
    {
        esquadroes = null;
        esquadroes = GameObject.FindGameObjectsWithTag("Esquadrao");
        AtualizaLista();
    }

    public void AtualizaTrabalho()
    {
        for (int y = 0; y < 7; y++)
        {
            abelhasTrabalhando[y] = 0;
        }
        for (int x = 0; x < esquadroes.Length; x++)
        {
            if (esquadroes[x].GetComponent<Squad>().trabalhando == true)
            {
                if (esquadroes[x].GetComponent<Squad>().trabalho == 1)
                {
                    abelhasTrabalhando[1]++;
                }
                else if (esquadroes[x].GetComponent<Squad>().trabalho == 2)
                {
                    abelhasTrabalhando[2]++;
                }
                else if (esquadroes[x].GetComponent<Squad>().trabalho == 3)
                {
                    abelhasTrabalhando[3]++;
                }
                else if (esquadroes[x].GetComponent<Squad>().trabalho == 4)
                {
                    abelhasTrabalhando[4]++;
                }
                else if (esquadroes[x].GetComponent<Squad>().trabalho == 5)
                {
                    abelhasTrabalhando[5]++;
                }
                if (esquadroes[x].GetComponent<Squad>().comendo)
                {
                    abelhasTrabalhando[6]++;
                }
            }
        }
        
    }

    public void AtualizaPlayerStats()
    {
        //Atualiza os status de acordo com a qtd de abelhas trabalhando
        playerS.stats.honey += abelhasTrabalhando[1] - abelhasTrabalhando[6] * 2; //Produzido - comido
        playerS.stats.royalJelly += abelhasTrabalhando[2];
        if (playerS.stats.propolis < playerS.stats.maxpropolis)
        {
            playerS.stats.propolis += abelhasTrabalhando[3];
            playerS.stats.polen -= abelhasTrabalhando[3];
        }
    }

    public void CriaNovoEsquadrao()
    {
        if (bercario)
        {
            Instantiate(esquadraoPreFab, saida.transform.position, transform.rotation, this.gameObject.transform);
            AtualizaSquad();
            AtualizaLista();
            if (coolDownBercario < 30)
                coolDownBercario += 1;
            timerBercario = 0;
        }
        
    }

    public void Bercario()
    {
        if (timerBercario > coolDownBercario)
        {
            btnBercario.image.color = new Color(1, 1, 1);
            bercario = true;
        }
        else
        {
            btnBercario.image.color = new Color(0.5f, 0.5f, 0.5f);
            timerBercario += Time.deltaTime;
            bercario = false;
        }
    }
}

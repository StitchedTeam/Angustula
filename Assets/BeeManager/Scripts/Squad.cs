using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Squad : MonoBehaviour {
    //Variáveis para comida
    public int fome;
    public int fomeMax = 30;
    public float fomeTimer;
    public bool comendo = false;

    //Variáveis para trabalho
    public int trabalho;
    public bool trabalhando = false;

    //Variáveis para Management
    public NavMeshAgent[] agents;
    public BeeManager bManager;
    public BeeStatistics bStatistics;
    public GameManager gameM;
    public EnemyManager enemyM;
    public int id;
    public float distancia;
    public Vector3 localTrabalho, localComida;

    public PlayerStats playerS;
    ParticleSystem particula;
    public Transform flores;
    public float timerPolen;



    // Use this for initialization
    void Start()
    {
        playerS = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        enemyM = GameObject.FindGameObjectWithTag("EnemyManager").GetComponent<EnemyManager>();
        flores = GameObject.FindGameObjectWithTag("Flores").transform;
        fome = fomeMax;
        AtualizaAgentes();
    }

    // Update is called once per frame
    void Update() {
        AtualizaFome();
        VerificaFome();
        VerificaTrabalho();
    }

    public Vector3 FindClosestHoney()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Mel");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest.transform.position;
    }

    public void MoveToLocal(Vector3 local)
    {
        for (int x = 0; x < transform.childCount; x++)
        {
            agents[x].destination = local;
        }

    }

    void AtualizaAgentes()
    {
        agents = null;
        agents = new NavMeshAgent[transform.childCount];
        for (int x = 0; x < transform.childCount; x++)
        {
            agents[x] = transform.GetChild(x).GetComponent<NavMeshAgent>();
        }
    }

    public void VaiComer()
    {
        localComida = FindClosestHoney();
        MoveToLocal(localComida);
    }

    public void VaiTrabalhar()
    {
        //Se a célular selecionada for vazia não faz nada
        if (trabalho != 0)
        {
            MoveToLocal(localTrabalho);
        }
    }

    void AtualizaFome()
    {
        if (!comendo && trabalho != 1 && trabalho != 5)
        {
            fomeTimer += Time.deltaTime;
            if (fomeTimer > 3)
            {
                fome -= Random.Range(1, 6);
                fomeTimer = 0;
            }
        }
        else
        {
            fomeTimer += Time.deltaTime;
            if (fomeTimer > 1 && playerS.stats.honey > 0)
            {
                fome += Random.Range(3, 8);
                if (fome >= fomeMax)
                {
                    bStatistics.fome = false;
                    if (trabalho != 5)
                        VaiTrabalhar();
                    fome = fomeMax;
                    comendo = false;
                }
                fomeTimer = 0;
            }
        }
    }

    void VerificaFome()
    {
        if (fome <= fomeMax / 3)
        {
            if (trabalho != 4)
            {
                bStatistics.fome = true;
                VaiComer();
                if (DistanciaAteObjetivo(localComida) <= 8)
                {
                    comendo = true;
                }
            }
            else
            {
                if (enemyM.isDone)
                {
                    bStatistics.fome = true;
                    VaiComer();
                    if (DistanciaAteObjetivo(localComida) <= 8)
                    {
                        comendo = true;
                    }
                }
                else
                {
                    VaiTrabalhar();
                    bStatistics.fome = false;
                    comendo = false;
                }
            }

        }
        if (fome < 0)
            fome = 0;
    }

    float DistanciaAteObjetivo(Vector3 objetivo)
    {
        distancia = Vector3.Distance(objetivo, agents[0].transform.position);
        return distancia;
    }

    void VerificaTrabalho()
    {
        if (DistanciaAteObjetivo(localTrabalho) <= 8)
        {
            trabalhando = true;
        }
        else
        {
            trabalhando = false;
        }
        switch (trabalho)
        {
            case 1:
                bStatistics.construindo = false;
                bStatistics.mel = true;
                bStatistics.geleia = false;
                bStatistics.defesa = false;
                bStatistics.propolis = false;
                bStatistics.polem = false;
                break;
            case 2:
                bStatistics.construindo = false;
                bStatistics.mel = false;
                bStatistics.geleia = true;
                bStatistics.defesa = false;
                bStatistics.propolis = false;
                bStatistics.polem = false;
                break;
            case 3:
                bStatistics.construindo = false;
                bStatistics.mel = false;
                bStatistics.geleia = false;
                bStatistics.defesa = false;
                bStatistics.propolis = true;
                bStatistics.polem = false;
                break;
            case 4:
                bStatistics.construindo = false;
                bStatistics.mel = false;
                bStatistics.geleia = false;
                bStatistics.defesa = true;
                bStatistics.propolis = false;
                bStatistics.polem = false;
                break;
            case 5:
                bStatistics.construindo = false;
                bStatistics.mel = false;
                bStatistics.geleia = false;
                bStatistics.defesa = false;
                bStatistics.propolis = false;
                bStatistics.polem = true;
                break;
            default:
                bStatistics.construindo = false;
                bStatistics.mel = false;
                bStatistics.geleia = false;
                bStatistics.defesa = false;
                bStatistics.propolis = false;
                bStatistics.polem = false;
                break;
        }
    }

    public void Atacar(Vector3 alvo)
    {
        for (int x = 0; x < agents.Length; x++)
        {
            agents[x].transform.LookAt(alvo);
            particula = agents[x].GetComponent<ParticleSystem>();
            particula.emission.SetBurst(0, new ParticleSystem.Burst(0, 1));
            particula.Play();
        }
    }
    
    public IEnumerator BuscarPolen()
    {
        while (trabalho == 5)
        {
            MoveToLocal(flores.position);
            Debug.Log("Vai pra Flor");
            yield return new WaitForSeconds(10);
            //Liga partícula de pólen
            fome = 10;
            playerS.stats.polen += 10;
            Debug.Log("Buscou o Pólen");
        }
        yield return null;
    }

}

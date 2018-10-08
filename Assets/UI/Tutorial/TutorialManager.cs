using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private Text texto;
    [SerializeField] private GameObject painel;
    [SerializeField] private GameObject previous;
    [SerializeField] private GameObject next;

    public int linhaTexto = 1;

    [SerializeField] private string[] linhas;

    private bool terminar = false;

    [SerializeField] private ParticleSystem particula;

    [SerializeField] private Vector3 partPos1;
    [SerializeField] private Vector3 partPos2;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(linhaTexto < 21)
            texto.text = linhas[linhaTexto].ToString();
        if (linhaTexto >= 21)
        {
            terminar = true;
        }

    }

    public void Next()
    {
        if (linhaTexto <= 20)
        {
            linhaTexto += 1;
        }

        if (terminar)
        {
            painel.SetActive(false);
        }

        if (linhaTexto == 2)
        {
            previous.SetActive(true);
        }
    }

    public void Previous()
    {
        if (linhaTexto >= 2)
        {
            linhaTexto -= 1;
        }

        if (linhaTexto == 1)
        {
            previous.SetActive(false);
        }
    }

    public void ParticlePlay()
    {
        if (linhaTexto == 4 || linhaTexto == 10)
        {
            if (linhaTexto == 4)
            {
                particula.transform.position = partPos1;
            }
            else if (linhaTexto == 10)
            {
                particula.transform.position = partPos2;
            }
            particula.Play();
        }
        else
        {
            particula.Stop();
        }
    }

    public void Kill()
    {
        linhaTexto = 22;
        painel.SetActive(false);
    }
}
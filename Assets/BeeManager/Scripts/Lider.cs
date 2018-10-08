using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lider : MonoBehaviour {
    Squad esquadrao;
    public float atkTimer;
    public int coolDown;

	// Use this for initialization
	void Start () {
        esquadrao = transform.GetComponentInParent<Squad>();
	}
	
	// Update is called once per frame
	void Update () {
        atkTimer += Time.deltaTime;
        AtualizaCoolDown();
    }

    void AtualizaCoolDown()
    {
        if (esquadrao.trabalho == 4)
            coolDown = 1;
        else
            coolDown = 3;
    }

    private void OnTriggerStay(Collider coll)
    {
       // print("1");
        if (coll.transform.parent.tag == "Inimigo")
        {
            if (atkTimer > coolDown)
            {
                
                coll.transform.parent.GetComponent<EnemyBehaviour>().hp--;
                esquadrao.Atacar(coll.gameObject.transform.position);
                atkTimer = 0;
            }
        }
    }
}

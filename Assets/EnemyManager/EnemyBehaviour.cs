using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour {

	Cell nextJelly;
	NavMeshAgent navAgt;
	public bool isAttacking = false;
    public int hp = 15;

    public float damage,coolDown;
    float atkTimer;

    //DRA QUANDO O INIMIGO MORRER (Usar outra funcao dq ondestroy buga)
    //void OnDestroy()
    //{
    //	FindObjectOfType<EnemyManager>().enemiesLeft --;
    //}

    void Start() {
		navAgt = GetComponent<NavMeshAgent>();
		nextJelly = FindRandomJelly();
		navAgt.SetDestination(nextJelly.transform.position);
	}

	void Update() {
		if(hp <= 0)
        {
            FindObjectOfType<EnemyManager>().enemiesLeft--;
            StopAllCoroutines();
            Destroy(gameObject);
        }
		
	}
	private void OnTriggerEnter(Collider other) {
		if(!isAttacking){
		if(other.transform.parent.GetComponent<Cell>())
			if(other.transform.parent.GetComponent<Cell>().ofType != "empty"){
				StartCoroutine(Attack(other.transform.parent.GetComponent<Cell>()));
			}
		}
	}

	IEnumerator Attack(Cell attackingCell)
	{
		isAttacking = true;
        atkTimer = 0;
		navAgt.destination = attackingCell.transform.position;
		while(attackingCell.hp> 0){
			while(!navAgt.isStopped){
				if(atkTimer >= coolDown)
                {
                    attackingCell.hp =(int)(attackingCell.hp- damage);
                    atkTimer = 0;
					FindObjectOfType<SoundManager>().PlayTrack("Enemy Sound");
                } else
                {
                    atkTimer += Time.deltaTime;
                }
				yield return null;
				if(attackingCell.hp<=0)	
					break;
			}
			yield return null;
		}
	//	if(attackingCell !=null&&  attackingCell.GetComponent<CellSelector>()!=){
	
        if (attackingCell.ofType == "potHoney")
           FindObjectOfType<PlayerStats>().stats.honeyCellCount--;
        else if (attackingCell.ofType == "potJelly")
            FindObjectOfType<PlayerStats>().stats.royalCellCount--;
        else if (attackingCell.ofType == "potPropolis")
            FindObjectOfType<PlayerStats>().stats.propolisCellCount--;
        attackingCell.ofType = "empty";
		attackingCell.UpdateType();
	//	}
		FindObjectOfType<SoundManager>().PlayTrack("Buraco Aberto");
		StartCoroutine(FindObjectOfType<ScreenShake>().Shake(0.5f));
		print("Destroyed cell");
		print(nextJelly);
		if(nextJelly != null){
		navAgt.SetDestination(nextJelly.transform.position);
		print("set to "+ navAgt.destination);
		}else {
		nextJelly = FindRandomJelly();
		if(nextJelly!= null){
			navAgt.SetDestination(nextJelly.transform.position);
			print("set to "+ navAgt.destination);
		}else 
			print("no jely");
		}
		isAttacking = false;
		
		
		yield return null;
	}

	Cell FindRandomJelly(){
		Cell[] cells = FindObjectsOfType<Cell>();
		Cell result = null;
		foreach(Cell c in cells)
			if(c.ofType == "potJelly")
				result = c;

		
		return(result);
	}
}

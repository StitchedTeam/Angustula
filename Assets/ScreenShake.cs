using System.Collections;
 using System.Collections.Generic;
  using UnityEngine;
   public class ScreenShake : MonoBehaviour { 
	   public Transform[] shakeables; 
	   public bool shaking; 
	   public IEnumerator Shake(float intensity) {
		    float time = Mathf.Sqrt(intensity); 
			Vector3[] initialPos = new Vector3[shakeables.Length];
			for (int b = 0; b < shakeables.Length; b++) 
				initialPos [b] = shakeables [b].position; 
			while(time > 0){ 
				for (int d = 0; d < shakeables.Length; d++) 
					shakeables[d].position = initialPos[d] + new Vector3(Mathf.Clamp(Random.Range(-intensity,intensity),-25,25),Mathf.Clamp(Random.Range(-intensity,intensity),-25,25),0);
				time -=0.01f;
				yield return null;
			} 
			for (int c = 0; c < shakeables.Length; c++) 
				shakeables [c].position = initialPos[c]; 
					
			shaking = false; 
			yield return null; 
		} 
	}
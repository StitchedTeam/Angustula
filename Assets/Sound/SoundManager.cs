using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	[HideInInspector]
	public static SoundManager instance;
	public int sourceQuantity;
	public int currentSource = 0;
	
	string testingNow;

	AudioSource[] sources;
	[SerializeField]
	public List<Track> tracks;
	//public bool playing;

	[System.Serializable]
	public class Track {
		public string name;
		[Range(0,3)]
		public float volume = 1;
		[Range(0.01f,10)]
		public float pitch = 1;
		public AudioClip track;
	}

	float lastIndex;

	void Awake()
	{
		if (instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}

		sources = new AudioSource[sourceQuantity];
		for(int a = 0; a< sources.Length; a++){
			sources[a] = gameObject.AddComponent<AudioSource>();
			sources[a].playOnAwake = false;
		}
			

	}


	IEnumerator TestCoroutine()
	{
		while(true) {
			yield return new WaitForSeconds(2f);
			PlayTrack(testingNow);
			yield return null;
			}
		
	}
	public void PlayTrack(string name){

		if(name != null && name!= "") {
			Track currentTrack = tracks[0];
			foreach(Track s in tracks) {
				if(s.name == name)
					currentTrack = s;
			}
			Play(currentTrack.track,currentTrack.pitch,currentTrack.volume);
		}

		
		
	}

	void Play(AudioClip s, float pitch, float volume) {
		sources[currentSource].pitch = pitch;
		sources[currentSource].volume = volume;
		sources[currentSource].clip = s;
		sources[currentSource].Play();

		currentSource++;
		if(currentSource >= sources.Length)
			currentSource = 0;

	}

}

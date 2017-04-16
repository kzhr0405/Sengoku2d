using UnityEngine;
using System.Collections;

public class Sound : MonoBehaviour {
	public AudioClip audioClip;
	private AudioSource audioSource;

	void Start () {
		audioSource = gameObject.GetComponent<AudioSource>();

		audioSource.clip = audioClip;
		audioSource.Play ();

	}
	
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBackground : MonoBehaviour {

	void Start () {
        DontDestroyOnLoad(transform.gameObject);
	}
	
}

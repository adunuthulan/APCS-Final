using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBat : MonoBehaviour {

	[SerializeField]
	private Image h;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		h.fillAmount = 0.8f;
	}
}

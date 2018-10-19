using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageManager : MonoBehaviour {

    [SerializeField] GameObject messageBox;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 messagePos = Camera.main.WorldToScreenPoint(transform.position);
        messageBox.transform.position = messagePos;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

    [SerializeField]
    private Stat health;
    private void Awake()
    {
        health.Initialize();
    }

    // Update is called once per frame
    void Update () {
		
	}
}

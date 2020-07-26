using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent] public class Oscilator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector = new Vector3(10f,10f,10f);
    [SerializeField] [Range(0,1)] float movementFactor; //0 not moving 1 full movement
    [SerializeField] float period = 2f; 
    Vector3 startingPos;

    // Start is called before the first frame update
    void Start(){
        //Guardamos la transformacion inicial del objeto
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update(){

        if (period >= Mathf.Epsilon) {
            float cycles = Time.time / period;
            const float tau = Mathf.PI * 2f; //x2 para tener el circulo completo 
            float rawSinWave = Mathf.Sin(cycles * tau);
            movementFactor = (rawSinWave / 2f) + 0.5f;
            Vector3 offSet = movementVector * movementFactor;
            transform.position = startingPos + offSet;
        } else {
            return;
        }
    }
}

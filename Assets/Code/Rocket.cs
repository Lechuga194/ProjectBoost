using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour{

    Rigidbody rigidBody;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start(){
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update(){
        ProcessInput();
    }

    /*
    Este metodo servira para procesar las teclas que introduce el usuario
    */
    private void ProcessInput() {
        if (Input.GetKey(KeyCode.Space)) {
            rigidBody.AddRelativeForce(Vector3.up);
            RocketSoundState(true);
        } else {
            RocketSoundState(false);
        }

        if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)) {
            transform.Rotate(Vector3.forward);
        }
        
        if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A)) {
            transform.Rotate(-Vector3.forward);
        }
        
    }

    /**
     * Metodo para saber si se debe reproducir el sonido de la nave
     */
    public void RocketSoundState(Boolean shouldPlay) {
        if (shouldPlay && !audioSource.isPlaying) {
            audioSource.Play();
        }

        if (!shouldPlay) {
            audioSource.Pause();
        }
    }
}

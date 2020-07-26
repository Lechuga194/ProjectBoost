using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour{

    Rigidbody rigidBody;
    AudioSource audioSource;
    MeshRenderer render;
    [SerializeField] float rcsThrust = 90f;
    float rotationSpeed;
    [SerializeField] AudioClip engineSound, dyingSound, metaSound; //Sonidos de la nave
    [SerializeField] ParticleSystem engineParticle, dyingParticle, metaParticle; //Sistema de particulas de la nave
    enum State { ALIVE, DYING, TRANSCENDING, CHARGING} //Posibles estados de la nave
    [SerializeField] State state = State.ALIVE; //Por defecto la nave esta viva
    [SerializeField] float loadingTimeNextLevel = 2f;


    // Start is called before the first frame update
    void Start(){
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        render = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update(){
        if(state == State.ALIVE || state == State.CHARGING) {
            ProcessInput();
        }
    }

    //Este metodo sirve para manejar las colisiones de la nave
    private void OnCollisionEnter(Collision collision) {

        //Verificamos si se debe seguir cambiando el estado o es un estado final
        if (state == State.DYING || state == State.TRANSCENDING) {
            return;
        }

        //Verificamos las distintas colisiones del nivel
        switch (collision.gameObject.tag) {
            //Caso para objetos amigables
            case "Friendly":
                print("Not dead");
                break;
            //Caso para plataformas de carga de combustible
            case "Meta":
                StartWin();
                break;
            case "Fuel_Platform":
                print("Charging fuel!");
                state = State.CHARGING;
                break;
            default:
                //Matamos al jugador
                StartDeath();
                break;
        }
        
    }

    private void StartWin() {
        print("Level complete!");
        metaParticle.Play();
        state = State.TRANSCENDING;
        ProcessAudio();
        Invoke("LoadNextScene", loadingTimeNextLevel);
    }

    private void StartDeath() {
        print("Dead");
        dyingParticle.Play();
        state = State.DYING;
        ProcessAudio();
        Invoke("LoadNextScene", loadingTimeNextLevel);
    }

    /**
     * Metodo para cargar la siguiente escenena
     */
    private void LoadNextScene(){
        if(state == State.TRANSCENDING) {
            SceneManager.LoadScene(1);
        }

        if(state == State.DYING) {
            SceneManager.LoadScene(0);
        }
        
    }

    /*
    Este metodo servira para procesar las teclas que introduce el usuario
    */
    private void ProcessInput() {

        rigidBody.freezeRotation = true; //Control manual de la rotacion

        rotationSpeed = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.Space)) {
            Thrust();
        } else {
            audioSource.Stop();
            engineParticle.Stop();
        }

        if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)) {
            transform.Rotate(Vector3.forward * rotationSpeed);
        }

        if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A)) {
            transform.Rotate(-Vector3.forward * rotationSpeed);
        }

        rigidBody.freezeRotation = false; //Control fisico de la rotacion

    }

    /**
     * Aplica la fuerza para volar y el audio de la nave
     */
    private void Thrust() {
        rigidBody.AddRelativeForce(new Vector3(0, 10, 0));
        if (!audioSource.isPlaying) {
            audioSource.PlayOneShot(engineSound);
        }
        if (!engineParticle.isPlaying) {
            engineParticle.Play();
        }
    }

    /**
     * Metodo para saber si se debe reproducir el sonido de la nave
     */
    public void ProcessAudio() {

        audioSource.Stop();

        if (state == State.DYING) {
            audioSource.PlayOneShot(dyingSound);
        }

        if (state == State.TRANSCENDING) {
            audioSource.PlayOneShot(metaSound);
        }

    }
}

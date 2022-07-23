using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region SerializeField Variables
    [SerializeField] float moveSpeed; //Variable de velocidad de movimiento.
    [SerializeField] float rotationSpeed; //Variable de velocidad de rotación.
    #endregion

    #region Private Variables
    private Rigidbody _rb; //Variable para el rigidbody.
    private Animator _anim; //Variable para el animator.
    private LevelManager levelManager; //Variable para comunicación con levelManager.

    private float moveForward; //Variable para el movimiento vertical.
    private float moveRotation; //Variable parar el movimiento de rotación.
    #endregion

    #region Public Variables
    public bool isWalking; //Booleano para saber si el player se está moviendo.
    #endregion

    #region Monobehaviour method
    private void Start()
    {
        levelManager = FindObjectOfType<LevelManager>(); //Guardamos el levelManager.
        _rb = GetComponent<Rigidbody>(); //Guardamos el rigidbody.
        _anim = GetComponentInChildren<Animator>(); //Guardamos el animator.
    }

    private void Update()
    {
        if(levelManager.GameState != GameState.GAME) { return; } //Si no estamos en el juego, no nos movemos.
        moveForward = Input.GetAxisRaw("Vertical"); //Guardamos el axis vertical.
        moveRotation = Input.GetAxis("Horizontal") * rotationSpeed; //Guardamos el axis horizontal.
    }

    private void FixedUpdate()
    {
        if (moveForward != 0 || moveRotation != 0) //Si alguno de los dos es diferente de 0 nos movemos.
        {
            float finalSpeed; //Variable de la velocidad final.
            if (moveForward < 0) { finalSpeed = 1; } //Si vamos hacia atrás reducimos el movimiento.
            else { finalSpeed = moveSpeed; } //Si no, el movimiento es normal.
            _rb.velocity = transform.TransformDirection(Vector3.forward) * moveForward * finalSpeed; //Movimiento.
            transform.Rotate(Vector3.up * moveRotation); //Rotación
            isWalking = true; //Seteamos que nos estamos moviendo.
        }
        else
        {
            isWalking = false; //Si no, no nos estamos moviento.
        }
    }

    private void LateUpdate()
    {
        _anim.SetBool("isWalking", isWalking); //Seteamos el animator.
        _anim.SetFloat("MoveX", moveForward); //Seteamos el animator.
    }
    #endregion

    #region Triggers
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Key" || other.gameObject.tag == "Secret") //Si hacemos trigger con una llave o un secreto.
        {
            levelManager.ItemFind(other.gameObject.tag); //Llamamos a la función.
            Destroy(other.gameObject); //Destruimos el objeto.
        }
        if (other.gameObject.tag == "Exit") //Si es con la salida.
        {
            levelManager.Win(); //Llamamos a la función de ganar.
        }
    }
    #endregion
}

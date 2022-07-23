using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState { PATROL, ALERT, ATTACK} //Enumerado de estados del enemigo.
public class EnemyBehaviour : MonoBehaviour
{
    #region SerializeField Variables
    [SerializeField] float speed; //Velocidad del enemigo.
    [SerializeField] Transform[] patrolPoint; //Puntos de patrulla.
    [SerializeField] GameObject player; //Transform del jugador.
    #endregion

    #region Private Variable
    public EnemyState enemyState; //Estados del enemigo.

    int index; //Del array de puntos de patrulla.

    Vector3 FinalDestination; //Vector de destino.

    float alertTime; //Tiempo de alerta.
    #endregion
    #region Monobehaviour Method
    private void Start()
    {
        ChangeStage(EnemyState.PATROL); //Inicializamos al enemigo en patrulla.
        index = 0; //Colocamos el index en 0.
        alertTime = 0; //Tiempo de alerta también en 0.
        SetDestination(); //Seteamos el movimiento del enemigo.
    }
    private void Update()
    {
        Behaviour(); //Llamamos al comportamiento a cada frame.
    }

    #endregion
    #region Private Method
    /// <summary>
    /// Método que indica cual es el destino del enemigo.
    /// </summary>
    private void SetDestination()
    {
        switch (enemyState) //Chequeamos el enemyState.
        {
            case EnemyState.ALERT: //Si está en alerta se queda en el lugar. 
                FinalDestination = this.transform.position;
                break;
            case EnemyState.ATTACK: //Si está atacando persigue al player.
                FinalDestination = player.transform.position;
                break;
            case EnemyState.PATROL: //Si está patrullando marca los puntos de patrulla.
                if (index == patrolPoint.Length -1) { index = 0; } //Si el index es igual al tamaño del array lo inicializa.
                else { index++; } //Si no, le suma 1.
                FinalDestination = patrolPoint[index].position; //Setea el destino.
                break;
        }
    }
    /// <summary>
    /// Método que cambia el estado del enemigo.
    /// </summary>
    /// <param name="state"> Estado al que cambia.</param>
    private void ChangeStage(EnemyState state)
    {
        enemyState = state; //Cambiamos el estado.
    }
    /// <summary>
    /// Método para el comportamiento del enemigo.
    /// </summary>
    private void Behaviour()
    {
        if (enemyState == EnemyState.PATROL) //Si está patrullando.
        {
            if (Vector3.Distance(FinalDestination, transform.position) < 1) { SetDestination(); } //Si la distancia con el destino es pequeña, cambia de punto.
            transform.position = Vector3.MoveTowards(transform.position, FinalDestination, speed * Time.deltaTime); //Se mueve hacia allí
        }
        if (enemyState == EnemyState.ALERT) //Si su estado es en alerta.
        {
            alertTime += Time.deltaTime; //Empezamos el contador.
            if (alertTime >= 3) //Si es mayor que tres.
            {
                transform.Rotate(Vector3.up * 90); //El enemigo rota 90º
                alertTime = 0; //Reseteamos el tiempo.
            }
            Debug.DrawRay(transform.position, transform.forward * 3, Color.red); //Rayo para testeo.
            RaycastHit hit; //Creamos la variable del raycast.
            if (Physics.Raycast(this.transform.position, transform.forward * 3, out hit)) //Lanzamos el rayo.
            {
                if (hit.collider.tag == "Player") { ChangeStage(EnemyState.ATTACK); } //Si golpea con el player cambiamos a ataque.
            }
        }
        if (enemyState == EnemyState.ATTACK) //Si está atacando.
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime); //Vamos hacia el player.
        }
    }
    #endregion
    #region Trigger
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Debug.Log(other.tag);
            ChangeStage(EnemyState.ALERT);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player") ChangeStage(EnemyState.PATROL);
    }
    #endregion

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState { PATROL, ALERT, ATTACK}
public class EnemyBehaviour : MonoBehaviour
{
    #region SerializeField Variables
    [SerializeField] float speed;
    [SerializeField] Transform[] patrolPoint;
    [SerializeField] GameObject player;
    #endregion

    #region Private Variable
    public EnemyState enemyState;
    int index;
    Vector3 FinalDestination;
    float alertTime;
    #endregion
    #region Monobehaviour Method
    private void Start()
    {
        ChangeStage(EnemyState.PATROL);
        index = 0;
        alertTime = 0;
        SetDestination();
    }
    private void Update()
    {
        Behaviour();
    }

    #endregion
    #region Private Method
    private void SetDestination()
    {
        switch (enemyState)
        {
            case EnemyState.ALERT:
                FinalDestination = this.transform.position;
                break;
            case EnemyState.ATTACK:
                FinalDestination = player.transform.position;
                break;
            case EnemyState.PATROL:
                if (index == patrolPoint.Length -1) { index = 0; }
                else { index++; }
                FinalDestination = patrolPoint[index].position;
                break;
        }
    }

    private void ChangeStage(EnemyState state)
    {
        enemyState = state;
    }

    private void Behaviour()
    {
        if (enemyState == EnemyState.PATROL)
        {
            if (Vector3.Distance(FinalDestination, transform.position) < 1) { SetDestination(); }
            transform.position = Vector3.MoveTowards(transform.position, FinalDestination, speed * Time.deltaTime);
        }
        if (enemyState == EnemyState.ALERT)
        {
            alertTime += Time.deltaTime;
            if (alertTime >= 3)
            {
                transform.Rotate(Vector3.up * 90);
                alertTime = 0;
            }
            Debug.DrawRay(transform.position, transform.forward * 3, Color.red);
            RaycastHit hit;
            if (Physics.Raycast(this.transform.position, transform.forward * 3, out hit))
            {
                if (hit.collider.tag == "Player") { ChangeStage(EnemyState.ATTACK); }
            }
        }
        if (enemyState == EnemyState.ATTACK)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
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

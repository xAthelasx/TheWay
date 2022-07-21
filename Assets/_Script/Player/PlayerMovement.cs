using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region SerializeField Variables
    [SerializeField] float moveSpeed;
    [SerializeField] float rotationSpeed;
    #endregion

    #region Private Variables
    private Rigidbody _rb;
    private Animator _anim;
    private LevelManager levelManager;

    private float moveForward;
    private float moveRotation;
    #endregion

    #region Public Variables
    public bool isWalking;
    #endregion

    #region Monobehaviour method
    private void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if(levelManager.GameState != GameState.GAME) { return; }
        moveForward = Input.GetAxisRaw("Vertical");
        moveRotation = Input.GetAxis("Horizontal") * rotationSpeed;
    }

    private void FixedUpdate()
    {
        if (moveForward != 0 || moveRotation != 0)
        {
            float finalSpeed;
            if (moveForward < 0) { finalSpeed = 1; }
            else { finalSpeed = moveSpeed; }
            _rb.velocity = transform.TransformDirection(Vector3.forward) * moveForward * finalSpeed;
            transform.Rotate(Vector3.up * moveRotation);
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }
    }

    private void LateUpdate()
    {
        _anim.SetBool("isWalking", isWalking);
        _anim.SetFloat("MoveX", moveForward);
    }
    #endregion

    #region Triggers
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Key" || other.gameObject.tag == "Secret")
        {
            levelManager.ItemFind(other.gameObject.tag);
            Destroy(other.gameObject);
        }
    }
    #endregion
}

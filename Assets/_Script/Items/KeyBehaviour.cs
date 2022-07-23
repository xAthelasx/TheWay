using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBehaviour : MonoBehaviour
{
    #region SerializeField Variables
    [SerializeField] float rotationSpeed; //Variable de velocidad de rotación.
    #endregion
    private void Update()
    {
        transform.Rotate(Vector3.one * rotationSpeed * Time.deltaTime); //Movimiento de rotación en todos los sentidos.
    }
}

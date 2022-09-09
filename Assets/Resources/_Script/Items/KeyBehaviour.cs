using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBehaviour : MonoBehaviour
{
    #region SerializeField Variables
    [SerializeField] float rotationSpeed; //Variable de velocidad de rotaci�n.
    #endregion
    private void Update()
    {
        if (this.gameObject.tag == "Key") { transform.Rotate(Vector3.one * rotationSpeed * Time.deltaTime); } //Movimiento de rotaci�n en todos los sentidos.
        else { transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime); }
    }
}

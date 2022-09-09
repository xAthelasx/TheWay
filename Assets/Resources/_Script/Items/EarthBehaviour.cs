using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthBehaviour : MonoBehaviour
{
    [SerializeField] float speedRotation;
    void Update()
    {
        transform.Rotate(Vector3.up * speedRotation * Time.deltaTime);
    }
}

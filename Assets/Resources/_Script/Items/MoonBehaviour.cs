using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonBehaviour : MonoBehaviour
{
    [SerializeField] float speedRotate;
    [SerializeField] GameObject earth;

    private void Update()
    {
        transform.RotateAround(earth.transform.position, Vector3.up, speedRotate * Time.deltaTime);
    }
}

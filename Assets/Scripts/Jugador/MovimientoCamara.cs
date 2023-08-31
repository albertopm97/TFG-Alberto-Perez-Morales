using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoCamara : MonoBehaviour
{

    //desde el inspector arrastraremos el componente transform del jugador para conectarlos
    //(objetivo representa el lugar donde debe apuntar la camara)
    public Transform objetivo;

    //Para poder dar algo de flexibilidad a la camara
    public Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = objetivo.position + offset;
    }
}

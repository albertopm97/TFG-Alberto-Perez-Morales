using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComportamientoEspada : ComportamientoProyectiles
{

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {

        //Ajustamos la posicion de las espadas en funcion de la direccion y la rapidez
        transform.position += direccion * rapidezActual * Time.deltaTime;
    }
}

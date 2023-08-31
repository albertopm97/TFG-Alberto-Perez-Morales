using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorAnillo : ControladorArmas
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    protected override void Atacar()
    {
        //Hacemos aparecer el anillo en la posicion del pivote (Controlador Anillo --> hijo de player)
        GameObject anillo = Instantiate(estadisticas.Prefab);
        anillo.transform.position = transform.position;

        //para que aparezca el jugador por encima del anillo
        anillo.transform.parent = transform;
        base.Atacar();
    }
}

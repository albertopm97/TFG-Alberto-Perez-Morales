using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorEspada : ControladorArmas
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    protected override void Atacar()
    {
        //Primero creamos un objeto nuevo con el prefab correspondiente
        GameObject nuevaEspada = Instantiate(estadisticas.Prefab);

        //Asignamos la posicion de spawn a la del pivote (Controlador Espada)
        nuevaEspada.transform.position = transform.position;

        //hacemos que la direccion de la espada sea la misma que la del jugador (editable más adelante si queremos que sea el mob mas cercano)
        nuevaEspada.GetComponent<ComportamientoEspada>().ComprobadorDireccion(mj.ultimoMovimiento);

        //editado para que ataque al mas cercano
        //nuevaEspada.GetComponent<ComportamientoEspada>().ComprobadorDireccion(enemigoMasCercano.position);
        base.Atacar();
    }
}

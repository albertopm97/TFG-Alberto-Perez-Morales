using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoEnemigos : MonoBehaviour
{
    EstadisticasEnemigos enemigo;
    public Transform jugador;

    // Start is called before the first frame update
    void Start()
    {
        enemigo = GetComponent<EstadisticasEnemigos>();
        //jugador = FindObjectOfType<MovimientoJugador>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        //Usamos la funcion MoveTowards para seguir al jugador 
        transform.position = Vector2.MoveTowards(transform.position, jugador.transform.position, enemigo.rapidezActual * Time.deltaTime);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pocion : Recolectable, IColeccionable
{
    public int vidaACurar;

    public void coger()
    {
        //EstadisticasJugador jugador = FindObjectOfType<EstadisticasJugador>();
        jugador.curar(vidaACurar);
    }
}

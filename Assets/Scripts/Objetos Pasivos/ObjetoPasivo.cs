using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjetoPasivo : MonoBehaviour
{
    protected EstadisticasJugador jugador;
    public ScriptableObjectObjetoPasivo datosObjeto;

    // Start is called before the first frame update
    void Start()
    {
        jugador = FindObjectOfType<EstadisticasJugador>();
        aplicarObjeto();
    }

    protected virtual void aplicarObjeto()
    {

    }
}

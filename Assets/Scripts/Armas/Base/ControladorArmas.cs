using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorArmas : MonoBehaviour
{

    //Con la directiva Header podemos agrupar variables
    [Header("Estadisticas Armas")]
    public ScriptableObjectArma estadisticas;
    float enfriamientoActual;


    protected MovimientoJugador mj;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        //Referenciamos el script movimiento del jugador
        //mj = FindObjectOfType<MovimientoJugador>();
        mj = GetComponentInParent<MovimientoJugador>();

        //Nos aseguramos de que no se active un ataque automaticamente al comenzar
        enfriamientoActual = estadisticas.Enfriamiento;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //Reducimos el tiempo de enfriamiento en bucle en el juego y Atacamos cuando llega a 0 (El metodo atacar se encargará de resetear el enfiramiento)
        enfriamientoActual -= Time.deltaTime;
        if(enfriamientoActual < 0f)
        {
            Atacar();
        }
    }

    protected virtual void Atacar()
    {
        enfriamientoActual = estadisticas.Enfriamiento;
    }
}

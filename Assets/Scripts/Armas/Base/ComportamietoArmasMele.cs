using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Script base para la implementación de armas mele
/// </summary>

public class ComportamietoArmasMele : MonoBehaviour
{
    public ScriptableObjectArma estadisticas;

    public float tiempoActivo;

    //Estadisticas del arma actuales
    protected float damageActual;
    protected float rapidezActual;
    protected float enfriamientoActual;
    protected float perforacionActual;

    public float getDamageActual()
    {
        return damageActual *= FindObjectOfType<EstadisticasJugador>().poderActual;
    }
    private void Awake()
    {
        damageActual = estadisticas.Damage;
        rapidezActual = estadisticas.Rapidez;
        enfriamientoActual = estadisticas.Enfriamiento;
        perforacionActual = estadisticas.Perforacion;
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        Destroy(gameObject, tiempoActivo);
    }

    protected virtual void Update()
    {

    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        //Nos aseguramos de que la colision sea a un enemigo y si es asi, usamos la funcion recibir daño de su script
        if (collision.CompareTag("Enemigos"))
        {
            EstadisticasEnemigos enemigo = collision.GetComponent<EstadisticasEnemigos>();
            enemigo.recibirAtaque(getDamageActual());
        }
        else if (collision.CompareTag("Destruible"))
        {
            if (collision.gameObject.TryGetComponent(out Destruibles destruible))
            {
                destruible.recibirAtaque(getDamageActual());
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComportamientoProyectiles : MonoBehaviour
{

    /// <summary>
    /// Script base para el comportamiento de todos los proyectiles (ponerlo en el prefab de las armas que son proyectiles)
    /// </summary>

    public ScriptableObjectArma estadisticas;

    protected Vector3 direccion;
    public float tiempoActivo;

    //Estadisticas actuales del arma
    protected float damageActual;
    protected float rapidezActual;
    protected float enfriamientoActual;
    protected float perforacionActual;

    private void Awake()
    {
        damageActual = estadisticas.Damage;
        rapidezActual = estadisticas.Rapidez;
        enfriamientoActual = estadisticas.Enfriamiento;
        perforacionActual = estadisticas.Perforacion;
    }

    public float getDamageActual()
    {
        return damageActual *= FindObjectOfType<EstadisticasJugador>().poderActual;
    }


    // Start is called before the first frame update
    protected virtual void Start()
    {
        //Destruimos el proyectil pasado el tiempo limite de actividad
        Destroy(gameObject, tiempoActivo);
    }

    public void ComprobadorDireccion(Vector3 dir)
    {
        direccion = dir;

    
        float dirX = direccion.x;
        float dirY = direccion.y;

        Vector3 scale = transform.localScale;
        Vector3 rotation = transform.rotation.eulerAngles;

        //Jugamos con la ecala multiplicada en negativo para girar el modelo sin aplicar rotacion al prefab
        if(dirX < 0 && dirY == 0) //Izquierda
        {
            scale.x = scale.x * -1;
            scale.y = scale.y * -1;
        }
        else if (dirX == 0 && dirY > 0) //Arriba
        {
            scale.x = scale.x * -1;
        }
        else if (dirX == 0 && dirY < 0) // Abajo
        {
            scale.y = scale.y * -1;
        }
        else if (dirX > 0 && dirY > 0) //Arriba derecha -- Para las direcciones diagonales es necesario tocar rotacion (por defecto -45 para ser recto hacia derecha)
        {
            rotation.z = 0f;
        }
        else if (dirX > 0 && dirY < 0) //Abajo derecha
        {
            rotation.z = -90f;
        }
        else if (dirX < 0 && dirY < 0) //Abajo izquierda
        {
            rotation.z = -180f;
        }
        else if (dirX < 0 && dirY > 0) //Arriba izquierda
        {
            rotation.z = -270f;
        }

        transform.localScale = scale;

        //Para ajustar la rotacion es necesario convertir Quaternion a un Vector3. podemos hacerlo usando la funcion Euler
        transform.rotation = Quaternion.Euler(rotation);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        //Nos aseguramos de que la colision sea a un enemigo y si es asi, usamos la funcion recibir daño de su script
        if (collision.CompareTag("Enemigos"))
        {
            EstadisticasEnemigos enemigo = collision.GetComponent<EstadisticasEnemigos>();
            enemigo.recibirAtaque(getDamageActual());
            reducirPerforacion();
        }
        else if (collision.CompareTag("Destruible"))
        {
            if(collision.gameObject.TryGetComponent(out Destruibles destruible))
            {
                destruible.recibirAtaque(getDamageActual());
                reducirPerforacion();
            }
        }
    }

    //Funcion para controlar la perforacion, si llega a 0 se destrulle el proyectil
    void reducirPerforacion()
    {
        perforacionActual--;
        if(perforacionActual <= 0)
        {
            Destroy(gameObject);
        }
    }
}

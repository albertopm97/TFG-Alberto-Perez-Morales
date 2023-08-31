using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestorRecogidaJugador : MonoBehaviour
{

    EstadisticasJugador jugador;
    CircleCollider2D areaIman;

    //Para el efecto de atraccion
    Transform posItem;
    public float velocidadAtraccion;
    

    void Start()
    {
        //jugador = FindObjectOfType<EstadisticasJugador>();
        jugador = GetComponentInParent<EstadisticasJugador>();
        areaIman = GetComponent<CircleCollider2D>();
    }


    void Update()
    {
        areaIman.radius = jugador.imanObjetosActual;

        if(posItem != null)
        {
            posItem.position = Vector2.MoveTowards(posItem.position, jugador.transform.position, velocidadAtraccion * Time.deltaTime);
        }
        
    }

    //Comprobamos si hay una colisión con un coleccionable, y si lo hay lo cogemos (un coleccionable es una clase que implementa IColeccionable)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out IColeccionable item))
        {
            posItem = collision.GetComponent<Transform>();

            /*Forma bugeada del iman
            //Calculamos la direccion en la que aplicar la atraccion del item
            Vector2 direccionIman = (transform.position - collision.transform.position).normalized;
            //aplicamos la fuerza
            rb.AddForce(direccionIman * velocidadAtraccion);*/
            
            //item.coger();
        }
    }
}

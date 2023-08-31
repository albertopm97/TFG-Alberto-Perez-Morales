using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComportamientoAnillo : ComportamietoArmasMele
{
    //Para controlar que los enemigos solo reciban daño del area 1 vez
    List<GameObject> enemigosTocados;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        enemigosTocados = new List<GameObject>();
    }

    // Start is called before the first frame update
    protected override void Update()
    {
        Vector3 rotacion = transform.rotation.eulerAngles;

        rotacion.z += 1f;

        //Para ajustar la escala es necesario convertir Quaternion a un Vector3. podemos hacerlo usando la funcion Euler
        transform.rotation = Quaternion.Euler(rotacion);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemigos") && !enemigosTocados.Contains(collision.gameObject))
        {
            EstadisticasEnemigos enemigo = collision.GetComponent<EstadisticasEnemigos>();
            enemigo.recibirAtaque(damageActual);

            //Añadimos al enemigo a la lista de enemigos tocados
            enemigosTocados.Add(collision.gameObject);
        }
        else if (collision.CompareTag("Destruible") && !enemigosTocados.Contains(collision.gameObject))
        {
            if (collision.gameObject.TryGetComponent(out Destruibles destruible))
            {
                destruible.recibirAtaque(damageActual);
                enemigosTocados.Add(collision.gameObject);
            }
        }

    }

}

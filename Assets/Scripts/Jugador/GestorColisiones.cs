using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestorColisiones : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out IColeccionable item))
        {
            item.coger();
        }
    }
}

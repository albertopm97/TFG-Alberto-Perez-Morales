using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destruibles : MonoBehaviour
{
    public float resistencia;

    public void recibirAtaque(float dmg)
    {
        resistencia -= dmg;

        if(resistencia <= 0)
        {
            destruir();
        }
    }

    public void destruir()
    {
        Destroy(gameObject);
    }


}

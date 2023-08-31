using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimacionesJugador : MonoBehaviour

{
    //Para referencias
    Animator am;
    SpriteRenderer sr;
    MovimientoJugador mj;


    // Start is called before the first frame update
    void Start()
    {
        am = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        mj = GetComponent<MovimientoJugador>();
    }

    // Update is called once per frame
    void Update()
    {
        if(mj.direccion.x != 0 || mj.direccion.y != 0)
        {
            am.SetBool("Mover", true);

            ComprobarDireccionSprite();
        }
        else
        {
            am.SetBool("Mover", false);
        }
    }

    //Para no definir animaciones distintas, simplemente invertimos la animacion de andar al ir a la izquierda (x es negativo)
    void ComprobarDireccionSprite()
    {
        if(mj.ultimoVectorHorizontal < 0)
        {
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }
    }
}

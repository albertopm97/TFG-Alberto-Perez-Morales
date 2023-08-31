using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjetoPasivoPollo : ObjetoPasivo
{
    protected override void aplicarObjeto()
    {
        jugador.poderActual *= datosObjeto.Multiplicador / 100f;
    }
}

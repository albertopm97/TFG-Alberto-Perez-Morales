using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjetoPasivoBotas : ObjetoPasivo
{
    protected override void aplicarObjeto()
    {
        jugador.velocidadMovimientoActual *= 1 + datosObjeto.Multiplicador / 100f;
    }
}

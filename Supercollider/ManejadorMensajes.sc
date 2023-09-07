(
/*
* Rutina asíncrona, encargada de iniciar la musica por partes, primero la
* percusión y luego la armonía
*/
~r1 = Routine.new({

    //Iniciamos la percusión
    ~patronBombo.play(~tGeneral, quant: 4);
    ~patronCaja.play(~tGeneral, quant: 2);

    //Esperamos 5 segundos para dar entrada a los arpegios
    wait(5);

    //Iniciamos los arpegios
    ~arpegio.play(~tGeneral, quant: 3);
});

/*
* Funcion auxiliar para realizar un cambio en el tempo de la música
*/
~cambiarTempoMusica = {

    ~tGeneral.tempo = ~tempo;
    ~tMelodia.tempo = ~tempo / 2;
};

/*
* Funcion auxiliar para realizar un cambio en el tempo de la música
*/
~cambiarModoMusica = {

    //Paramos la ejecución del patron que controla los arpegios
    ~arpegio.stop;

    //inicializamos los arpegios usando la nueva secuencia y activamos el patron
    ~prueba.inicializarArpegios(~generador.secuenciaGenerada);
    ~prueba.tocarArpegio;

    //Continuamos la reproducción del patrón
    ~arpegio.play(~tGeneral, quant: 1);
};
/*
* Funcion para controlar la adaptación dinámica del ritmo de la musica en función
* del número de enemigos activos
*/
~adaptarRitmo = {
    arg numEnemigos; //Argumento pasado como parametro a la funcion

    //El número máximo de enemigos activo es de 20, por lo que se ha dividio en rangos de 5 en 5 con
    //ritmo creciente
    case
    { (numEnemigos >= 0) && (numEnemigos < 5) && (~rangoRimoPrevio != 0)}{
        ~tempo = 90/60;
        ~rangoRimoPrevio = 0;
        ~cambiarTempoMusica.value();
    }
    {(numEnemigos >= 5) && (numEnemigos < 10) && (~rangoRimoPrevio != 1)}  {
        ~tempo = 100/60;
        ~rangoRimoPrevio = 1;
        ~cambiarTempoMusica.value();
    }
    {(numEnemigos >= 10) && (numEnemigos < 15) && (~rangoRimoPrevio != 2)} {
        ~tempo = 110/60;
        ~rangoRimoPrevio = 2;
        ~cambiarTempoMusica.value();
    }
    {(numEnemigos >= 15) && (numEnemigos < 20) && (~rangoRimoPrevio != 3)} {
        ~tempo = 120/60;
        ~rangoRimoPrevio = 3;
        ~cambiarTempoMusica.value();
    };
};

/*
* Funcion para controlar la adaptación dinámica del modo de la secuencia de acordes en función
* del % de vida del jugador (modos mayores para +50% y menores para -50%)
*/
~adaptarModos = {
    arg vidaActual; //Argumento pasado como parametro a la funcion


    case
    { (vidaActual >= 40) && (~rangoPrevioModo != 0)}{ //La vida del jugador vuelve a esar por encima del 40%

        //Recuperamos la secuencia original
        ~generador.secuenciaGenerada = ~secuenciaOriginal;
        ~rangoPrevioModo = 0;
        ~cambiarModoMusica.value();
    }
    { (vidaActual < 40) && (~rangoPrevioModo != 1)}{ //La vida del jugador cae por debajo del 40%

        //Cambiamos la secuencia a
        ~generador.cambiarModoSecuencia('m');
        ~rangoPrevioModo = 1;
        ~cambiarModoMusica.value();
    };
};


/*
* Función para controlar la activación o desactivación del filtro de resonancia en cuando haya o no
* algún mini jefes activo en la partida
*
* Como parámetro a la función tocarArpegio podemos pasar el valor filterQ del filtro
* para activarlo en mayor o menor medida. Este valor debe ser mayor que 0 y menor de 10, por
* tanto, y para hacer más dinámica la música, lo calculamos en función del número de min jefes.
* Así, usaremos el número de mini jefes como valor filterQ hasta un máximo de 5, a partir de ese valor
* el efecto del filtro se vuelve demasiado agresivo
*/
~cambioMinibosses = {
    arg numJefes;

    ~filterQ = numJefes;

    if(~filterQ > 5){
        ~filterQ = 5;
    };


    case
    { (numJefes > 0) && (~resonanciaActiva == false)}{

        //Paramos la ejecución del patrón
        ~arpegio.stop;

        //Volvemos a inicicializar el patrón de los arpegios pasando el valor de filterQ
        ~prueba.tocarArpegio(~filterQ);

        //Reactivamos los arpegios
        ~arpegio.play(~tGeneral, quant: 1);

        //Por ultimo indicamos que la resonancia está activa
        ~resonanciaActiva = true;
    }
    { (numJefes == 0) && (~resonanciaActiva == true)}{

        //Paramos la ejecución del patrón
        ~arpegio.stop;

        //Volvemos a inicicializar el patrón de los arpegios. Al no pasar argumento, se
        // usa por defecto el valor 0.01 de filterQ para desactivar el filtro
        ~prueba.tocarArpegio();

        //Reactivamos los arpegios
        ~arpegio.play(~tGeneral, quant: 1);

        //Por ultimo indicamos que la resonancia está desactivada
        ~resonanciaActiva = false;
    };
};

/*
* Manejador para el mensaje de inicio de musica al comienzo del juego
*/
OSCdef.new(
    \iniciarMusica,             //Simbolo para identificar el manejador
    {arg msg;
        [msg].postln;  //Función que indica que hacer al recibir el mensaje

        //inicializamos Generador de secuencias y sus diccionarios y creamos la secuencia de acordes
        ~generador = GeneradorSecuencias();
        ~generador.inicializarDiccionarios();
        ~generador.inicializarGrafoTransiciones();

        //Generamos una secuencia de tamaño aleatorio entre 20 y 50 acordes
        ~generador.generarSecuencia(rrand(20, 50));

        //Almacenamos la secuencia original para recuperarla si cambiamos de modo
        ~secuenciaOriginal = ~generador.secuenciaGenerada;

        //Varible necesaria para gestionar los cambios de ritmo y modo dinámicos
        ~rangoRimoPrevio = 0;
        ~rangoPrevioModo = 0;

        //Variable necesaria para gestionar la activación dinámica del filtro de resonancia
        ~resonanciaActiva = false;

        //Seleccionamos aleatoriamente una escala (do, re, mi, ... si).
        ~n = ['C', 'D', 'E', 'F', 'G', 'A', 'B'].choose;

        //Transformamos la secuencia generada de grados a notas de la escala elegida
        ~generador.transformarSecuecia(~n);

        //Inicializamos el audio en la clase generadora de acordes
        C.audio;

        //Creamos objeto de la clase generador de acordes
        ~prueba = C();

        //Limpiamos la memoria de buses y creamos uno para mantener el sintetizador de reverb
        //De esta forma, todo lo que mandemos sonar en ese bus, tendrá reverb aplicado
        s.newBusAllocators;
        ~bus = Bus.audio(s, 2);

        //Inicializamos los sintetizadores y los arpegios necesarios para la música
        ~prueba.inicializarSynths();
        ~prueba.inicializarArpegios(~generador.secuenciaGenerada);

        //Variable para controlar el ritmo de la música.
        ~tempo = 90/60;

        //Inicializamos los relojes que controlaran el tempo para sincronizar percusion y acordes
        ~tGeneral = TempoClock.new(~tempo).permanent_(true);
        ~tMelodia = TempoClock.new(~tempo/2).permanent_(true);

        //inicializamos el patron que reproducen los arpegios
        ~prueba.tocarArpegio;

        //Activamos el reverb en el bus
        Synth(\reverb, [\in, ~bus, \out, 0]);

        //Inicializamos los patrones que reproducen la percusión
        ~prueba.tocarPercusion();

        //Inicializamos el patron que reproduce las melodías
        ~prueba.tocarMelodia();

        //creamos un array vacio para almacenar todos los reproductores de las melodías
        ~melodias = [];

        //Activamos la rutina que inicia la música
        ~r1.play;
    },
    '/iniciarMusica', //Mensaje a leer
    nil,
    57120  //Puerto para escuchar los mensajes (indicado en OSCHandler en Unity)
);

/*
* Manejador para el mensaje de fin de musica al finalizar el juego
*/
OSCdef.new(
    \finalizarMusica,
    {arg msg;
        [msg].postln;

        //Para parar la música simplemente interrumpimos la ejecución de los patrones
        ~patronBombo.stop;
        ~patronCaja.stop;
        ~arpegio.stop;

        ~melodias.do{
            arg item;

            item.stop;
        };
    },
    '/finalizarMusica',
    nil,
    57120
);

/*
* Manejador para el mensaje de cambio en los enemigos activos
* Usada para adaptar el ritmo de la música de forma dinámica en función del número de enemigos
*/
OSCdef.new(
    \cambioEnemigosActivos,
    {arg msg;
        [msg].postln;

        //Extraemos el valor pasado por mensaje
        ~enemigosActivos = [msg][0][1].postln;

        //Llamamos a la función que adapta el ritmo pasando como parametro el numero de enemigos
        ~adaptarRitmo.value(~enemigosActivos);
    },
    '/cambioEnemigosActivos',
    nil,
    57120
);

/*
* Manejador para el mensaje de cambio en la vida actual del jugador
* Usada para adaptar el modo de la secuencia de forma dinámica
*/
OSCdef.new(
    \cambioVidaActual,
    {arg msg;
        [msg].postln;

        //Extraemos el valor pasado por mensaje
        ~vidaActual = [msg][0][1].postln;

        //Llamamos a la función que adapta el ritmo pasando como parametro la vida actual del jugador
        ~adaptarModos.value(~vidaActual);
    },
    '/cambioVidaActual',
    nil,
    57120
);


/*
* Manejador para el mensaje de subida de nivel
* Usada para añadir nuevas capas melódicas de forma dinámica
*/
OSCdef.new(
    \lvUp,
    {arg msg;
        [msg].postln;

       //Iniciamos una nueva melodia, añadiendo su reproductor al array de melodías
        ~melodias = ~melodias.add(~melodia.play(~tMelodia, quant: 4));
    },
    '/lvUp',
    nil,
    57120
);

/*
* Manejador para el mensaje de cambio en la vida actual del jugador
* Usada para adaptar el modo de la secuencia de acordes entre mayor y menor de forma dinámica
*/
OSCdef.new(
    \cambioMinibosses,
    {arg msg;
        [msg].postln;

        //Extraemos el valor pasado por mensaje
        ~numeroBosses = [msg][0][1].postln;
        ~cambioMinibosses.value(~numeroBosses);
    },
    '/cambioMinibosses', //Mensaje a leer
    nil,
    57120
);
)
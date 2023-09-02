(
/*
* Rutina asíncrona, encargada de iniciar la musica por partes, primero la percusión y luego la armonía
*/
~r1 = Routine.new({

    //Iniciamos la percusión por partes, primero bombo y caja y por ultimo tom
    ~patronBombo.play(~tGeneral, quant: 4);
    ~patronCaja.play(~tGeneral, quant: 2);
    wait(5);
    ~arpegio.play(~tGeneral, quant: 3);
});

/*
* Funcion auxiliar para realizar un cambio en la música
*/
~cambiarMusica = {

    ~patronBombo.stop;
    ~patronCaja.stop;
    ~arpegio.stop;

    ~numMelodias = ~melodias.size;

    //Si hay melodias sonando, adaptar su ritmo
    if(~numMelodias != 0){
        ~melodias.do{
            arg item;

            item.stop;
        };

        ~melodias = [];
        for(0, ~numMelodias - 1){

            ~melodias.add(~melodia.play(~tMelodia, quant: 4));
        };
    };

    //Adaptar el ritmo de todos los patrones
    ~patronBombo.play(~tGeneral, quant: 4);
    ~patronCaja.play(~tGeneral, quant: 2);

    ~arpegio.play(~tGeneral, quant: 1);
};

/*
* Funcion para controlar la adaptación dinámica del ritmo de la musica en función
* del número de enemigos activos
*/
~adaptarRitmo = {
    arg numEnemigos; //Argumento pasado como parametro a la funcion


    case
    { (numEnemigos >= 0) && (numEnemigos < 5) && (~rangoRimoPrevio != 0)}{
        ~tempo = 90/60;
        ~rangoRimoPrevio = 0;
        ~tGeneral = TempoClock.new(~tempo).permanent_(true);
        ~tMelodia = TempoClock.new(~tempo/2).permanent_(true);
        ~cambiarMusica.value();
    }
    {(numEnemigos >= 5) && (numEnemigos < 10) && (~rangoRimoPrevio != 1)}  {
        ~tempo = 100/60;
        ~rangoRimoPrevio = 1;
        ~tGeneral = TempoClock.new(~tempo).permanent_(true);
        ~tMelodia = TempoClock.new(~tempo/2).permanent_(true);
        ~cambiarMusica.value();
    }
    {(numEnemigos >= 10) && (numEnemigos < 15) && (~rangoRimoPrevio != 2)} {
        ~tempo = 110/60;
        ~rangoRimoPrevio = 2;
        ~tGeneral = TempoClock.new(~tempo).permanent_(true);
        ~tMelodia = TempoClock.new(~tempo/2).permanent_(true);
        ~cambiarMusica.value();
    }
    {(numEnemigos >= 15) && (numEnemigos < 20) && (~rangoRimoPrevio != 3)} {
        ~tempo = 120/60;
        ~rangoRimoPrevio = 3;
        ~tGeneral = TempoClock.new(~tempo).permanent_(true);
        ~tMelodia = TempoClock.new(~tempo/2).permanent_(true);
        ~cambiarMusica.value();
    };
};

/*
* Funcion para controlar la adaptación dinámica del modo de la secuencia de acordes en función
* del % de vida del jugador (modos mayores para +50% y menores para -50%)
*/
~adaptarModos = {
    arg vidaActual; //Argumento pasado como parametro a la funcion


    case
    { (vidaActual >= 50) && (~rangoPrevioModo != 0)}{
        ~generador.cambiarModoSecuencia('M');
        ~prueba.inicializarArpegios(~generador.secuenciaGenerada);
        ~rangoPrevioModo = 0;

        ~prueba.tocarArpegio;
        ~cambiarMusica.value();
    }
    { (vidaActual < 50) && (~rangoPrevioModo != 1)}{
        ~generador.cambiarModoSecuencia('m');
        ~prueba.inicializarArpegios(~generador.secuenciaGenerada);
        ~rangoPrevioModo = 1;

        ~prueba.tocarArpegio;
        ~cambiarMusica.value();
    };
};


/*
* Función para controlar la activación o desactivación del filtro de resonancia en cuando haya o no
* algún mini boss activo en la partida.
*
* Como parámetro a la función tocarArpegio podemos pasar el valor bwr (ratio de ancho de banda) del
* filtro para activarlo en mayor o menor medida. Este valor debe ser mayor que 0 y menor de 10, por
* tanto, y para hacer más dinámica la música, lo calculamos en función del número de minibosses.
* Así, usaremos el número de minibosses como valor bwr hasta un máximo de 5, a partir de ese valor
* el efecto del filtro se vuelve demasiado agresivo.
*/
~cambioMinibosses = {
    arg numBosses;

    ~valorBwr = numBosses;

    if(~valorBwr > 5){
        ~valorBwr = 5;
    };


    case
    { (numBosses > 0) && (~resonanciaActiva == false)}{

        //Paramos la ejecución del patrón,
        ~arpegio.stop;

        //Volvemos a inicicializar el patron de los arpegios pasando el valor de bwr
        ~prueba.tocarArpegio(~valorBwr);

        //Reactivamos los arpegios
        ~arpegio.play(~tGeneral, quant: 1);

        //Por ultimo indicamos que la resonancia est activa
        ~resonanciaActiva = true;
    }
    { (numBosses == 0) && (~resonanciaActiva == true)}{
        //Paramos la ejecución del patrón,
        ~arpegio.stop;

        //Volvemos a inicicializar el patron de los arpegios. Al no pasar argumento, se
        // usa por defecto el valor 0.01 de bwr para desactivar el filtro
        ~prueba.tocarArpegio();

        //Reactivamos los arpegios
        ~arpegio.play(~tGeneral, quant: 1);

        //Por ultimo indicamos que la resonancia est activa
        ~resonanciaActiva = false;
    };
};

/*
* Manejador para la llamada de inicio de musica al comienzo del juego
*/
OSCdef.new(
    \iniciarMusica,             //Simbolo para identificar el manejador.
    {arg msg;
        [msg].postln;  //Funcion que indica que hacer al recibir el mensaje

        //inicializamos Generador de secuencias y sus diccionarios y creamos la secuencia de acordes
        ~generador = GeneradorSecuencias();
        ~generador.inicializarDiccionarios();
        ~generador.inicializarGrafoTransiciones();
        ~generador.generarSecuencia(20);

        //Varible necesaria para gestionar los cambios de ritmo dinámicos
        ~rangoRimoPrevio = 0;
        ~rangoPrevioModo = 0;

        //Variable necesaria para gestionar la activación dinámica del filtro de resonancia
        ~resonanciaActiva = false;

        //Seleccionamos aleatoriamente una escala (do, re, mi, ... si)
        ~n = ['C', 'D', 'E', 'F', 'G', 'A', 'B'].choose;

        //Transformamos la secuencia generada de grados a notas de una escala concreta
        ~generador.transformarSecuecia(~n);

        //Inicializamos la clase generadora de acordes
        C.audio;

        //Creamos objeto de la clase generador de acordes
        ~prueba = C();

        //Reseteamos y creamos un bus para mantener el sintetizador de reverb
        //De esta forma, todo lo que mandemos sonar en ese bus, tendrá reverb aplicado
        s.newBusAllocators;
        ~bus = Bus.audio(s, 2);

        //Inicializamos los sintetizadores y los arpegios necesarios para la musica
        ~prueba.inicializarSynths();

        //Para inicializar los arpegios, se descomponen los acordes en notas que se tocan separadas
        ~prueba.inicializarArpegios(~generador.secuenciaGenerada);

        //Inicializamos el reloj que controlara el tempo para sincronizar percusion y acordes
        ~tempo = 90/60;
        ~tGeneral = TempoClock.new(~tempo).permanent_(true);
        ~tMelodia = TempoClock.new(~tempo/2).permanent_(true);

        //inicializamos el patron que controla los arpegios
        ~prueba.tocarArpegio;

        //Activamos el reverb en el bus
        Synth(\reverb, [\in, ~bus, \out, 0]);

        //Inicializamo la percusión
        ~prueba.tocarPercusion();

        //Iniciamos patron de melodia
        ~prueba.tocarMelodia();

        //creamos un array vacio para almacenar todos los reproductores de las melodias e iniciamos la primera
        ~melodias = [];

        //Activamos la rutina que inicia la música
        ~r1.play;
    },
    '/iniciarMusica', //Mensaje a leer
    nil,
    57120  //Puerto para escuchar los mensajes (indicado en OSCHandler en Unity)
);

/*
* Manejador para la llamada de fin de musica al finalizar el juego
*/
OSCdef.new(
    \finalizarMusica,             //Simbolo para identificar el manejador.
    {arg msg;
        [msg].postln;  //Funcion que indica que hacer al recibir el mensaje

        ~patronBombo.stop;
        ~patronCaja.stop;
        ~patronTom.stop;
        ~arpegio.stop;
    },
    '/finalizarMusica', //Mensaje a leer
    nil,
    57120  //Puerto para escuchar los mensajes (indicado en OSCHandler en Unity)
);

/*
* Manejador para la llamada de cambio en los enemigos activos.
* Usada para adaptar el ritmo de la música de forma dinámica en función del número de enemigos
*/
OSCdef.new(
    \cambioEnemigosActivos,             //Simbolo para identificar el manejador.
    {arg msg;
        [msg].postln;

        //Extraemos el valor pasado por mensaje
        ~enemigosActivos = [msg][0][1].postln;

        //Llamamos a la función que adapta el ritmo pasando como parametro el numero de enemigos
        ~adaptarRitmo.value(~enemigosActivos);
    },
    '/cambioEnemigosActivos', //Mensaje a leer
    nil,
    57120  //Puerto para escuchar los mensajes (indicado en OSCHandler en Unity)
);

/*
* Manejador para la llamada de cambio en la vida actual del jugador.
* Usada para adaptar el modo de la secuencia de acordes entre mayor y menor de forma dinámica
*/
OSCdef.new(
    \cambioVidaActual,             //Simbolo para identificar el manejador.
    {arg msg;
        [msg].postln;

        //Extraemos el valor pasado por mensaje
        ~vidaActual = [msg][0][1].postln;

        //Llamamos a la función que adapta el ritmo pasando como parametro el numero de enemigos
        ~adaptarModos.value(~vidaActual);
    },
    '/cambioVidaActual', //Mensaje a leer
    nil,
    57120  //Puerto para escuchar los mensajes (indicado en OSCHandler en Unity)
);


/*
* Manejador para la llamada de cambio en la vida actual del jugador.
* Usada para adaptar el modo de la secuencia de acordes entre mayor y menor de forma dinámica
*/
OSCdef.new(
    \lvUp,             //Simbolo para identificar el manejador.
    {arg msg;
        [msg].postln;

        //Extraemos el valor pasado por mensaje
        ~nivel = [msg][0][1].postln;

        //Llamamos a la función que adapta el ritmo pasando como parametro el numero de enemigos
        ~melodias = ~melodias.add(~melodia.play(~tMelodia, quant: 4));
    },
    '/lvUp', //Mensaje a leer
    nil,
    57120  //Puerto para escuchar los mensajes (indicado en OSCHandler en Unity)
);

/*
* Manejador para la llamada de cambio en la vida actual del jugador.
* Usada para adaptar el modo de la secuencia de acordes entre mayor y menor de forma dinámica
*/
OSCdef.new(
    \cambioMinibosses,             //Simbolo para identificar el manejador.
    {arg msg;
        [msg].postln;

        //Extraemos el valor pasado por mensaje
        ~numeroBosses = [msg][0][1].postln;
        ~cambioMinibosses.value(~vidaActual);
    },
    '/cambioMinibosses', //Mensaje a leer
    nil,
    57120  //Puerto para escuchar los mensajes (indicado en OSCHandler en Unity)
);
)
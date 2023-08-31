(
~r1 = Routine.new({

    //Iniciamos la percusión por partes, primero bombo y caja y por ultimo tom
    ~patronBombo.play(~t, quant: 4);
    ~patronCaja.play(~t, quant: 2);
    wait(5);
    ~patronTom.play(~t);
    wait(10);
    ~arpegio.play(~t, quant: 1);
});


OSCdef.new(
    \iniciarMusica,             //Simbolo para identificar el manejador.
    {arg msg;
        [msg].postln;  //Funcion que indica que hacer al recibir el mensaje

        //inicializamos Generador de secuencias y sus diccionarios y creamos la secuencia de acordes
        ~generador = GeneradorSecuencias();
        ~generador.inicializarDiccionarios();
        ~generador.inicializarGrafoTransiciones();
        ~generador.generarSecuencia(50);

        //Seleccionamos aleatoriamente una escala (do, re, mi, ... si)
        ~n = ['C', 'D', 'E', 'F', 'G', 'A', 'B'].choose;

        //Transformamos la secuencia generada de grados a notas de una escala concreta
        ~generador.transformarSecuecia('A');

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
        ~tempo = 100/60;
        ~t = TempoClock.new(~tempo).permanent_(true);

        //inicializamos el patron que controla los arpegios
        ~prueba.tocarArpegio;

        //Activamos el reverb en el bus
        Synth(\reverb, [\in, ~bus, \out, 0]);

        //Inicializamo la percusión
        ~prueba.tocarPercusion();

        ~r1.play;
    },
    '/iniciarMusica', //Mensaje a leer
    nil,
    57120  //Puerto para escuchar los mensajes (indicado en OSCHandler en Unity)
);

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
)
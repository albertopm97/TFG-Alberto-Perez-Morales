// a dirty short name for a simple usage
// chord generator for jazz standard usage
// alpha stuff
C {
	var <>symbol, <>root, <>name, <>structure, <>transp ;
	classvar <>symbols ;
	classvar <>decay, <>release ;

	*initClass {
		var t ;
		decay = 0.5;
		release = 0.5 ;
		symbols = () ;
		t = "CM M Cmaj	{0,4,7}
Cm C- Cmin	{0,3,7}
C+ Caug CM#5 CM+5	{0, 4, 8}
C° Cdim Cmb5 Cm˚5	{0, 3, 6}
C7 Cdom7	{0, 4, 7, 10}
CM7 CMa7 Cj7 Cmaj7	{0, 4, 7, 11}
CmM7 Cm#7 C−M7 Cminmaj7	{0, 3, 7, 11}
Cm7 C-7 Cmin7	{0, 3, 7, 10}
C+M7 Caugmaj7 CM7#5 CM7+5	{0, 4, 8, 11}
C+7 Caug7 C7#5 C7+5	{0, 4, 8, 10}
CØ CØ7 Cø Cø7 Cmin7dim5 Cm7b5 Cm7°5 C−7b5 C−7°5	{0, 3, 6, 10}
Co7 C°7 Cdim7	{0, 3, 6, 9}
C7b5 Cdom7dim5	{0, 4, 6, 10}
CM9 Cmaj9	{0, 4, 7, 11, 14}
C9 Cdom9	{0, 4, 7, 10, 14}
CmM9 C−M9 Cminmaj9	{0, 3, 7, 11, 14}
Cm9 C−9 Cmin9	{0, 3, 7, 10, 14}
C+M9 Caugmaj9	{0, 4, 8, 11, 14}
C+9 C9#5 Caug9	{0, 4, 8, 10, 14}
CØ9	{0, 3, 6, 10, 14}
CØ9b9	{0, 3, 6, 10, 13}
C°9 Cdim9	{0, 3, 6, 9, 14}
C°b9 Cdimb9	{0, 3, 6, 9, 13}
C11 Cdom11	{0, 4, 7, 10, 14, 17}
CM11 Cmaj11	{0, 4, 7, 11, 14, 17}
CmM11 C−M11 Cminmaj11	{0, 3, 7, 11, 14, 17}
Cm11 C−11 Cmin11	{0, 3, 7, 10, 14, 17}
C+M11 Caugmaj11	{0, 4, 8, 11, 14, 17}
C+11 C11#5 Caug11	{0, 4, 8, 10, 14, 17}
CØ11	{0, 3, 6, 10, 13, 17}
C°11	{0, 3, 6, 9, 13, 16}
CM13 Cmaj13	{0, 4, 7, 11, 14, 17, 21}
C13 Cdom13	{0, 4, 7, 10, 14, 17, 21}
CmM13 C−M13 Cminmaj13	{0, 3, 7, 11, 14, 17, 21}
Cm13 C−13 Cmin13	{0, 3, 7, 10, 14, 17, 21}
C+M13 Caugmaj13	{0, 4, 8, 11, 14, 17, 21}
C+13 C13#5 Caug13	{0, 4, 8, 10, 14, 17, 21}
CØ13	{0, 3, 6, 10, 14, 17, 21}
C6 CM6	{0,4,7,6}
Cm6 Cminmaj6	{0,3,7,6}
C7#9	{0, 4, 7, 10, 15}
C7b9	{0, 4, 7, 10, 13}
C7#11	{0, 4, 7, 10, 19}
C7b11	{0, 4, 7, 10, 17}"
		.split($\n)
		.collect{|i| i.split($\t)}.collect{|i|
			[
				i[0].split($\ ).collect{|j| j.replace("C", "").asSymbol},
				i[1].replace("{", "[").replace("}", "]").interpret

			]
		}.collect{|ch|
			ch[0].do{|key|
				symbols[key] = ch[1]
			}
		}

	}

	*new { arg symbol;
		^super.new.initC(symbol)
	}

	*audio {

		Server.local.waitForBoot{
			SynthDef(\piano, {|note = 60, vol = -6, decay, release|
				Out.ar(0,
					MdaPiano.ar(note.midicps, decay:decay, release:release)* vol.dbamp*Line.kr(1,1,6, doneAction:2))
			}).add

		}
	}

	initC { arg aSymbol ;
		var data, sus, add ;
		symbol = aSymbol ;
		transp = 60 ;
		data =  this.getRoot(symbol.asString) ;
		root = data[0] ; name = data[1];
		structure = symbols[symbol.asString.replace(name.asString, "")
			.replace("sus", "@").split($@)[0]
			.replace("add", "@").split($@)[0]
			.asSymbol] ;
		sus = this.processSus(symbol) ;
		if(sus.notNil) {
			structure.remove(3) ;
			structure.remove(4) ;
			structure = structure.add(sus)
		} ;
		add = this.processAdd(symbol).postln ;
		if (add.notNil) {
			structure = structure.add(add)
		}

	}

	processSus {|symbol|
		var sus = nil ;
		if (symbol.asString.contains("sus")){
			sus = symbol.asString.replace("sus", "@").split($@)[1];
			case
			{sus == ""}{sus = 5}
			{sus == [4]}{sus = 5}
			{sus == [2]}{sus = 2}
		} ;
		^sus
	}

	processAdd {|symbol|
		var add = nil ;
		var base ;
		var dict = (\9: 14, \11: 17, \13: 21) ;
		if (symbol.asString.contains("add")){
			add = symbol.asString.replace("add", "@").split($@)[1];
			base = dict[add.replace("b", "").replace("#", "").asSymbol] ;
			add.postln ;
			case {add.split($b)[1].notNil}{
				add = base-1 ;
				"flatten".postln
			}
			{add.split($#)[1].notNil}{
				add = base+1 ;
					"sharpen".postln
			}
			{(add.split($#)[1].isNil) && (add.split($b)[1].isNil) }
			{add = base;
					"no alt".postln
			}
		} ;
		^add
	}

	//c = C('CMadd9') ; // c.structure

	getRoot  { arg symbol ;
		var chr, act, root, rootName, alt ;
		var noteBase = (\C:0,\D:2,\E:4,\F:5,\G:7,\A:9,\B:11) ;
		name = symbol[0] ;
		root = noteBase[name.asSymbol] ;
		if ([$b, $#].includes(symbol[1]) ){
			name = name++symbol[1] ;
			root = root + [-1,1][[$b, $#].indexOf(symbol[1])]
		} ;
		^[root, name.asSymbol] ;
	}

	play {|vol = -9|
		(transp+structure+root).do{|i|
			i.postln;
			Synth(\piano,  [\note, i, \vol, 0.5])
		}

	}

	/*-------------------------------------CODIGO TFG--------------------------------------------------*/

	/*-SINTETIZADORES PROPIOS-*/
	inicializarSynths{
	    (

        /******-----MELODIAS--------********/

       SynthDef(\melodia, {
       arg freq = 440, dur = 0.25, amp = 0.3, waveform = 0;

       var env = EnvGen.kr(Env.perc(0.01, 0.1), doneAction: 2);
       var sound;

       // Select waveform (0: Square, 1: Saw, 2: Triangle)
       sound = SinOsc.ar(freq, 0, 0.3) * Pulse.ar(2 * freq, 0.2) * env * amp;

       Out.ar(0, sound);
       }).add;

        /******-----ARPEGIOS--------********/
        SynthDef(\retro, {
        arg freq = 440, dur = 0.25, amp = 0.2, filterQ = 0.001, resonanceFreq = 440;

        var env = EnvGen.kr(Env([0, 1, 0], [0.01, 0.4]), doneAction: 2);

        // Basic waveform
        var waveform = LFSaw.ar(freq) * amp;

        // Bit-crushing effect to reduce bit depth
        var bitCrushed = (waveform * 16).round / 16;

        var sound = bitCrushed * env;

        Resonz.ar(sound, freq * resonanceFreq, filterQ);

        Out.ar(~bus, sound);
        }).add;

        /******-----REVERB--------********/
        SynthDef.new(\reverb, {
	    arg in=0, mix=0.08, out=0;
	    var sig, wet;
	    sig = In.ar(in, 2);
	    sig = FreeVerb2.ar(
		sig[0], sig[1],
		mix: mix.varlag(2),
		room: 0.999,
		damp: 0.85
	    );
	    Out.ar(out, sig);
        }).add;


		/******-----PERCUSION--------********/

		SynthDef(\bombo, {
			arg freqA=1000, freqB=100, freqC=30, freqDur1=0.03, freqDur2=1, freqC1=(-3),freqC2=1,
            atk=0.01, rel=0.1, c1=1, c2=(-1), amp=0.8, pan=0, out=0;
            var sig, env, freqSweep;
            freqSweep = Env([freqA, freqB, freqC], [freqDur1, freqDur2], [freqC1, freqC2]).ar;
            env = Env([0, 1, 0], [atk, rel], [c1, c2]).kr(2);
            sig = SinOsc.ar(freqSweep, pi/2);
			sig = sig * env;
			sig = Pan2.ar(sig, pan, amp);

            //sig = FreeVerb.ar(sig, 0.2, 0.8, 0.2);
            Out.ar(out, sig);
		}).add;


        SynthDef(\caja, {
            arg freq = 1, dur = 0.25, amp = 0.5;

            var env = EnvGen.kr(Env([0, 1, 0], [0.01, 0.1]), doneAction: 2);
            var sound;

            // Noise component for "hit" sound
            var hit = WhiteNoise.ar(amp * 0.5) * env;

            // Resonant filter for "body" sound
            var body = SinOsc.ar(freq, 0, LFNoise1.kr(0.1).range(0.8, 1.2)) * 0.3;
            body = RLPF.ar(body, freq * 2, 0.2) * env;

            // Combine hit and body sounds
            sound = hit + body;

            //sound = FreeVerb.ar(sound, 0.2, 0.8, 0.2);
            Out.ar(0, sound);
        }).add;

        //FINALMENTE TOM NO SERÁ UTILIZADO
        SynthDef(\tom, {
            arg freq = 50, dur = 0.5, amp = 0.5;

            // Envelope for the overall tom sound
            //var env = EnvGen.kr(Env([0, 1, 0], [0.01, 0.3]), doneAction: 2);
            var env = Env.perc(0.01, 0.3).kr( 1);

            // Sinusoidal oscillator with pitch decay
            var sound = SinOsc.ar(freq, 0, Line.kr(freq, freq * 0.6, dur)) * env;

            // Band-pass filter to emphasize tom character
            sound = BPF.ar(sound, freq * 1.5, 0.5) * amp;

            Out.ar(0, sound);
        }).add;
		/**-----PERCUSION---------**/
        )
        }

	inicializarArpegios{
		arg acordes;

		~notasArpegios = [];
		~duraciones = [];
        ~numAcordes = acordes.size;

		acordes.do{
			arg item;
            C(item).incluirAcorde();
		};

		~duraciones = Array.fill(~notasArpegios.size, 1);
	}

	incluirAcorde {|vol = -9|
		~notasAcorde = [];

		(transp+structure+root).do{
            arg item, i;

            ~aux = (transp+structure+root);
            if ( i > 1,                // Boolean expression (chooses one at random)
                { if (i == (~aux.size - 1), { ~notasAcorde = ~notasAcorde.add(item); }); },    // true function
                { ~notasAcorde = ~notasAcorde.add(item); }    // false function
            )
			//~notasAcorde = ~notasAcorde.add(i);
		};

		//Una vez inicializados los array de notas y duraciones, es necesario añadir las mismas notas en sentido contrario (menos la primera y a ultiama)
		~notasInversas = ~notasAcorde.reverse;
		~notasInversas.removeAt(0);
		~notasInversas.removeAt(~notasInversas.size - 1);
		~notasAcorde = ~notasAcorde ++ ~notasInversas;

		//Por ultimo añadimos las notas del acode arpegiado a la lista con todos los arpegios
		~notasArpegios = ~notasArpegios ++ ~notasAcorde;
	}

	tocarArpegio {
        arg filtroResonancia = 0.001;

        (
			~arpegio = Pbindef(\armonia,
			\type, \note,
			\instrument, \retro,
            \filterQ, filtroResonancia,
            \amp, 0.05,
			\midinote, Pseq(~notasArpegios, inf),
			\dur, Pseq(~duraciones.normalizeSum * 2 * ~numAcordes, inf),
			\out, 0
			);
		)
	}

    tocarPercusion {

        (
            ~patronBombo = Pbindef(\patronBombo,
             \instrument, \bombo,
             \amp, Pwhite(0.5, 0.75, inf),
             \dur, 1,
             //\dur, Prand([0.25, 0.5, 1], inf),
             \freqA, Pwhite(500, 600, inf),
             \freqB, Prand([30, 40, 50], inf),
             \freqC, Prand([10, 20, 30], inf),
             \atk, 0.01,
             \rel, Pwhite(0.1, 0.15, inf),
             \pan, 0,
             \out, 0
            );



            ~patronTom = Pbindef(\patronTom,
                \instrument, \tom,
                \freq, 50,
                \amp, 0.05,
                \dur, Pseq([Pseq([3/24],3), Pseq([1/16],12), 1], inf) * 4,
                \atk, Pwhite(0.001, 0.1, inf),
                \rel, Pwhite(0.01, 0.1, inf),
                \out, 0
            );




            ~patronCaja = Pbindef(\patronCaja,
            \instrument, \caja,
            \freq, 1,
            \amp, Pwhite(0.8, 1, inf),
            \dur, 2,
            \atk, Pwhite(0.001, 0.01, inf),
            \rel, Pwhite(0.15, 0.5, inf),
            \out, 0
            );
        )
	}

    tocarMelodia{

        (
            ~posiblesFrasesMelodicas = [
                [0, 2, 4, 5, 7, 9, 11], // Escala ascendente
                [0, -2, -4, -5, -7, -9, -11], // Escala descendente
                [0, 2, 4, 3, 7, 5, 9], // Tipo arpegio
                [0, 2, 4, 0, 2, 4], // Patron repetitivo
            ];
            ~melodia = Pbind(
            \instrument, \melodia,
            \degree, Prand({ Pdup(Prand([1, 2, 3], inf), Prand(~posiblesFrasesMelodicas.choose, inf)) } ! 8, inf),
            \dur, Prand([0.125, 0.25, 0.5], inf),
            \amp, Prand([1, 1.3, 1.5], inf),
            \legato, Prand([0.9, 0.8, 0.7], inf));

            //HAY QUE SOLUCIONAR QUE SE ESCUCHA BAJITO LAS MELODIAS ALEATORIOAS (MIRAR FIN DE PRUEBA8BIT)
            //POSIBLE USO DE Ppar
            //Finalmente intentar usar el filtro de resonancia (ver telegram profesor)
        )
	}

	/*-------------------------------------CODIGO TFG--------------------------------------------------*/
}

/*
C.audio;
C.decay = 0.25; C.release = 0.1
c = C(\C7add13).play ; c.structure.postln
c.root
C('Gbmaj9').play

{
"Eb7 Dm7 Eb7 Dm7 Eb7 Dm7 Em7b5 A7b5 Dm7".split($ ).collect{|i|
C(i.postln.asSymbol).play;
1.wait
}
}.fork
{
// C. Mingus, re-incarnation of a love bird
"Gm7 Ebmaj7 Am7b5 D7#9 Gm7 Ebmaj7 Am7b5 D7 Gm Ebmaj7 Cm7 F7 Am7b5 D7 Gm".split($ ).collect{|i|
C(i.postln.asSymbol).play;
1.wait
}
}.fork
*/
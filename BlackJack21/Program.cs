

class Program
{
    static void Main(string[] args)
    {
        Console.Write("Ingrese el nombre del jugador: ");
        string nombreJugador = Console.ReadLine();

        BlackJackGame juego = new BlackJackGame(nombreJugador);
        juego.Jugar();

        Console.WriteLine("Fin del juego.");
    }

}


public enum Palo
{
    Corazones,
    Diamantes,
    Rombos,
    Picas
}
public class Carta
{
    public Palo Palo { get; }
    public int Valor { get; }

    public Carta(Palo palo, int valor)
    {
        Palo = palo;
        Valor = valor;
    }
    public void MostarCarta()
    {
        Console.WriteLine($"{Valor} de {Palo}");
    }

}

public class Baraja
{
    private List<Carta> cartas;
    private Random rand;

    public Baraja()
    {
        cartas = new List<Carta>();
        rand = new Random();

        foreach (Palo palo in Enum.GetValues(typeof(Palo)))
        {
            for (int valor = 1; valor <= 13; valor++)
            {
                cartas.Add(new Carta(palo, valor));
            }
        }
    }
    public int ContarCartas()
    {
        return cartas.Count;
    }
    public void Barajar()
    {
        int n = ContarCartas();
        while (n > 1)
        {
            n--;
            int k = rand.Next(n + 1);
            Carta Carta = cartas[k];
            cartas[k] = cartas[n];
            cartas[n] = Carta;
            //Console.WriteLine($"{Carta.Palo} + {Carta.Valor}");
        }
    }
    public Carta RobarCarta(int contador)
    {
        Carta cartarobada = cartas[contador];
        return cartarobada;
    }
    public void EliminarCarta(int indice)
    {
        if (indice >= 0 && indice < cartas.Count)
        {
            cartas.RemoveAt(indice);
        }
    }
    public static void RepartirCarta(Jugador jugador, Baraja baraja)
    {
        int numCartas = baraja.ContarCartas();
        if (numCartas > 0)
        {
            int indiceCarta = numCartas - 1; // Índice de la última carta
            Carta carta = baraja.RobarCarta(indiceCarta);
            jugador.CogerCarta(carta);
            baraja.EliminarCarta(indiceCarta); // Elimina la carta de la baraja
        }
        else
        {
            Console.WriteLine("No quedan cartas en la baraja.");
        }
    }

}
public class Jugador
{
    protected List<Carta> mano;

    public string nombre { get; }

    public Jugador(string nom)
    {
        nombre = nom;
        mano = new List<Carta>();
    }
    public void CogerCarta(Carta robada)
    {
        mano.Add(robada);
    }
    public void mostrarCartas()
    {
        foreach (Carta cartas in mano)
        {
            Console.WriteLine(cartas.Valor + " " + cartas.Palo);
        }
    }

}
public class JugadorBlackjack : Jugador
{
    public JugadorBlackjack(string nombre) : base(nombre)
    {
    }

    public bool TieneAs()
    {
        List<Carta> manoJugador = mano;
        foreach (Carta carta in manoJugador)
        {
            if (carta.Valor == 1) // Valor del As en el blackjack
            {
                return true;
            }
        }
        return false;
    }

    public int CalcularPuntuacion()
    {
        int puntuacion = 0;
        int ases = 0;

        foreach (Carta carta in mano)
        {
            if (carta.Valor >= 10)
            {
                puntuacion += 10; // Rey, Reina y Jota
            }
            else if (carta.Valor == 1) // Valor del As
            {
                ases++;
                puntuacion += 1;
            }
            else
            {
                puntuacion += carta.Valor;
            }
        }

        while (puntuacion < 12 && ases > 0)
        {
            puntuacion += 10; // Si la puntuación no supera 12 y hay al menos un As, cambia el valor del As a 11.
            ases--;
        }

        return puntuacion;
    }

    public bool HaPerdido()
    {
        return CalcularPuntuacion() > 21;
    }
}
public class Crupier : JugadorBlackjack
{
    public Crupier(string nombre) : base(nombre)
    {
    }

    public void Jugar(Baraja baraja)
    {
        while (CalcularPuntuacion() < 17)
        {
            Baraja.RepartirCarta(this, baraja);
        }
    }
}
public class BlackJackGame
{
    private JugadorBlackjack jugador1;
    private Crupier crupier;
    private Baraja baraja;

    public BlackJackGame(string nombreJugador)
    {
        jugador1 = new JugadorBlackjack(nombreJugador);
        crupier = new Crupier("Crupier");
        baraja = new Baraja();
        baraja.Barajar();
    }

    public int DiferenciaHasta21(JugadorBlackjack jugador)
    {
        int puntosJugador = jugador.CalcularPuntuacion();
        return Math.Max(0, 21 - puntosJugador);
    }

    public int PuntosJugador(JugadorBlackjack jugador)
    {
        return jugador.CalcularPuntuacion();
    }

    public int PuntosCrupier()
    {
        return crupier.CalcularPuntuacion();
    }

    public string DeterminarResultado()
    {
        int puntosJugador = PuntosJugador(jugador1);
        int puntosCrupier = PuntosCrupier();

        if (puntosJugador > 21)
        {
            return $"El jugador {jugador1.nombre} ha perdido.";
        }
        else if (puntosCrupier > 21)
        {
            return $"El crupier ha perdido. ¡{jugador1.nombre} ha ganado!";
        }
        else if (puntosJugador > puntosCrupier)
        {
            return $"{jugador1.nombre} ha ganado.";
        }
        else if (puntosCrupier > puntosJugador)
        {
            return "El crupier ha ganado.";
        }
        else
        {
            return "Es un empate.";
        }
    }
    public void Jugar()
    {
        Baraja.RepartirCarta(jugador1, baraja);
        Baraja.RepartirCarta(crupier, baraja);
        Baraja.RepartirCarta(jugador1, baraja);
        Baraja.RepartirCarta(crupier, baraja);

        while (true)
        {
            Console.WriteLine("Cartas del crupier:");
            crupier.mostrarCartas();
            Console.WriteLine($"Puntuación del crupier: {PuntosCrupier()}");
            Console.WriteLine($"El crupier necesita {DiferenciaHasta21(crupier)} puntos para ganar");
            Console.WriteLine($"Cartas de {jugador1.nombre}:");
            jugador1.mostrarCartas();
            Console.WriteLine($"Puntuación de {jugador1.nombre}: {PuntosJugador(jugador1)}");
            Console.WriteLine($"{jugador1.nombre} necesita {DiferenciaHasta21(jugador1)} puntos para ganar");

            if (PuntosJugador(jugador1) > 21)
            {
                Console.WriteLine($"El jugador {jugador1.nombre} ha perdido.");
                break;
            }

            Console.Write("¿Desea robar una carta? (S/N): ");
            string respuesta = Console.ReadLine().Trim();

            if (respuesta.Equals("S", StringComparison.OrdinalIgnoreCase))
            {
                Baraja.RepartirCarta(jugador1, baraja);
            }
            else if (respuesta.Equals("N", StringComparison.OrdinalIgnoreCase))
            {
                if (PuntosCrupier() < 17 && PuntosCrupier() < PuntosJugador(jugador1)) // Crupier roba una última carta si el jugador se planta con más puntos.
                {
                    Baraja.RepartirCarta(crupier, baraja);
                }
                break;
            }

            if (PuntosCrupier() >= 17)
            {
                break; // El crupier ya tiene al menos 17 puntos, termina el juego.
            }

            // Turno del crupier
            Baraja.RepartirCarta(crupier, baraja);
        }

        Console.WriteLine("Cartas del crupier:");
        crupier.mostrarCartas();
        Console.WriteLine($"Puntuación del crupier: {PuntosCrupier()}");
        Console.WriteLine($"Cartas de {jugador1.nombre}:");
        jugador1.mostrarCartas();
        Console.WriteLine($"Puntuación de {jugador1.nombre}: {PuntosJugador(jugador1)}");
        Console.WriteLine(DeterminarResultado());
    }
}



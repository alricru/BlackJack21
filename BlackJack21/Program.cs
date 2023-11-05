
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        Console.WriteLine("+------------------------------------+");
        Console.WriteLine("| +-----+ ------------------         |");
        Console.WriteLine("| |A    | JUEGO: Blackjack 21        |");
        Console.WriteLine("| |  ♠  | LENGUAJE: C#               |");
        Console.WriteLine("| |    A| AUTOR: Alejandro Rivero    |");
        Console.WriteLine("| +-----+ ------------------         |");
        Console.WriteLine("|         PROGRAMACIÓN Y MOTORES     |");
        Console.WriteLine("+------------------------------------+");

        Console.Write("ESCRIBA SU NOMBRE: ");
        string nombreJugador = Console.ReadLine();
        Console.Clear();

        BlackJackGame juego = new BlackJackGame(nombreJugador);
        juego.Jugar();

        Console.WriteLine("FIN DEL JUEGO.");
    }

}


public enum Palo
{
    Corazones,
    Treboles,
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
    public void DibujarCartas()
    {
        foreach (Carta carta in mano)
        {
            Console.Write("+-----+");
        }
        Console.WriteLine(); 

        foreach (Carta carta in mano)
        {
            Console.Write($"|{CartaStr(carta),-4} |");
        }
        Console.WriteLine(); 

        foreach (Carta carta in mano)
        {
            Console.Write("|  ");
            Console.Write($"{SimboloStr(carta.Palo)}");
            Console.Write("  |");
        }
        Console.WriteLine(); 

        foreach (Carta carta in mano)
        {
            Console.Write($"|   {CartaStr(carta),-2}|");
        }
        Console.WriteLine(); 

        foreach (Carta carta in mano)
        {
            Console.Write("+-----+");
        }
        Console.WriteLine();
    }
    public string SimboloStr(Palo palo)
    {
        switch (palo)
        {
            case Palo.Corazones:
                return "♥";
            case Palo.Rombos:
                return "♦";
            case Palo.Treboles:
                return "♣";
            case Palo.Picas:
                return "♠";
            default:
                return "";
        }
    }

    public string CartaStr(Carta carta)
    {
        if (carta.Valor >= 2 && carta.Valor <= 10)
        {
            return carta.Valor.ToString();
        }
        else
        {
            switch (carta.Valor)
            {
                case 1:
                    return "A";
                case 11:
                    return "J";
                case 12:
                    return "Q";
                case 13:
                    return "K";
                default:
                    return "";
            }
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
            return $"{jugador1.nombre} HA PERDIDO.";
        }
        else if (puntosCrupier > 21)
        {
            return $"EL CRUPIER PIERDE. ¡{jugador1.nombre} HA GANADO!";
        }
        else if (puntosJugador == 21)
        {
            return $"¡{jugador1.nombre} HA LOGRADO UN BLACKJACK!";
        }
        else if (puntosJugador > puntosCrupier)
        {
            return $"¡{jugador1.nombre} HA GANADO!";
        }
        else if (puntosCrupier > puntosJugador)
        {
            return "¡GANA EL CRUPIER!";
        }
        else
        {
            return "ES UN EMPATE.";
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
            Console.WriteLine("-------------------------------------");
            Console.WriteLine("MANO DEL CRUPIER:");
            Console.WriteLine("-------------------------------------");
            crupier.DibujarCartas();
            Console.WriteLine("-------------------------------------");
            Console.WriteLine($"PUNTUACIÓN DEL CRUPIER: {PuntosCrupier()}");
            Console.WriteLine("-------------------------------------");
            Console.WriteLine($"EL CRUPIER NECESITA {DiferenciaHasta21(crupier)} PUNTOS PARA GANAR");
            Console.WriteLine("-------------------------------------");
            Console.WriteLine($"MANO DE {jugador1.nombre}:");
            Console.WriteLine("-------------------------------------");
            jugador1.DibujarCartas();
            Console.WriteLine("-------------------------------------");
            Console.WriteLine($"PUNTUACIÓN DE {jugador1.nombre}: {PuntosJugador(jugador1)}");
            Console.WriteLine("-------------------------------------");
            Console.WriteLine($"{jugador1.nombre} NECESITA {DiferenciaHasta21(jugador1)} PUNTOS PARA GANAR");
            Console.WriteLine("-------------------------------------");


            Console.Write("¿QUIERES ROBAR UNA CARTA? (S/N): ");
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

            if (PuntosCrupier() >= 17 || PuntosJugador(jugador1) >= 21)
            {
                break; // El crupier ya tiene al menos 17 puntos o el jugador 21 o más, termina el juego.
            }

            // Turno del crupier
            Baraja.RepartirCarta(crupier, baraja);
        }
        Console.WriteLine("-------------------------------------");
        Console.WriteLine("MANO DEL CRUPIER:");
        Console.WriteLine("-------------------------------------");
        crupier.DibujarCartas();
        Console.WriteLine("-------------------------------------");
        Console.WriteLine($"PUNTUACIÓN DEL CRUPIER: {PuntosCrupier()}");
        Console.WriteLine("-------------------------------------");
        Console.WriteLine($"MANO DE {jugador1.nombre}:");
        Console.WriteLine("-------------------------------------");
        jugador1.DibujarCartas();
        Console.WriteLine("-------------------------------------");
        Console.WriteLine($"PUNTUACIÓN DE {jugador1.nombre}: {PuntosJugador(jugador1)}");
        Console.WriteLine("-------------------------------------");
        Console.WriteLine(DeterminarResultado());
        Console.WriteLine("-------------------------------------");
    }
}
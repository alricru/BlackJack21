
class Program
{
    static void Main(string[] args)
    {
        Baraja baraja = new Baraja();
        baraja.Barajar();
        baraja.RobarCarta(1);
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
                //Console.WriteLine($"Carta: {valor} de {palo}.");
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
            Console.WriteLine($"{Carta.Valor} de {Carta.Palo}");
        }
    }
    public Carta RobarCarta(int contador)
    {
        Carta cartarobada = cartas[contador];
        return cartarobada;
    }
}







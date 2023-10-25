
using System.ComponentModel;

class Program
{

    static void Main(string[] args)
    {

    }
}

public class Cartas{
    public enum PaloCartas
    {
        picas = 1,
        corazones = 2,
        trebol = 3,
        rombos = 4
    }
    void CrearCarta()
    {
        Random rand = new Random();
        int numero = rand.Next(1, 10);
        var palo = Enum.GetValues(typeof(PaloCartas)).Cast<PaloCartas>().ToList();
        PaloCartas randPaloCartas = palo[rand.Next(palo.Count)];
    }
}


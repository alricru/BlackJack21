
class Program
{
    static void Main(string[] args)
    {
        Deck deck = new Deck();
        deck.Shuffle();
    }
}
public enum Suit //Enumerador para los palos de las cartas
{
    Corazones,
    Diamantes,
    Rombos,
    Picas
}
public class Card //Clase para representar una carta con su palo y valor
    {
    public Suit Suit { get; }
        public int Value { get; }

        public Card(Suit suit, int value) //Constructor de clase
        {
            Suit = suit;
            Value = value;
        }
    }

    public class Deck
    {
        private List<Card> cards;
        private Random random;

        public Deck()
        {
            cards = new List<Card>();
            random = new Random();

            foreach (Suit suit in Enum.GetValues(typeof(Suit))) //Se crea la baraja en la lista "cards"
            {
                for (int value = 1; value <= 13; value++)
                {
                    cards.Add(new Card(suit, value));
                    Console.WriteLine($"Carta: {value} de {suit}.");
                }
            }
        }

        public void Shuffle() //Método que mezcla la baraja
        {
            int n = cards.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                Card card = cards[k];
                cards[k] = cards[n];
                cards[n] = card;
                Console.WriteLine($"{card.Value} de {card.Suit}");
            }
        }
    }







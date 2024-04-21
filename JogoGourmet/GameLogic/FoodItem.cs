namespace JogoGourmet.GameLogic
{
    public class FoodItem
    {
        public string Name { get; private set; }
        public List<string> Characteristics { get; private set; } = [];

        public FoodItem(string name, List<string> characteristics)
        {
            Name = name;
            Characteristics = characteristics;
        }
    }
}

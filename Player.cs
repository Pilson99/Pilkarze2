namespace Pilkarze
{
    public class Player
    {
        private string _name, _lastName;
        private int _age;
        private double _weight;

        public Player(string name, string lastName, int age, double weight)
        {
            _name = name;
            _lastName = lastName;
            _age = age;
            _weight = weight;
        }

        public void Update(string name, string lastName, int age, double weight)
        {
            _name = name;
            _lastName = lastName;
            _age = age;
            _weight = weight;
        }

        public string Name { get { return _name; } }
        public string LastName { get { return _lastName; } }
        public int Age { get { return _age; } }
        public double Weight { get { return _weight; } }
        public string FullInfo { get { return _name + " " + _lastName + " - Wiek: " + _age + ", Waga: " + _weight; } }
    }
}
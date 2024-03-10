public class Dog
{
    private int _id;
    private string _breed;
    private double _age;
    private int _weight;
    private bool _vaccinated;

    public int Id
    {
        get { return _id; }
        set { _id = value; }
    }
    public string Breed
    {
        get { return _breed; }
        set { _breed = value; }
    }
    public double Age
    {
        get { return _age; }
        set { _age = value; }
    }
    public int Weight
    {
        get { return _weight; }
        set { _weight = value; }
    }
    public bool Vaccinated
    {
        get { return _vaccinated; }
        set { _vaccinated = value; }
    }
}
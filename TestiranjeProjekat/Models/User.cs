using System.Collections.Generic;

public class User
{
    private int _id;
    private string _name;
    private string _surname;
    private List<Dog> _dogs;
    public int Id
    {
        get { return _id; }
        set { _id = value; }
    }
    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }
    public string Surname
    {
        get { return _surname; }
        set { _surname = value; }
    }
}
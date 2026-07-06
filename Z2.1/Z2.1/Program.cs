using System;

public class Rectangle
{
    private double _width;
    private double _height;

    public int X { get; set; }
    public int Y { get; set; }

    public double Width
    {
        get => _width;
        set
        {
            if (value < 0)
                throw new ArgumentException("Ширина не может быть отрицательной.");

            _width = value;
        }
    }

    public double Height
    {
        get => _height;
        set
        {
            if (value < 0)
                throw new ArgumentException("Высота не может быть отрицательной.");

            _height = value;
        }
    }

    public double Area => Width * Height;

    public double Perimeter => 2 * (Width + Height);

    public Rectangle(int x, int y, double width, double height)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }
}

class Program
{
    static void Main()
    {
        Console.Write("Введите координату X: ");
        int x = Convert.ToInt32(Console.ReadLine());

        Console.Write("Введите координату Y: ");
        int y = Convert.ToInt32(Console.ReadLine());

        Console.Write("Введите ширину: ");
        double width = Convert.ToDouble(Console.ReadLine());

        Console.Write("Введите высоту: ");
        double height = Convert.ToDouble(Console.ReadLine());

        try
        {
            Rectangle rectangle = new Rectangle(
                x,
                y,
                width,
                height);

            Console.WriteLine($"\nЛевый верхний угол: ({rectangle.X}, {rectangle.Y})");
            Console.WriteLine($"Ширина: {rectangle.Width}");
            Console.WriteLine($"Высота: {rectangle.Height}");
            Console.WriteLine($"Площадь: {rectangle.Area}");
            Console.WriteLine($"Периметр: {rectangle.Perimeter}");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
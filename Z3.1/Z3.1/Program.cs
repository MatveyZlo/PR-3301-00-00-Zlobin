using System;
using System.Collections;
using System.Collections.Generic;

public class SmartStack<T> : IEnumerable<T>
{
    private T[] items;
    private int count;

    public SmartStack()
    {
        items = new T[4];
    }

    public SmartStack(int capacity)
    {
        if (capacity <= 0)
            throw new ArgumentException("Некорректная ёмкость.");

        items = new T[capacity];
    }

    public SmartStack(IEnumerable<T> collection)
    {
        if (collection == null)
            throw new ArgumentNullException(nameof(collection));

        int size = 0;

        foreach (T item in collection)
        {
            size++;
        }

        items = new T[size];

        foreach (T item in collection)
        {
            items[count++] = item;
        }
    }

    public int Count
    {
        get { return count; }
    }

    public int Capacity
    {
        get { return items.Length; }
    }

    public void Push(T item)
    {
        if (count == items.Length)
        {
            Resize(items.Length * 2);
        }

        items[count++] = item;
    }

    public void PushRange(IEnumerable<T> collection)
    {
        if (collection == null)
            throw new ArgumentNullException(nameof(collection));

        T[] temp = new T[4];
        int tempCount = 0;

        foreach (T item in collection)
        {
            if (tempCount == temp.Length)
            {
                Array.Resize(ref temp, temp.Length * 2);
            }

            temp[tempCount++] = item;
        }

        for (int i = tempCount - 1; i >= 0; i--)
        {
            Push(temp[i]);
        }
    }

    public T Pop()
    {
        if (count == 0)
            throw new InvalidOperationException("Стек пуст.");

        count--;

        T item = items[count];
        items[count] = default(T);

        return item;
    }

    public T Peek()
    {
        if (count == 0)
            throw new InvalidOperationException("Стек пуст.");

        return items[count - 1];
    }

    public bool Contains(T item)
    {
        EqualityComparer<T> comparer =
            EqualityComparer<T>.Default;

        for (int i = 0; i < count; i++)
        {
            if (comparer.Equals(items[i], item))
            {
                return true;
            }
        }

        return false;
    }

    private void Resize(int newCapacity)
    {
        T[] newArray = new T[newCapacity];

        for (int i = 0; i < count; i++)
        {
            newArray[i] = items[i];
        }

        items = newArray;
    }

    public T this[int index]
    {
        get
        {
            if (index < 0 || index >= count)
                throw new ArgumentOutOfRangeException();

            return items[count - 1 - index];
        }
    }

    public IEnumerator<T> GetEnumerator()
    {
        for (int i = count - 1; i >= 0; i--)
        {
            yield return items[i];
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

class Program
{
    static void Main()
    {
        SmartStack<int> stack = new SmartStack<int>();

        while (true)
        {
            Console.WriteLine("1. Push (Добавить)");
            Console.WriteLine("2. PushRange");
            Console.WriteLine("3. Pop (Удалить)");
            Console.WriteLine("4. Peek (Посмотреть вершину)");
            Console.WriteLine("5. Contains (Поиск)");
            Console.WriteLine("6. Count");
            Console.WriteLine("7. Capacity");
            Console.WriteLine("8. View All");
            Console.WriteLine("9. Проверить индексатор");
            Console.WriteLine("0. Выход");

            Console.Write("\nВыберите действие: ");
            int choice = Convert.ToInt32(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    Console.Write("Введите число: ");
                    int value = Convert.ToInt32(Console.ReadLine());
                    stack.Push(value);
                    Console.WriteLine("Элемент добавлен.");
                    break;

                case 2:
                    Console.Write("Сколько элементов добавить: ");
                    int n = Convert.ToInt32(Console.ReadLine());

                    int[] values = new int[n];

                    for (int i = 0; i < n; i++)
                    {
                        Console.Write($"Элемент {i + 1}: ");
                        values[i] = Convert.ToInt32(Console.ReadLine());
                    }

                    stack.PushRange(values);
                    Console.WriteLine("Коллекция добавлена.");
                    break;

                case 3:
                    try
                    {
                        Console.WriteLine($"Удалён элемент: {stack.Pop()}");
                    }
                    catch (InvalidOperationException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    break;

                case 4:
                    try
                    {
                        Console.WriteLine($"Вершина стека: {stack.Peek()}");
                    }
                    catch (InvalidOperationException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    break;

                case 5:
                    Console.Write("Введите число для поиска: ");
                    int search = Convert.ToInt32(Console.ReadLine());

                    Console.WriteLine(
                        stack.Contains(search)
                        ? "Элемент найден."
                        : "Элемент не найден.");
                    break;

                case 6:
                    Console.WriteLine($"Count = {stack.Count}");
                    break;

                case 7:
                    Console.WriteLine($"Capacity = {stack.Capacity}");
                    break;

                case 8:
                    Console.WriteLine("\nСодержимое стека:");

                    if (stack.Count == 0)
                    {
                        Console.WriteLine("Стек пуст.");
                    }
                    else
                    {
                        foreach (int item in stack)
                        {
                            Console.WriteLine(item);
                        }
                    }

                    break;

                case 9:
                    try
                    {
                        Console.Write("Введите глубину: ");
                        int index = Convert.ToInt32(Console.ReadLine());

                        Console.WriteLine(
                            $"Элемент: {stack[index]}");
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        Console.WriteLine(
                            "Индекс вне диапазона.");
                    }

                    break;

                case 0:
                    return;

                default:
                    Console.WriteLine("Неверный пункт меню.");
                    break;
            }
        }
    }
}
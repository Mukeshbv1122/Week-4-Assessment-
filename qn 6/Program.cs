using System;
class Node
{
    public int Data;
    public Node Next;

    public Node(int data)
    {
        Data = data;
        Next = null;
    }
}
class LinkedListQueue
{
    private Node front;
    private Node rear;

    public LinkedListQueue()
    {
        front = null;
        rear = null;
    }

    // Insert at rear
    public void Insert(int data)
    {
        Node newNode = new Node(data);

        if (rear == null)
        {
            front = rear = newNode;
        }
        else
        {
            rear.Next = newNode;
            rear = newNode;
        }
        Console.WriteLine($"{data} inserted into queue.");
    }

    // Delete from front
    public void Delete()
    {
        if (front == null)
        {
            Console.WriteLine("Queue is empty. Cannot delete.");
            return;
        }

        Console.WriteLine($"{front.Data} deleted from queue.");
        front = front.Next;

        if (front == null) // Queue became empty
            rear = null;
    }

    // Display queue
    public void Display()
    {
        if (front == null)
        {
            Console.WriteLine("Queue is empty.");
            return;
        }

        Console.Write("Queue: ");
        Node temp = front;
        while (temp != null)
        {
            Console.Write(temp.Data + " ");
            temp = temp.Next;
        }
        Console.WriteLine();
    }

    // Show front element
    public void ShowFront()
    {
        if (front == null)
            Console.WriteLine("Queue is empty.");
        else
            Console.WriteLine("Front: " + front.Data);
    }

    // Show rear element
    public void ShowRear()
    {
        if (rear == null)
            Console.WriteLine("Queue is empty.");
        else
            Console.WriteLine("Rear: " + rear.Data);
    }
}

class Program
{
    static void Main()
    {
        LinkedListQueue q = new LinkedListQueue();

        q.Insert(10);
        q.Insert(20);
        q.Insert(30);

        q.Display();
        q.ShowFront();
        q.ShowRear();

        q.Delete();
        q.Display();

        q.Delete();
        q.Delete();
        q.Delete();
    }
}
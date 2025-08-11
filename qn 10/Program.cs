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
class Stack
{
    private Node top;

    public Stack()
    {
        top = null;
    }

    // Push operation
    public void Push(int data)
    {
        Node newNode = new Node(data);
        newNode.Next = top;
        top = newNode;
        Console.WriteLine($"{data} pushed to stack");
    }

    // Pop operation
    public void Pop()
    {
        if (top == null)
        {
            Console.WriteLine("Stack is empty. Cannot pop.");
            return;
        }
        Console.WriteLine($"{top.Data} popped from stack");
        top = top.Next;
    }

    // Peek (top element)
    public void Peek()
    {
        if (top == null)
        {
            Console.WriteLine("Stack is empty.");
            return;
        }
        Console.WriteLine($"Top element is {top.Data}");
    }

    // Display stack
    public void Display()
    {
        if (top == null)
        {
            Console.WriteLine("Stack is empty.");
            return;
        }
        Console.Write("Stack elements: ");
        Node temp = top;
        while (temp != null)
        {
            Console.Write(temp.Data + " ");
            temp = temp.Next;
        }
        Console.WriteLine();
    }
}
class Program
{
    static void Main()
    {
        Stack stack = new Stack();

        stack.Push(10);
        stack.Push(20);
        stack.Push(30);
        stack.Display();
        stack.Peek();
        stack.Pop();
        stack.Peek();
        stack.Display();
    }
}
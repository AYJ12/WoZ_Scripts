using System;

public class UI_Stack<T>
{
    private T[] popUp_UI;
    public int index;

    public int Size;

    public UI_Stack(int size)
    {
        Size = size;
        popUp_UI = new T[size];
        index = -1;
    }

    public UI_Stack()
    {
        popUp_UI = new T[10];
        Size = 10;
        index = -1;
    }

    public void Push(T data)
    {
        if (Size - 1 == index)
        {
            throw new Exception("Stack overflow");
        }

        popUp_UI[++index] = data;
    }

    // ����
    public T Pop()
    {
        if (IsEmpty())
        {
            throw new Exception("Stack is Empty");
            return default(T);
        }

        return popUp_UI[index--];
    }

    // üũ
    public T Peek()
    {
        if (IsEmpty())
        {
            throw new Exception("Stack is Empty");
            return default(T);
        }

        return popUp_UI[index];
    }

    // ����ִ��� üũ
    public bool IsEmpty()
    {
        return index == -1;
    }
}
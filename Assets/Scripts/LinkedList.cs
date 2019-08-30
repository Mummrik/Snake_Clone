﻿using System;
using UnityEngine;

public class LinkedList<Object> /*: Iterator<Object>*/
{

    // Usefull info https://www.cs.cmu.edu/~adamchik/15-121/lectures/Linked%20Lists/linked%20lists.html
    private Node<Object> head;

    public LinkedList()
    {
        // When creating a new instance of a Linked list it should be empty aka null
        head = null;
    }

    public void addFirst(Object item)
    {
        // when adding a new element to the list it should create a new node whit the previous nodes data therefor linking it
        head = new Node<Object>(item, head);
    }

    public Object getFirst()
    {
        // method to get the data of the first element of the list
        if (head == null) { throw new NullReferenceException(); }
        return head.data;
    }

    public Object removeFirst()
    {
        // Remove the first element, and then set the head to the next element in the list
        Object temp = getFirst();
        head = head.next;
        return temp;
    }

    public void addLast(Object item)
    {
        // if the list is empty, add the item as the first element
        if (head == null)
        {
            addFirst(item);
        }
        else
        {
            Node<Object> temp = head;   // create a temporary node that contains the heads data
            while (temp.next != null)   // iterate through the list untill you reach the last element that is null
            {
                temp = temp.next;
            }
            // when reaching the last element add the item to the list and set the last element to null
            temp.next = new Node<Object>(item, null);
        }
    }

    public Object getLast()
    {
        if (head == null) { throw new NullReferenceException(); }

        Node<Object> temp = head;
        while (temp.next != null)
        {
            temp = temp.next;
        }

        return temp.data;
    }

    public int count()
    {
        if (head == null) { return 0; }

        int count = 1;
        Node<Object> temp = head;
        while (temp.next != null)
        {
            temp = temp.next;
            count++;
        }

        return count;
    }

    public void clear()
    {
        head = null;
    }

    // Check and see if the list contains an Object
    public bool contains(Object item)
    {
        Node<Object> temp = head;
        do
        {
            if (temp.data.Equals(item))
            {
                return true;
            }
            temp = temp.next;
        } while (!temp.data.Equals(item));

        return false;
    }

    public Object get(int index)
    {
        if (head == null)
        {
            throw new IndexOutOfRangeException();
        }
        Node<Object> temp = head;
        for (int i = 0; i < index; i++)
        {
            temp = temp.next;
        }
        if (temp == null)
        {
            throw new IndexOutOfRangeException();
        }

        return temp.data;
    }

    public void insertAfter(Object key, Object item)
    {
        Node<Object> temp = head;
        // Iterate untill key is found in the list
        while (temp != null && !temp.data.Equals(key))
        {
            temp = temp.next;
        }
        // if key is found, we set the next element to the new item, and grab a reference to the next element after the new element
        if (temp != null)
        {
            temp.next = new Node<Object>(item, temp.next);
        }
    }

    public void insertBefore(Object key, Object item)
    {
        if (head == null) { return; }

        if (head.data.Equals(key))
        {
            addFirst(item);
            return;
        }
        Node<Object> previous = null;
        Node<Object> current = head;

        while (current != null && !current.data.Equals(key))
        {
            previous = current;
            current = current.next;
        }
        if (current != null)
        {
            previous.next = new Node<Object>(item, current);
        }
    }

    public void remove(Object key)
    {
        if (head == null) { throw new NullReferenceException(); }
        if (head.data.Equals(key))
        {
            head = head.next;
            return;
        }
        Node<Object> previous = null;
        Node<Object> current = head;

        while (current != null && !current.data.Equals(key))
        {
            previous = current;
            current = current.next;
        }

        if (current == null) { throw new NullReferenceException(); }

        // Delete the current node from the list
        previous.next = current.next;
    }

}
public class Node<Object>
{
    // Stores the data of the element in the list, and a reference to the next element in the list that contains its data
    public Object data;
    public Node<Object> next;

    public Node(Object data, Node<Object> next)
    {
        this.data = data;
        this.next = next;
    }
}

/*
public interface Iterator<Object>
{
    public Iterator<Object> Iterator()
    {
        return new LinkedListIterator();
    }
}


public class LinkedListIterator : Iterator<object>
{

}*/


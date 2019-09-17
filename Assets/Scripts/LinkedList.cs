using System;

public class LinkedList<T>
{

    // Usefull info https://www.cs.cmu.edu/~adamchik/15-121/lectures/Linked%20Lists/linked%20lists.html
    private Node<T> head;

    class Node<T>
    {
        // Stores the data of the element in the list, and a reference to the next element in the list that contains its data
        public T data;
        public Node<T> next;

        public Node(T data, Node<T> next)
        {
            this.data = data;
            this.next = next;
        }
    }

    public LinkedList()
    {
        // When creating a new instance of a Linked list it should be empty aka null. even though it will always be null, but just to make it clear
        head = null;
    }

    public void AddFirst(T item)
    {
        // when adding a new element to the list it should create a new node whit the previous nodes data as the next element therefore linking it
        head = new Node<T>(item, head);
    }

    public T GetFirst()
    {
        // method to get the data of the first element of the list
        if (head == null) { throw new NullReferenceException(); }
        return head.data;
    }

    public void RemoveFirst()
    {
        // Remove the first element, and then set the head to the next element in the list
        head = head.next;
    }

    public void AddLast(T item)
    {
        // if the list is empty, add the item as the first element
        if (head == null)
        {
            AddFirst(item);
        }
        else
        {
            Node<T> temp = head;   // create a temporary node that contains the heads data
            while (temp.next != null)   // iterate through the list untill you reach the last element that is null
            {
                temp = temp.next;
            }
            // when reaching the last element add the item to the list and set the last element to null
            temp.next = new Node<T>(item, null);
        }
    }

    public T GetLast()
    {
        if (head == null) { throw new NullReferenceException(); }

        Node<T> temp = head;
        while (temp.next != null)   // iterate through the list untill you reach the last element that is null
        {
            temp = temp.next;
        }
        // then return the data of the last node
        return temp.data;
    }

    public int Count()
    {
        // if the head is null just return 0 
        if (head == null) { return 0; }

        int count = 1;  // start at 1 since the list atleast contains 1 element
        Node<T> temp = head;
        while (temp.next != null)   // iterate through the list and increas the count by 1 untill you reach the last element that is null
        {
            temp = temp.next;
            count++;
        }
        // return the count
        return count;
    }

    public void Clear()
    {
        // clear the list, by setting the first element to null all the other elements will lose its reference
        head = null;
    }

    // Check and see if the list contains an Object
    public bool Contains(T item)
    {
        // start whit the head
        Node<T> temp = head;
        do
        {
            if (temp.data.Equals(item)) // if the temp node's data is the same as the item you try to compare then return true
            {
                return true;
            }
            temp = temp.next;
        } while (!temp.data.Equals(item));  // do this while the temp nodes data dont match the item you want to compare

        return false;   // if nothing was found just return false
    }

    public T Get(int index)
    {
        if (head == null || index > Count()) { throw new IndexOutOfRangeException(); }

        Node<T> temp = head;
        for (int i = 0; i < index; i++) // iterate through the list untill the index is reached
        {
            temp = temp.next;
        }
        // return the data of the node
        return temp.data;
    }

    public void InsertAfter(T key, T item)
    {
        Node<T> temp = head;
        // Iterate untill key is found in the list
        while (temp != null && !temp.data.Equals(key))
        {
            temp = temp.next;
        }
        // if key is found, we set the next element to the new item, and grab a reference to the next element after the new element
        if (temp != null)
        {
            temp.next = new Node<T>(item, temp.next);
        }
    }

    public void InsertBefore(T key, T item)
    {
        // if there is nothing in the list then add the item as the first element
        if (head == null)
        {
            AddFirst(item);
            return;
        }   

        if (head.data.Equals(key))
        {
            AddFirst(item);
            return;
        }
        Node<T> previous = null;
        Node<T> current = head;

        while (current != null && !current.data.Equals(key))
        {
            previous = current;
            current = current.next;
        }
        if (current != null)
        {
            previous.next = new Node<T>(item, current);
        }
    }

    public void Remove(T key)
    {
        if (head == null) { throw new NullReferenceException(); }
        if (head.data.Equals(key))
        {
            head = head.next;
            return;
        }
        Node<T> previous = null;
        Node<T> current = head;

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

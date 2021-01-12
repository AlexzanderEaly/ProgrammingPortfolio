#ifndef LINKEDLIST_H
#define LINKEDLIST_H

class LinkedList
{
private:
	struct Node
	{
		int value;
		Node* next;

		Node(int val)
		{
			value = val;
			next = nullptr;
		}
	};

	Node* head;

public:
	//constuctor
	LinkedList();

	//copy consturot
	LinkedList(LinkedList &);

	//adds a node to the end
	void append(int);

	//adds a node at the postion or at the end if the postion is out of range
	void insert(int, int);
	
	//deletes a node with x value
	void deleteNode(int);

	void deleteNodeAt(int);

	//find postion of a node with x value
	int findVal(int);

	//displays the list
	void showlist();

	//reverse the order of the list
	static void reverse(LinkedList &);
	
	//deletes the whole list
	~LinkedList();
	

};


#endif
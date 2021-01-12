/*The guts of the linked list class
Made by Alexzander Ealy*/
#include"linkedInt.h"
#include<iostream>
using namespace std;

LinkedList::LinkedList()
{
	head = nullptr;
}

LinkedList::LinkedList(LinkedList & list)
{
	Node* nodePtr = list.head;

	while(nodePtr)
	{
		append(nodePtr->value);
		nodePtr = nodePtr->next;
	}
}

void LinkedList::append(int val)
{
	Node* newNode = new Node(val);
	Node* nodePtr;
	if(!head)
	{
		head = newNode;
	}
	else
	{
		nodePtr = head;
		while(nodePtr->next != nullptr)
		{
			nodePtr = nodePtr->next;
		}
		nodePtr->next = newNode;
	}


}

void LinkedList::insert(int val, int pos)
{
	int count = 0;
	Node* nodePtr = head;
	Node* prevsNode = nullptr;
	Node* newNode = new Node(val);
	
	while(pos < 0)
	{
		cout << "Invaild pos" << endl;
		exit(EXIT_FAILURE);
	}

	if(pos == 0)
	{
		newNode->next = nodePtr;
		head = newNode;
	}
	else
	{
		while(count < pos && nodePtr)
		{
			prevsNode = nodePtr;
			nodePtr = nodePtr->next;
			count++;
		}
	
	//gives newNode address of the node infront of it
		newNode->next = nodePtr;
	//makes the node behind newNode point to it
		prevsNode->next = newNode;
	}
}


void LinkedList::deleteNode(int val)
{
	Node* nodePtr = head;
	Node* prevsNode = nullptr;
	if(head->value == val)
	{
		nodePtr = nodePtr->next;
		delete head;
		head = nodePtr;
	}
	else
	{
		//transvers the list till it hits the value or the last node 
		while(nodePtr->next != nullptr && nodePtr->value != val)
		{
			prevsNode = nodePtr;
			nodePtr = nodePtr->next;
		}
		//makes sure that if its the last node that it has the value
			if(nodePtr->value == val)
			{
				prevsNode->next = nodePtr->next;
				delete nodePtr;
			}
			//other wise it says value not found
			else
			{
				cout << "Value not found" << endl;
			}
	}

}

void LinkedList::deleteNodeAt(int pos)
{
	int count = 0;
	Node* nodePtr = head;
	Node* prevsNode = nullptr;

	
	while(pos < 0)
	{
		cout << "Invaild pos" << endl;
		exit(EXIT_FAILURE);
	}

	if(pos == 0)
	{
		nodePtr = nodePtr->next;
		delete head;
		head = nodePtr;
	}
	else
	{
		while(count < pos && nodePtr)
		{
			prevsNode = nodePtr;
			nodePtr = nodePtr->next;
			count++;
		}
		prevsNode->next = nodePtr->next;
		delete nodePtr;
	}
}

void LinkedList::showlist()
{
	Node* nodePtr = head;
	while(nodePtr != nullptr)
		{
			cout << nodePtr->value << endl;
			nodePtr = nodePtr->next;
		}
}

void LinkedList::reverse(LinkedList &lists)
{
	int count = 0;
	int size = 0;
	Node* nodePtr = lists.head;

	while(nodePtr)
	{
		nodePtr = nodePtr->next;
		size++;
	}

	nodePtr = lists.head;
	while(size > 0)
	{
		while(count < size-1 && nodePtr)
			{
				nodePtr = nodePtr->next;
				count++;
			}
		size--;
		count = 0;
		lists.append(nodePtr->value);
		lists.deleteNode(nodePtr->value);
		nodePtr = lists.head;
	}
}

int LinkedList::findVal(int val)
{
	Node* nodePtr = head;
	int counter = 0;
	if(head->value == val)
	{
		return counter;
	}
	else
	{
		//transvers the list till it hits the value or the last node 
		while(nodePtr->next != nullptr && nodePtr->value != val)
		{
			nodePtr = nodePtr->next;
			counter++;
		}
		//makes sure that if its the last node that it has the value
			if(nodePtr->value == val)
			{
				return counter;
			}
			//other wise it says value not found
			else
			{
				cout << "Value not found" << endl;
				return -1;
			}
	}
}

LinkedList::~LinkedList()
{
	
	Node* nodePtr = head;
	Node* prevsNode = nullptr;
	while(head->next != nullptr)
	{
		delete head;
		head = nodePtr->next;
		nodePtr = head;
	}

}
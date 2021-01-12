/*Tests the LinkedList class
Made By Alexzander Ealy*/
#include "linkedInt.h"
#include <iostream>
using namespace std;

int main()
{
	LinkedList ListOfInts;

	cout << "Adding items to list" << endl
		 << "Here they are" << endl;
	ListOfInts.append(1);
	ListOfInts.append(2);
	ListOfInts.append(3);
	ListOfInts.append(4);
	
	ListOfInts.showlist();
	
	cout << endl;
	cout << endl;

	cout << "added 50 to list" << endl;
	ListOfInts.insert(50,1);

	cout << "The list with 50 in it" << endl;
	ListOfInts.showlist();
	cout << endl;
	cout << endl;

	cout << "The list with 50 removed" << endl;
	ListOfInts.deleteNode(50);
	ListOfInts.showlist();

	cout << "Made a new list with all the same values" << endl;
	LinkedList ListOfInts2(ListOfInts);
	cout << "Here they are" << endl;
	ListOfInts2.showlist();
	cout << endl;

	cout << "The list in reverse Order" << endl;
	ListOfInts2.reverse(ListOfInts2);
	ListOfInts2.showlist();

	cout << "Finding the postion of 3" << endl;
	cout << "Postion: " << ListOfInts2.findVal(3) << endl;

	cout << "Deleting the node at postion 1" << endl;
	ListOfInts2.deleteNodeAt(ListOfInts2.findVal(3));
	ListOfInts2.showlist();



return 0;
};

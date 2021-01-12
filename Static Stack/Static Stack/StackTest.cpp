/*Tests the Stack class
Made by Alexzander Ealy*/
#include <iostream>
#include "StaticStack.h"
using namespace std;


int main()
{
	StaticStack<int>stack1(10) ;
	int num;

	for(int index = 0; index < 11;index++)
	{
		if(!stack1.isFull())
			cout << " pushing " << index << endl;
		
			stack1.push(index);
	}

	StaticStack<int>stack2(stack1);

	
	for(int index = 0; index < 11;index++)
	{
		stack2.pop(num);
		if(!stack2.isEmpty())
			cout << num << endl;
	}
return 0;
}
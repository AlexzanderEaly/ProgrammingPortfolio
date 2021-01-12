/*Test the static que class
Made by Alexzander Ealy*/


#include "StaticQue.h"
#include <iostream>
using namespace std;

int main()
{
	StaticQue<int> que1(5);
	int num;

	for(int index = 0; index < 5;index++)
	{	
		que1.push(index);
		cout << "queuing " << index << endl;
	}
	for (int index = 0; index < 5;index++)
	{	
		que1.pop(num);
		cout << "dequeing " << num << endl;
	}
	que1.push(1);
	que1.pop(num);
	cout << "enqueuing 1" << endl;
	cout << "dequeuing " << num << endl;

return 0;
}
/*Static version of a stack
Made by Alexzander Ealy*/


#ifndef STATICSTACK_H
#define STATICSTACK_H
#include <iostream>
using namespace std;

template<class T>
	class StaticStack
	{
		private:
			T *stackPointer;//the way to acces the elements
			int top;		//holds the subscript the element thats about to be poped
			int size;		//holds the size of the array

		public:
			//conscutors
			StaticStack(StaticStack &);
			StaticStack(int );

			//deconstuctor
			~StaticStack();
			
			//stack ops
			bool isFull();
			bool isEmpty();
			void pop(T &);
			void push(T );
	};

	template<class T>
	StaticStack<T>::StaticStack<T>(StaticStack &copy)
	{
		size = copy.size;
		stackPointer = new T [copy.size];
		if(isEmpty())
			top = -1;
		else
		{
			for(int index = 0; index < copy.size;index++)
			{
				stackPointer[index] = copy.stackPointer[index];
			}
			top = copy.top;
		}
	}

	template<class T>
	StaticStack<T>::StaticStack(int s)
	{
		stackPointer = new T [s];
		size = s;
		top = -1;
	}

	template <class T>
	bool StaticStack<T>::isFull()
	{
		if(top == (size-1))
			return true;
		else
			return false;
	}

	template <class T>
	bool StaticStack<T>::isEmpty()
	{
		if(top == -1)
			return true;
		else
			return false;
	}

	template <class T>
	void StaticStack<T>::pop(T &num)
	{
		if(isEmpty())
			cout << "Stack is empty" << endl;
		else
		{
			num = stackPointer[top];
			top--;
		}
	}

	template <class T>
	void StaticStack<T>::push(T num)
	{
		if(isFull())
			cout << "Stack is full" << endl;
		else
		{
			top++;
			stackPointer[top] = num;
		}
	}

	
	template <class T>
	StaticStack<T>::~StaticStack()
	{
		if(size > 0)
			delete [] stackPointer;

	}





#endif
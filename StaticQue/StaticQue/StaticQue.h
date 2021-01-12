/*Static version of a queue
Made by Alexzander Ealy*/


#ifndef STATICQUE
#define STATICQUE
#include <iostream>
using namespace std;

template<class T>
	class StaticQue
	{
		private:
			T *QuePointer;//the way to acces the elements
			int front;		//holds the front of the array
			int rear;		//the rear of the array
			int size;		//holds the size of the array
			int numOfItems;


	public:
			//conscutors
			StaticQue(StaticQue &);
			StaticQue(int );

			//deconstuctor
			~StaticQue();
			
			//stack ops
			bool isFull();
			bool isEmpty();
			void pop(T &);
			void push(T );
	};

	template<class T>
	StaticQue<T>::StaticQue(StaticQue &copy)
	{
		QuePointer = new T[copy.size];
		size = copy.size;
		front = copy.front;
		rear = copy.rear;
		numOfItems = copy.numOfItems;
   
		for (int count = 0; count < copy.size; count++)
			 QuePointer[count] = copy.QuePointer[count];
	}

	template<class T>
	StaticQue<T>::StaticQue(int s)
	{
		QuePointer = new T [s];
		size = s;
		front = -1;
		rear = -1;
		numOfItems = 0;
	}

	template <class T>
	bool StaticQue<T>::isFull()
	{
		 bool status;

		if (numOfItems < size)
			 status = false;
		else
			 status = true;

		return status;
	}

	template <class T>
	bool StaticQue<T>::isEmpty()
	{
		if(numOfItems == 0)
			return true;
		else 
			return false;
	}

	template <class T>
	void StaticQue<T>::pop(T &num)
	{
		if(!isEmpty())
		{
			front = (front + 1) % size;
			num = QuePointer[front];
			numOfItems--;
		}
		else
			cout << "Queue is empty" << endl;
	}

	template <class T>
	void StaticQue<T>::push(T num)
	{
		if(!isFull())
		{
			rear = (rear + 1) % size;
			QuePointer[rear] = num;
			numOfItems++;
		}
		else
		{
			cout << "Queue is full" << endl;
		}
	}

	
	template <class T>
	StaticQue<T>::~StaticQue()
	{
		if(size > 0)
			delete [] QuePointer;

	}





#endif
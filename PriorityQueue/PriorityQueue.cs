using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriorityQueue
{
    public class PriorityQueue<T> where T : IComparable<T>
    {
		private T[] data;
		private int heapSize;
		private Comparison<T> compare;
		private Dictionary<T, int> indexDict;

#region Constructors

		public PriorityQueue(int capacity)
			: this(capacity, Comparer<T>.Default)
		{
		}

		public PriorityQueue(int capacity, IComparer<T> comparer)
			: this(capacity, comparer.Compare)
		{
		}

		public PriorityQueue(int capacity, Comparison<T> comparison)
		{
			data = new T[capacity];
			indexDict = new Dictionary<T, int>(capacity);
			heapSize = 0;
			compare = comparison;
		}


		public PriorityQueue(IEnumerable<T> values)
			: this(values, Comparer<T>.Default)
		{
		}

		public PriorityQueue(IEnumerable<T> values, IComparer<T> comparer) 
			: this(values, comparer.Compare)
		{
		}

		public PriorityQueue(IEnumerable<T> values, Comparison<T> comparison)
		{
			data = values.ToArray();
			heapSize = values.Count();
			indexDict = new Dictionary<T, int>(heapSize);
			compare = comparison;
			BuildMinHeap();
		}

#endregion

		/// <summary>
		/// Return the number of element in queue.
		/// </summary>
		public int Count => heapSize;

		/// <summary>
		/// Return the capacity of queue.
		/// </summary>
		public int Capacity => data.Length;

		public T Front => data[0];

		public IEnumerable<T> Sort()
		{
			for (int i = heapSize - 1; i > 0; --i)
			{
				Swap(0, i);
				--heapSize;
				MinHeapify(0);
			}
			return data.AsEnumerable().Reverse();
		}

		public T Dequeue()
		{
			if (heapSize < 1)
				throw new IndexOutOfRangeException("Heap Underflowed.");
			var min = data[0];
			data[0] = data[--heapSize];
			indexDict[data[0]] = heapSize;
			MinHeapify(0);
			return min;
		}

		public void Enqueue(T item)
		{
			DecreaseKey(heapSize++, item);
		}

		public void BuildMinHeap()
		{
			heapSize = data.Length;
			for (int i = Parent(heapSize - 1); i >= 0; --i)
				MinHeapify(i);
		}

		public void DecreaseKey(int index, T key)
		{
			var idxParent = Parent(index);
			data[index] = key;
			indexDict[key] = index;
			while (index > 0 && compare(data[idxParent], data[index]) > 0)
			{
				Swap(index, idxParent);
				index = idxParent;
				idxParent = Parent(index);
			}
		}

		public void DecreaseKey(T oldKey, T newKey)
		{
			int index = indexDict[oldKey];

			DecreaseKey(index, newKey);
		}

		private void MinHeapify(int index)
		{
			int left = Left(index);
			int right = Right(index);
			int smallest;

			if (left < heapSize && compare(data[left], data[index]) < 0)
				smallest = left;
			else
				smallest = index;

			if (right < heapSize && compare(data[right], data[smallest]) < 0)
				smallest = right;
			if (smallest != index)
			{
				Swap(index, smallest);
				MinHeapify(smallest);
			}
		}

		private int Parent(int index)
		{
			return (index - 1) / 2;
		}

		private int Left(int index)
		{
			return (index * 2) + 1;
		}

		private int Right(int index)
		{
			return (index * 2) + 2;
		}

		private void Swap(int lhs, int rhs)
		{
			var temp = data[lhs];
			indexDict[data[lhs]] = rhs;
			indexDict[data[rhs]] = lhs;
			data[lhs] = data[rhs];
			data[rhs] = temp;
		}

    }
}

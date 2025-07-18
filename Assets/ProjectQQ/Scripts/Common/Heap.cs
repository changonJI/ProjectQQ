using System;
using System.Collections;

/// <summary>
/// 이진트리_기준값 가장 우선되는 대상을 위로 띄우기
/// </summary>
/// <typeparam name="T"></typeparam>
public class Heap<T> where T : IHeapItem<T>
{
    private readonly T[] items;
    private int currentItemCount;

    public Heap(int maxHeapSize)
    {
        items = new T[maxHeapSize];
    }

    /// <summary>
    /// 힙에 대상 추가하기
    /// </summary>
    /// <param name="item">추가할 대상 입력</param>
    public void Add(T item)
    {
        item.HeapIndex = currentItemCount;
        items[currentItemCount] = item;
        SortUp(item);
        ++currentItemCount;
    }

    /// <summary>
    /// 가장 앞의 것 뽑아오기 (힙에서는 제거됨)
    /// </summary>
    /// <returns>가장 앞에 있던 것 반환</returns>
    public T Pop()
    {
        T firstItem = items[0];
        --currentItemCount;
        items[0] = items[currentItemCount];
        items[0].HeapIndex = 0;
        SortDown(items[0]);

        return firstItem;
    }

    /// <summary>
    /// 모든 값 지우기
    /// </summary>
    public void Clear()
    {
        Array.Clear(items, 0, items.Length);
        currentItemCount = 0;
    }

    public void UpdateItem(T item)
    {
        SortUp(item);
    }

    public int Count => currentItemCount;

    public bool Contains(T item)
    {
        return Equals(items[item.HeapIndex], item);
    }

    // 트리 위쪽으로 정렬하며 올리기
    void SortUp(T item)
    {
        int parentIndex = (item.HeapIndex - 1) / 2;

        while (true)
        {
            T parentItem = items[parentIndex];
            if (item.CompareTo(parentItem) > 0) // item이 parent보다 우선되는 경우
            {
                Swap(item, parentItem);
            }

            parentIndex = (item.HeapIndex - 1) / 2;
        }
    }
    
    // 트리 아래쪽으로 정렬하며 내리기
    void SortDown(T item)
    {
        while (true)
        {
            int childIndexLeft = item.HeapIndex * 2 + 1;
            int childIndexRight = item.HeapIndex * 2 + 2;
            int swapIndex = 0;

            if (childIndexLeft < currentItemCount)
            {
                swapIndex = childIndexLeft;

                if (childIndexRight < currentItemCount)
                {
                    if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0)
                    {
                        swapIndex = childIndexRight;
                    }
                }

                if (item.CompareTo(items[swapIndex]) < 0)
                {
                    Swap(item, items[swapIndex]);
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }
    }

    void Swap(T itemA, T itemB)
    {
        items[itemA.HeapIndex] = itemB;
        items[itemB.HeapIndex] = itemA;

        int tmpItemIndex = itemA.HeapIndex;
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = tmpItemIndex;
    }
}

public interface IHeapItem<T> : IComparable<T>
{
    int HeapIndex
    {
        get; set;
    }
}
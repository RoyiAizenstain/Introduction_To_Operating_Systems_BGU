// This class implements a multi-threaded merge sort algorithm for sorting an array of strings.
// It uses a divide-and-conquer approach to sort the array in parallel, improving performance for large datasets.
class MTMergeSort
{
    public static List<string> MergeSort(string[] inputArray, int minimumSize = 2)
    {
        if (inputArray == null || inputArray.Length <= 1)
            return new List<string>(inputArray ?? Array.Empty<string>()); // Return an empty or single-element list

        // Create a temporary array for merging
        string[] auxiliaryArray = new string[inputArray.Length];

        // Start the multi-threaded merge sort
        MergeSortRec(inputArray, auxiliaryArray, 0, inputArray.Length - 1, minimumSize);

        // Convert the sorted array to a list and return it
        return new List<string>(inputArray);
    }

    private static void MergeSortRec(string[] inputArray, string[] auxiliaryArray, int startIndex, int endIndex, int minimumSize)
    {
        if (startIndex >= endIndex)
            return;

        int middleIndex = (startIndex + endIndex) / 2;

        if (endIndex - startIndex + 1 > minimumSize)
        {
            // Use threads for left and right halves
            Thread leftPartThread = new Thread(() => MergeSortRec(inputArray, auxiliaryArray, startIndex, middleIndex, minimumSize));
            Thread rightPartThread = new Thread(() => MergeSortRec(inputArray, auxiliaryArray, middleIndex + 1, endIndex, minimumSize));

            leftPartThread.Start();
            rightPartThread.Start();

            leftPartThread.Join();
            rightPartThread.Join();
        }
        else
        {
            // Sort sequentially if the size is below the threshold
            MergeSortRec(inputArray, auxiliaryArray, startIndex, middleIndex, minimumSize);
            MergeSortRec(inputArray, auxiliaryArray, middleIndex + 1, endIndex, minimumSize);
        }

        // Merge the sorted halves
        Merge(inputArray, auxiliaryArray, startIndex, middleIndex, endIndex);
    }

    private static void Merge(string[] inputArray, string[] auxiliaryArray, int startIndex, int middleIndex, int endIndex)
    {
        int leftIndex = startIndex, rightIndex = middleIndex + 1, resultIndex = startIndex;

        // Merge the two halves into the temporary array
        while (leftIndex <= middleIndex && rightIndex <= endIndex)
        {
            if (string.Compare(inputArray[leftIndex], inputArray[rightIndex], StringComparison.Ordinal) <= 0)
            {
                auxiliaryArray[resultIndex++] = inputArray[leftIndex++];
            }
            else
            {
                auxiliaryArray[resultIndex++] = inputArray[rightIndex++];
            }
        }

        // Copy remaining elements from the left half
        while (leftIndex <= middleIndex)
        {
            auxiliaryArray[resultIndex++] = inputArray[leftIndex++];
        }

        // Copy remaining elements from the right half
        while (rightIndex <= endIndex)
        {
            auxiliaryArray[resultIndex++] = inputArray[rightIndex++];
        }

        // Copy the sorted elements back to the original array
        for (int copyIndex = startIndex; copyIndex <= endIndex; copyIndex++)
        {
            inputArray[copyIndex] = auxiliaryArray[copyIndex];
        }
    }

    /*
    // Example usage of the MTMergeSort class
    public static void Main(string[] args)
    {
        // Example usage
        string[] inputArray = { "banana","aaa", "apple", "orange", "grape","bbbb", "kiwi" };
        List<string> sortedList = MergeSort(inputArray, 2);
        Console.WriteLine("Sorted Array:");
        foreach (var item in sortedList)
        {
            Console.WriteLine(item);
        }
    }
    */
}
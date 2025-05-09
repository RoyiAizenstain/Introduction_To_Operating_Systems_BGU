using System.Diagnostics;
    /*
     * This class is used to multiply two matrices concurrently using multiple threads
     */
    class MatrixMultiplier
    {
        /// Method to multiply two matrices concurrently
        public static void MultiplyMatricesConcurrently(int[,] matrix1, int[,] matrix2, int[,] result, int totalRows1, int totalCols1, int totalCols2, int threadCount)
        {
            // Create an array of threads
            Thread[] workerThreads = new Thread[threadCount];
            int rowsPerThread = totalRows1 / threadCount;

            // Create and start threads
            for (int threadIndex = 0; threadIndex < threadCount; threadIndex++)
            {
                // Calculate the start and end row for each thread
                int startRow = threadIndex * rowsPerThread;
                int endRow = (threadIndex == threadCount - 1) ? totalRows1 : startRow + rowsPerThread;

                // Create a new thread to multiply the rows of the matrices
                workerThreads[threadIndex] = new Thread(() => MultiplyRows(matrix1, matrix2, result, startRow, endRow, totalCols1, totalCols2));
                workerThreads[threadIndex].Start();
            }

            // Wait for all threads to finish
            foreach (var thread in workerThreads)
            {
                thread.Join();
            }
        }

        // Method to multiply a portion of the matrices
        private static void MultiplyRows(int[,] matrix1, int[,] matrix2, int[,] result, int startRow, int endRow, int totalCols1, int totalCols2)
        {
            // Perform the multiplication for the assigned rows
            for (int row = startRow; row < endRow; row++)
            {
                // Multiply the rows of matrix1 with the columns of matrix2
                for (int col = 0; col < totalCols2; col++)
                {
                    result[row, col] = 0;
                    for (int k = 0; k < totalCols1; k++)
                    {
                        result[row, col] += matrix1[row, k] * matrix2[k, col];
                    }
                }
            }
        }
        /*
        // Example usage of the MatrixMultiplier class
        public static void Main(string[] args)
        {
            Console.WriteLine($"Process ID: {Process.GetCurrentProcess().Id}");
            int rowsA = 10;
            int colsA = 50;
            int colsB = 10;
            int numThreads = 2;
            // Initialize matrices
            int[,] matrixA = new int[rowsA, colsA];
            int[,] matrixB = new int[colsA, colsB];
            int[,] resultMatrix = new int[rowsA, colsB];
            // Fill matrices with random values
            Random rand = new Random();
            for (int i = 0; i < rowsA; i++)
                for (int j = 0; j < colsA; j++)
                    matrixA[i, j] = rand.Next(1, 10);
            for (int i = 0; i < colsA; i++)
                for (int j = 0; j < colsB; j++)
                    matrixB[i, j] = rand.Next(1, 10);
            // Multiply matrices concurrently
            MultiplyMatricesConcurrently(matrixA, matrixB, resultMatrix, rowsA, colsA, colsB, numThreads);
            // Print the result
            Console.WriteLine("Result Matrix:");
            for (int i = 0; i < rowsA; i++)
            {
                for (int j = 0; j < colsB; j++)
                {
                    Console.Write(resultMatrix[i, j] + "\t");
                }
                Console.WriteLine();
            }
        }
        */


    }



using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace ImageSearch
{
    class Program
    {
        static void Main(string[] args)
        {
            // Check if the correct number of arguments are provided
            if (args.Length != 4)
            {
                Console.WriteLine("Usage: ImageSearch <image1> <image2> <nThreads> <algorithm>");
                return;
            }
            //parse arguments
            string largeImagePath = args[0];
            string smallImagePath = args[1];

            // Check if the number of threads is a positive integer
            if (!int.TryParse(args[2], out int threadCount) || threadCount < 1)
            {
                Console.WriteLine("Error: nThreads must be a positive integer.");
                return;
            }
            // Check if the algorithm is either "exact" or "euclidian"
            string algorithm = args[3].ToLower();
            if (algorithm != "exact" && algorithm != "euclidian")
            {
                Console.WriteLine("Error: algorithm must be either 'exact' or 'euclidian'.");
                return;
            }
            // Check if the image files exist
            try
            {
                // Check if the image files exist
                if (!File.Exists(largeImagePath))
                {
                    Console.WriteLine($"Error: Image file '{largeImagePath}' does not exist.");
                    return;
                }

                // Check if the image files exist
                if (!File.Exists(smallImagePath))
                {
                    Console.WriteLine($"Error: Image file '{smallImagePath}' does not exist.");
                    return;
                }

                // Load the images
                using (Bitmap largeImage = new Bitmap(largeImagePath))
                using (Bitmap smallImage = new Bitmap(smallImagePath))
                {
                    // Check if the images are in a supported format
                    Bitmap largeImageConverted = ConvertToFormat(largeImage, PixelFormat.Format32bppArgb);
                    Bitmap smallImageConverted = ConvertToFormat(smallImage, PixelFormat.Format32bppArgb);

                    // Find all matches
                    var matches = FindAllMatches(largeImageConverted, smallImageConverted, threadCount, algorithm);

                    // Print the results
                    if (matches.Count > 0)
                    {
                        // Print the matches
                        foreach (var match in matches)
                        {
                            Console.WriteLine($"> {match.X},{match.Y}");
                        }
                    }
                    // Dispose of the images
                    if (largeImageConverted != largeImage) largeImageConverted.Dispose();
                    if (smallImageConverted != smallImage) smallImageConverted.Dispose();
                }
            }
            // Handle exceptions
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        // ConvertToFormat method to convert the image to a specific pixel format
        static Bitmap ConvertToFormat(Bitmap source, PixelFormat format)
        {
            if (source.PixelFormat == format)
                return source;

            // Create a new bitmap with the desired pixel format
            Bitmap converted = new Bitmap(source.Width, source.Height, format);
            // Draw the source image onto the new bitmap
            using (Graphics g = Graphics.FromImage(converted))
            {
                // Set the interpolation mode for better quality
                g.DrawImage(source, new Rectangle(0, 0, source.Width, source.Height));
            }
            return converted;
        }

        // FindAllMatches method to find all matches of the small image in the large image using multithreading
        static List<Point> FindAllMatches(Bitmap largeImage, Bitmap smallImage, int threadCount, string algorithm)
        {
            // Check if the images are valid
            int largeWidth = largeImage.Width;
            int largeHeight = largeImage.Height;
            int smallWidth = smallImage.Width;
            int smallHeight = smallImage.Height;
            if (smallWidth > largeWidth || smallHeight > largeHeight)
            {
                return new List<Point>();
            }

            // Calculate the maximum number of positions to search
            int maxX = largeWidth - smallWidth;
            int maxY = largeHeight - smallHeight;
            int totalPositions = (maxX + 1) * (maxY + 1);

            // Create a concurrent collection to store the matches
            var matches = new ConcurrentBag<Point>();

            // Lock the bits of the images for faster access
            BitmapData largeData = largeImage.LockBits(
                new Rectangle(0, 0, largeWidth, largeHeight),
                ImageLockMode.ReadOnly, largeImage.PixelFormat);

            // Lock the bits of the small image for faster access
            BitmapData smallData = smallImage.LockBits(
                new Rectangle(0, 0, smallWidth, smallHeight),
                ImageLockMode.ReadOnly, smallImage.PixelFormat);

            // Calculate the stride and bytes per pixel for both images
            int bytesPerPixel = Image.GetPixelFormatSize(largeImage.PixelFormat) / 8;
            int largeStride = largeData.Stride;
            int smallStride = smallData.Stride;

            // Calculate the width and height of the images
            IntPtr largePtr = largeData.Scan0;
            IntPtr smallPtr = smallData.Scan0;

            // Create byte arrays to hold the pixel data for both images
            byte[] largeBytes = new byte[largeStride * largeHeight];
            byte[] smallBytes = new byte[smallStride * smallHeight];

            // Copy the pixel data from the images to the byte arrays
            Marshal.Copy(largePtr, largeBytes, 0, largeBytes.Length);
            Marshal.Copy(smallPtr, smallBytes, 0, smallBytes.Length);

            // Calculate the number of positions each thread will process
            int positionsPerThread = (int)Math.Ceiling((double)totalPositions / threadCount);

            // Create and start the threads
            Thread[] threads = new Thread[threadCount];
            
            for (int t = 0; t < threadCount; t++)
            {
                // Create a new thread for each thread count
                int threadIndex = t;
                // Create a new thread to process the positions
                threads[t] = new Thread(() =>
                {
                    // Calculate the start and end positions for this thread
                    int startPosition = threadIndex * positionsPerThread;
                    int endPosition = Math.Min(startPosition + positionsPerThread, totalPositions);

                    // Calculate the start and end Y positions
                    for (int pos = startPosition; pos < endPosition; pos++)
                    {
                        // Calculate the X and Y positions
                        int x = pos % (maxX + 1);
                        int y = pos / (maxX + 1);

                        // Check if the position is valid
                        bool isMatched;
                        // Check if the algorithm is "exact" or "euclidian"
                        if (algorithm == "exact")
                        {
                            // use the exact match algorithm
                            isMatched = IsExactMatch(largeBytes, smallBytes, x, y, largeWidth, largeHeight,
                                                    smallWidth, smallHeight, largeStride, smallStride, bytesPerPixel);
                        }
                        else
                        {
                            // use the euclidian match algorithm
                            isMatched = IsEuclidianMatch(largeBytes, smallBytes, x, y, largeWidth, largeHeight,
                                                        smallWidth, smallHeight, largeStride, smallStride, bytesPerPixel);
                        }

                        // If a match is found, add it to the matches collection
                        if (isMatched)
                        {
                            matches.Add(new Point(x, y));
                        }
                    }
                });
                // Start the thread
                threads[t].Start();
            }
            // Wait for all threads to finish
            foreach (var thread in threads)
            {
                thread.Join();
            }

            // Unlock the bits of the images
            largeImage.UnlockBits(largeData);
            smallImage.UnlockBits(smallData);

            //return the matches
            return new List<Point>(matches);
        }
        // IsExactMatch method to check if the small image matches the large image at a specific position
        static bool IsExactMatch(byte[] largeBytes, byte[] smallBytes, int startX, int startY,
                                int largeWidth, int largeHeight, int smallWidth, int smallHeight,
                                int largeStride, int smallStride, int bytesPerPixel)
        {
            // loop through the small image and check if it matches the large image
            for (  int y = 0; y < smallHeight; y++)
            {
                for (int x = 0; x < smallWidth; x++)
                {
                    // Calculate the position in the large and small images
                    int largePos = ((startY + y) * largeStride) + ((startX + x) * bytesPerPixel);
                    int smallPos = (y * smallStride) + (x * bytesPerPixel);

                    // Check if the position is valid
                    for (int c = 0; c < bytesPerPixel; c++)
                    {
                        if (largeBytes[largePos + c] != smallBytes[smallPos + c])
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        // IsEuclidianMatch method to check if the small image matches the large image at a specific position using Euclidean distance
        static bool IsEuclidianMatch(byte[] largeBytes, byte[] smallBytes, int startX, int startY,
                                    int largeWidth, int largeHeight, int smallWidth, int smallHeight,
                                    int largeStride, int smallStride, int bytesPerPixel)
        {
            double totalDistance = 0;

            // loop through the small image and check if it matches the large image
            for (int y = 0; y < smallHeight; y++)
            {
                for (int x = 0; x < smallWidth; x++)
                {
                    // Calculate the position in the large and small images
                    int largePos = ((startY + y) * largeStride) + ((startX + x) * bytesPerPixel);
                    int smallPos = (y * smallStride) + (x * bytesPerPixel);
                    
                    double pixelDistance = 0;
                    // Calculate the Euclidean distance between the pixels
                    for (int c = 0; c < 3; c++)
                    {
                        int diff = largeBytes[largePos + c] - smallBytes[smallPos + c];
                        pixelDistance += diff * diff;
                    }
                    // Add the alpha channel to the distance calculation
                    double distance = Math.Sqrt(pixelDistance);
                    totalDistance += distance;
                    // Check if the distance is greater than a threshold
                    if (totalDistance > 0)
                    {
                        return false;
                    }
                }
            }
            // Check if the total distance is within a threshold
            return totalDistance == 0;
        }
    }
}

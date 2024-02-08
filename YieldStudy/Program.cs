using System;
using System.Collections.Generic;
using System.Linq;

namespace YieldStudy
{
    internal class Program
    {
        // Example of a client request. This approach simplifies client interaction by
        // eliminating the need for pagination concerns. The yield instruction facilitates
        // an easier consumption of this database.
        static void Main(string[] args)
        {
            MockDatabaseClient client = new MockDatabaseClient();

            //Get all the elements
            IEnumerable<string> elements = client.GetAllElements();

            // Just iterate throw the elements, easy!
            foreach (string element in elements)
            {
                Console.WriteLine(element);
            }
        }
    }

    /// <summary>
    /// Encapsulates the logic for paginating access to the mock database. This allows
    /// clients to retrieve all elements from the database without having to handle
    /// pagination themselves.
    /// </summary>
    public class MockDatabaseClient
    {
        private List<string> _mockDatabase = new List<string>() { "A", "B", "C", "D", "E" };

        /// <summary>
        /// Retrieves all elements from the mock database.
        /// </summary>
        public IEnumerable<string> GetAllElements()
        {
            IEnumerable<string> elements;
            int page = 0;
            int pagesize = 2;
            
            do 
            {
                elements = MockGETRequest(page++, pagesize);

                foreach (string element in elements)
                {
                    yield return element;
                }
            } 
            while (elements.Count() > 0); // If there are no more elements, stop making requests.
        }

        /// <summary>
        /// Retrieves elements from the mock database using pagination.
        /// </summary>
        /// <param name="page">The page number to retrieve.</param>
        /// <param name="pageSize">The maximum number of elements to return per page.</param>
        private IEnumerable<string> MockGETRequest(int page, int pagesize)
        {
            int skip = (page - 1) * pagesize;
            return _mockDatabase
                .Skip(skip)
                .Take(pagesize);
        }
    }


}

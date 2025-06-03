using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamPracticeJuneThird.Repository.Helpers
{
    public class Filter // intended to store filter conditions used to generate a dynamic SQL WHERE clause when retrieving data from a database (like in RetrieveCollectionAsync)
    {                   // It allows code elsewhere to easily define and pass filtering criteria
        public static Filter Empty => new Filter(); // a shortcut for creating a new, empty filter
        public Dictionary<string, object> Conditions { get; set; } = new Dictionary<string, object>();
        public Filter AddCondition(string field, object value) // Adds a new condition to the Conditions dictionary
        {
            Conditions[field] = value; // If the field already exists, it updates the value.
            return this; // Returns the Filter instance itself to allow method chaining.
        }
    }
}

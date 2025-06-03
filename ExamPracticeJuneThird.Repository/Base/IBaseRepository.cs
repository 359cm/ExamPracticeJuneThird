namespace ExamPracticeJuneThird.Repository.Base
{
    public interface IBaseRepository <TObj, TFilter, TUpdate>
        // TObj: The type of the object being stored (e.g., User, Product, Order, etc.)
        // TFilter: The type used to filter a collection of TObj items(for example, a search or query object)
        // TUpdate: The type used for updating an existing TObj(can be a partial update model or DTO)

        where TObj : class // ensures that TObj must be a reference type (not a value type like int, bool, etc.)
    {
        Task<int> CreateAsync(TObj entity); // creates a new object in the data store
        Task<TObj> RetrieveAsync(int objectId); // retrieves a single object by its ID
        IAsyncEnumerable<TObj> RetrieveCollectionAsync(TFilter filter); // retrieves a collection of TObj items that match a given filter
        Task<bool> UpdateAsync(int objectId, TUpdate update); // updates an object by ID
        Task<bool> DeleteAsync(int objectId); // deletes an object by its ID
    }
}
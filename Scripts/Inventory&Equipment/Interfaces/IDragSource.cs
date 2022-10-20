public interface IDragSource<T> where T : class
{
    T GetItem();
    int GetNumber();
    void RemoveItem(int number);
}

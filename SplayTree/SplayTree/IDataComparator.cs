namespace SplayTree.SplayTree
{
    public interface IDataComparator<in TDataType>
    {
        int Compare<T>(TDataType paCompared1, TDataType paCompared2);
    }
}
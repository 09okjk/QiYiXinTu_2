namespace Save
{
    public interface ISaveable
    {
        object SaveData();           // 返回要保存的数据（通常是自定义数据类）
        void LoadData(object data);  // 传入保存的数据，恢复状态
    }
}
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AAC.Databases
{
    public class GroupNote
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }
        public string RunnerName { get; set; }
    }
    public class GroupsDatabase
    {
        readonly SQLiteAsyncConnection _database;

        public GroupsDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<GroupNote>().Wait();
        }

        public Task<List<GroupNote>> GetGroupList() => _database.Table<GroupNote>().ToListAsync();
        public Task<int> SaveGroupsNote(GroupNote data) => _database.InsertAsync(data);
        public Task<int> RemoveGroupNote(GroupNote group) => _database.Table<GroupNote>().DeleteAsync(g => g.Name == group.Name && g.RunnerName == group.RunnerName);
        public Task<int> ClearAllGroups() => _database.DeleteAllAsync(new TableMapping(typeof(GroupNote)));
    }
}

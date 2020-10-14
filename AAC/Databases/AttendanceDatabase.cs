using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AAC.Databases
{
    public class AttendanceNote
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime AttendanceDateTime { get; set; }
    }
    public class AttendanceDatabase
    {
        readonly SQLiteAsyncConnection _database;

        public AttendanceDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<AttendanceNote>().Wait();
        }

        public Task<List<AttendanceNote>> GetAttendanceList() => _database.Table<AttendanceNote>().ToListAsync();
        public Task<int> SaveAttendanceNote(AttendanceNote data) => _database.InsertAsync(data);
        public Task<int> ClearAllAttendances() => _database.DeleteAllAsync(new TableMapping(typeof(AttendanceNote)));
    }
}

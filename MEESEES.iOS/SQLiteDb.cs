using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Foundation;
using UIKit;
using MEESEES.Data;
using MEESEES.iOS;
using Xamarin.Forms;
using SQLite;

[assembly: Dependency(typeof(SQLiteDb))]
namespace MEESEES.iOS
{
    public class SQLiteDb : ISQLiteDb
    {
        public SQLiteAsyncConnection GetConnection()
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var path = Path.Combine(documentsPath, "meesees.db3");
            return new SQLiteAsyncConnection(path);
        }
    }
}
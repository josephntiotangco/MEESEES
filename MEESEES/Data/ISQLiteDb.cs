﻿using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace MEESEES.Data
{
    public interface ISQLiteDb
    {
        SQLiteAsyncConnection GetConnection();
    }
}

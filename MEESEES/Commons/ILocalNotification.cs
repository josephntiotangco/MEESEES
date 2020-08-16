using System;
using System.Collections.Generic;
using System.Text;

namespace MEESEES.Commons
{
    public interface ILocalNotification
    {
        //void LocalNotification(string title, string body, int id, DateTime notifyTime);
        //void Cancel(int id);
        void CreateNotification(string title, string message, int schedule);
    }
}

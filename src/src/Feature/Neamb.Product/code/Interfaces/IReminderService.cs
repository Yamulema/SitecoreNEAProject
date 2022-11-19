using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neambc.Neamb.Feature.Product.Model;

namespace Neambc.Neamb.Feature.Product.Interfaces
{
    public interface IReminderService
    {
        bool SetReminder(string id, string mdsid);
        bool HasReminder(string id, string mdsid);
        Reminder GetReminder(SweepstakesDTO model);
    }
}
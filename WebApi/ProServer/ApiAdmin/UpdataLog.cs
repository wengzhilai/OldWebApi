using ProInterface;
using ProInterface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;
using System.Data.Entity.Validation;

namespace ProServer.ApiAdmin
{
    public class UpdataLog<T>: IUpdataLog<T>
    {
        private DBEntities _db;
        private string oldEntContent;
        private T _oldEnt;
        public UpdataLog(DBEntities db,T oldEnt)
        {
            _db = db;
            _oldEnt = oldEnt;
            oldEntContent =JSON.DecodeToStrNoRec(oldEnt);
        }
        public UpdataLog(DBEntities db)
        {
            _db = db;
        }

        public bool UpdataLogSaveCreate(GlobalUser gu, T newEnt)
        {
            _db.fa_updata_log.Add(new fa_updata_log
            {
                ID=Fun.GetSeqID<fa_updata_log>(),
                CREATE_TIME = DateTime.Now,
                CREATE_USER_ID = gu.UserId,
                CREATE_USER_NAME = gu.UserName,
                NEW_CONTENT = JSON.DecodeToStrNoRec(newEnt),
                TABLE_NAME = newEnt.GetType().Name,
            });
            return true;
        }

        public bool UpdataLogSaveUdate(GlobalUser gu, T newEnt)
        {
            _db.fa_updata_log.Add(new fa_updata_log
            {
                CREATE_TIME = DateTime.Now,
                CREATE_USER_ID = gu.UserId,
                CREATE_USER_NAME = gu.UserName,
                OLD_CONTENT = oldEntContent,
                NEW_CONTENT = JSON.DecodeToStrNoRec(newEnt),
                TABLE_NAME = newEnt.GetType().Name
            });
            return true;
        }

        public bool UpdataLogDelete(GlobalUser gu)
        {
            _db.fa_updata_log.Add(new fa_updata_log
            {
                CREATE_TIME = DateTime.Now,
                CREATE_USER_ID = gu.UserId,
                CREATE_USER_NAME = gu.UserName,
                OLD_CONTENT = oldEntContent,
                TABLE_NAME = _oldEnt.GetType().Name
            });
            return true;
        }
    }
}

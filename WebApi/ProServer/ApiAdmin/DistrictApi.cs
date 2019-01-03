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
    public class DistrictApi
    {
        public bool DistrictSave(string loginKey, ref ErrorInfo err, DISTRICT inEnt, IList<string> allPar=null)
        {
            if (!GlobalFun.UserCheckFunctioAuthority(loginKey, ref err, MethodBase.GetCurrentMethod())) return false;
            using (DBEntities db = new DBEntities())
            {
                var ent = db.fa_district.SingleOrDefault(a => a.ID == inEnt.ID);
                if (ent == null)
                {
                    ent = Mapper.Map<fa_district>(inEnt);
                    db.fa_district.Add(ent);
                }
                else
                {
                    ent = Fun.ClassToCopy<DISTRICT, fa_district>(inEnt, ent, allPar);
                }
                return Fun.DBEntitiesCommit(db, ref err);
            }
        }

        public DISTRICT DistrictSingle(string loginKey, ref ErrorInfo err, int id)
        {
            if (!GlobalFun.UserCheckFunctioAuthority(loginKey, ref err, MethodBase.GetCurrentMethod())) return null;
            using (DBEntities db = new DBEntities())
            {
                var reEnt = db.fa_district.SingleOrDefault(x => x.ID == id);
                return Mapper.Map<DISTRICT>(reEnt);
            }
        }
    }
}

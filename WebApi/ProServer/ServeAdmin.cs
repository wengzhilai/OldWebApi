
using ProServer.ApiAdmin;

namespace ProServer
{
    public class ServeAdmin
    {
        private DistrictApi _DistrictApi;
        public ServeAdmin()
        {
            MapperCfg.Configuration.Configure();
        }
        public DistrictApi DistrictApi
        {
            get
            {
                if (_DistrictApi == null) { _DistrictApi = new DistrictApi(); }
                return _DistrictApi;
            }

            set
            {
                _DistrictApi = value;
            }
        }
    }
}

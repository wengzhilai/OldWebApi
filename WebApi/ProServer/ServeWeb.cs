using ProInterface;
using ProServer.ApiWeb;

namespace ProServer
{
    public class ServeWeb
    {
        private IFamilyApi _FamilyApi;
        private IPublicApi _PublicApi;
        private IUserApi _UserApi;
        private IUserInfoApi _UserInfoApi;
        private IRoleApi _RoleApi;

        public ServeWeb() {
            MapperCfg.Configuration.Configure();
        }


        public IUserInfoApi UserInfoApi
        {
            get
            {
                if (_UserInfoApi == null) { _UserInfoApi = new UserInfoApi(this); }
                return _UserInfoApi;
            }

            set
            {
                _UserInfoApi = value;
            }
        }

        public IUserApi UserApi
        {
            get
            {
                if (_UserApi == null) { _UserApi = new UserApi(); }
                return _UserApi;
            }

            set
            {
                _UserApi = value;
            }
        }

        public IRoleApi RoleApi
        {
            get
            {
                if (_RoleApi == null) { _RoleApi = new RoleApi(); }
                return _RoleApi;
            }
            set
            {
                _RoleApi = value;
            }
        }

        public IFamilyApi FamilyApi
        {
            get
            {
                if (_FamilyApi == null) { _FamilyApi = new FamilyApi(this); }
                return _FamilyApi;
            }

            set
            {
                _FamilyApi = value;
            }
        }

        public IPublicApi PublicApi
        {
            get
            {
                if (_PublicApi == null) { _PublicApi = new PublicApi(); }
                return _PublicApi;
            }

            set
            {
                _PublicApi = value;
            }
        }
    }
}

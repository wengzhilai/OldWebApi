using System.Web.Http;
using ProServer;
using ProInterface.Models;
using ProInterface;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using System.IO;
using System.Text;
using System.Web;

namespace WebApi.Controllers
{
    /// <summary>
    /// 公共方法
    /// </summary>
    [RoutePrefix("api/Public")]
    public class PublicController : ApiController
    {
        private ServeWeb api;
        /// <summary>
        /// 构造方法
        /// </summary>
        public PublicController()
        {
            api = new ServeWeb();
        }

        /// <summary>
        /// 获取阴历
        /// </summary>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        [Route("GetChineseCalendar")]
        [HttpPost]
        [HttpOptions]
        public async Task<dynamic> GetChineseCalendar(ApiRequesEntityBean<ErrorInfo> inEnt)
        {
            if (Request.Method.Method == "OPTIONS") return null;
            ErrorInfo err = new ErrorInfo();
            Fun<ErrorInfo>.Valid(inEnt, ModelState, ref err);
            if (err.IsError) return err;
            dynamic reEnt = await Task.Run(() => Fun<ErrorInfo>.Func(api.PublicApi.GetChineseCalendar, ref err, inEnt));
            if (err.IsError) return err;
            return reEnt;
        }


        /// 发送验证码到手机
        /// <para>发送时会在用户的Login表里修改VERIFY_CODE，并在fa_sms_send增加记录</para>
        [Route("SendCode")]
        [HttpPost]
        [HttpOptions]
        public async Task<dynamic> SendCode(ApiRequesEntityBean<ErrorInfo> inEnt)
        {
            if (Request.Method.Method == "OPTIONS") return null;
            ErrorInfo err = new ErrorInfo();
            Fun<ErrorInfo>.Valid(inEnt, ModelState, ref err);
            if (err.IsError) return err;
            dynamic reEnt = await Task.Run(() => Fun<ErrorInfo>.Func(api.PublicApi.SendCode, ref err, inEnt));
            if (err.IsError) return err;
            return reEnt;
        }

        /// <summary>
        /// 文件删除
        /// </summary>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        [Route("FileDel")]
        [HttpPost]
        [HttpOptions]
        public async Task<dynamic> FileDel(ApiRequesEntityBean<ErrorInfo> inEnt)
        {
            if (Request.Method.Method == "OPTIONS") return null;
            ErrorInfo err = new ErrorInfo();
            Fun<ErrorInfo>.Valid(inEnt, ModelState, ref err);
            if (err.IsError) return err;
            dynamic reEnt = await Task.Run(() => Fun<ErrorInfo>.Func(api.PublicApi.FileDel, ref err, inEnt));
            if (err.IsError) return err;
            return reEnt;
        }

        /// <summary>
        /// 获取文件列表
        /// </summary>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        [Route("FileList")]
        [HttpPost]
        [HttpOptions]
        [HttpGet]
        public async Task<dynamic> FileList(ApiRequesPageBean<ApiPagingDataBean<FILES>> inEnt)
        {
            if (Request.Method.Method == "OPTIONS") return null;
            ErrorInfo err = new ErrorInfo();
            Fun<ErrorInfo>.Valid(inEnt, ModelState, ref err);
            if (err.IsError) return err;
            dynamic reEnt = await Task.Run(() => Fun<ApiPagingDataBean<FILES>>.Func(api.PublicApi.FileList, ref err, inEnt));
            if (err.IsError) return err;
            return reEnt;
        }

        /// <summary>
        /// 文件删除
        /// </summary>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        [Route("FileUp")]
        [HttpPost]
        [HttpOptions]
        public async Task<dynamic> FileUp(ApiRequesPageBean<FILES> inEnt)
        {
            if (Request.Method.Method == "OPTIONS") return null;
            ErrorInfo err = new ErrorInfo();
            Fun<ErrorInfo>.Valid(inEnt, ModelState, ref err);
            if (err.IsError) return err;
            dynamic reEnt = await Task.Run(() => Fun<FILES>.Func(api.PublicApi.FileUp, ref err, inEnt));
            if (err.IsError) return err;
            return reEnt;
        }

        /// <summary>
        /// 检测登录Key是否存在
        /// </summary>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        [Route("CheckToken")]
        [HttpPost]
        [HttpOptions]
        public async Task<dynamic> CheckToken(ApiRequesPageBean<ErrorInfo> inEnt)
        {
            if (Request.Method.Method == "OPTIONS") return null;
            ErrorInfo err = new ErrorInfo();
            Fun<ErrorInfo>.Valid(inEnt, ModelState, ref err);
            if (err.IsError) return err;
            dynamic reEnt = await Task.Run(() => Fun<ErrorInfo>.Func(api.PublicApi.CheckToken, ref err, inEnt));
            if (err.IsError) return err;
            return reEnt;
        }

        /// <summary>
        /// 通过multipart/form-data方式上传文件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [HttpOptions]
        public async Task<dynamic> PostFile()
        {
            if (Request.Method.Method == "OPTIONS") return null;
            ErrorInfo err = new ErrorInfo();
            try
            {
                // 是否请求包含multipart/form-data。
                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }

                string root = HttpContext.Current.Server.MapPath("/UploadFiles/");
                if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/UploadFiles/")))
                {
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/UploadFiles/"));
                }

                var provider = new MultipartFormDataStreamProvider(root);

                StringBuilder sb = new StringBuilder(); // Holds the response body

                // 阅读表格数据并返回一个异步任务.
                await Request.Content.ReadAsMultipartAsync(provider);

                // 如何上传文件到文件名.
                foreach (var file in provider.FileData)
                {
                    string orfilename = file.Headers.ContentDisposition.FileName.TrimStart('"').TrimEnd('"');
                    FileInfo fileinfo = new FileInfo(file.LocalFileName);
                    //sb.Append(string.Format("Uploaded file: {0} ({1} bytes)\n", fileInfo.Name, fileInfo.Length));
                    //最大文件大小
                    //int maxSize = Convert.ToInt32(SettingConfig.MaxSize);
                    if (fileinfo.Length <= 0)
                    {
                        
                    }
                    else if (fileinfo.Length > AppSet.MaxFileSize)
                    {
                        err.IsError = true;
                        err.Message = "上传文件大小超过限制";
                    }
                    else
                    {
                        string fileExt = orfilename.Substring(orfilename.LastIndexOf('.'));
                        //定义允许上传的文件扩展名
                        //String fileTypes = SettingConfig.FileTypes;
                        //if (String.IsNullOrEmpty(fileExt) || Array.IndexOf(fileTypes.Split(','), fileExt.Substring(1).ToLower()) == -1)
                        //{
                        //    json.Msg = "图片类型不正确";
                        //    json.Code = 303;
                        //}
                        //else
                        //{
                        //String ymd = DateTime.Now.ToString("yyyyMMdd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
                        //String newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", System.Globalization.DateTimeFormatInfo.InvariantInfo);

                        fileinfo.CopyTo(Path.Combine(root, fileinfo.Name + fileExt), true);
                        err.IsError = false;
                        err.Message = "操作成功";
                        sb.Append("/UploadFiles/" + fileinfo.Name + fileExt);
                        //}
                    }
                    fileinfo.Delete();//删除原文件
                }
            }
            catch (System.Exception e)
            {
                err.IsError = true;
                err.Message = "服务器无响应";
            }
            return err;
        }


    }
}

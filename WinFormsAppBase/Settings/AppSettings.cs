namespace WinFormsAppBase.Settings
{
    /// <summary>
    /// 
    /// </summary>
    public class AppSettings
    {
        /// <summary>
        /// 程式名稱
        /// </summary>
        public string? AppName { get; set; }


        /// <summary>
        /// 本機路徑
        /// </summary>
        public string? LocalPath { get; set; }

        /// <summary>
        /// 下載遠端資料
        /// </summary>
        public string? DownloadRemotePath { get; set; }
        /// <summary>
        /// 下載IP、下載網域名稱
        /// </summary>
        public string? DownloadHostName { get; set; }
        /// <summary>
        /// 下載使用者名稱
        /// </summary>
        public string? DownloadUserName { get; set; }
        /// <summary>
        /// 下載密碼
        /// </summary>
        public string? DownloadPassword { get; set; }

        /// <summary>
        /// 上傳IP、上傳網域名稱
        /// </summary>
        public string? UploadHostName { get; set; }
        /// <summary>
        /// 上傳使用者名稱
        /// </summary>
        public string? UploadUserName { get; set; }
        /// <summary>
        /// 上傳密碼
        /// </summary>
        public string? UploadPassword { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public int LastTime { get; set; }


    }
}

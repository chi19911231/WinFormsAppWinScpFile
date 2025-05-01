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
        /// 本機檔案刪除(Y:是)
        /// </summary>
        public string? LocalFileDelete { get; set; } 

        /// <summary>
        /// 本機路徑
        /// </summary>
        public string? LocalPath { get; set; }

        /// <summary>
        /// 網路磁碟上傳功能是否啟用(Y:是)
        /// </summary>
        public string? NetworkDriveUploadEnable { get; set; }

        /// <summary>
        /// 網路磁碟上傳路徑
        /// </summary>
        public string? NetworkDriveUploadPath { get; set; }

        /// <summary>
        /// 網路磁碟下載功能是否啟用(Y:是)
        /// </summary>
        public string? NetworkDriveDownloadEnable { get; set; }

        /// <summary>
        /// 網路磁碟下載路徑
        /// </summary>
        public string? NetworkDriveDownloadPath { get; set; }


        /// <summary>
        /// Sftp上傳功能是否啟用(Y:是)
        /// </summary>
        public string? SftpFilesUploadEnable { get; set; }

        /// <summary>
        /// Sftp IP(上傳)、網域名稱(上傳)
        /// </summary>
        public string? SftpUploadHostName { get; set; }

        /// <summary>
        /// Sftp使用者名稱(上傳)
        /// </summary>
        public string? SftpUploadUserName { get; set; }

        /// <summary>
        /// Sftp使用者密碼(上傳)
        /// </summary>
        public string? SftpUploadPassword { get; set; }

        /// <summary>
        /// Sftp上傳路徑
        /// </summary>
        public string? SftpUploadRemotePath { get; set; }


        /// <summary>
        /// Sftp下載功能是否啟用(Y:是)
        /// </summary>
        public string? SftpFilesDownloadEnable { get; set; }

        /// <summary>
        /// Sftp IP(下載)、網域名稱(下載)
        /// </summary>
        public string? SftpDownloadHostName { get; set; }

        /// <summary>
        /// Sftp使用者名稱(下載)
        /// </summary>
        public string? SftpDownloadUserName { get; set; }

        /// <summary>
        /// Sftp使用者密碼(下載)
        /// </summary>
        public string? SftpDownloadPassword { get; set; }

        /// <summary>
        /// Sftp下載路徑
        /// </summary>
        public string? SftpDownloadRemotePath { get; set; }

        /// <summary>
        /// 最後時間
        /// </summary>
        public int LastTime { get; set; }

        /// <summary>
        /// 倒數
        /// </summary>
        public int CountDown { get; set; }

    }
}

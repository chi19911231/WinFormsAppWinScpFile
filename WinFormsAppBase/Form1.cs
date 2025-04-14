using WinSCP;

namespace WinFormsAppBase
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// 倒數
        /// </summary>
        private int countdown = 10;
        public Form1()
        {
            InitializeComponent();
        }    

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = Settings.AppConfig.Settings.AppName;

            labelState.Text = "計時開始，10秒後開始執行。";
            timer1.Interval = 1000;
            timer1.Start();
        }

        /// <summary>
        /// SFTP單檔案下載
        /// </summary>
        public void SftpFileDownload()
        {

            string? localPath = Settings.AppConfig.Settings.LocalPath;
            string? downloadRemotePath = Settings.AppConfig.Settings.DownloadRemotePath;

            // 第一步：從第一台主機下載檔案
            SessionOptions sessionOptionsDownload = new SessionOptions
            {
                Protocol = Protocol.Sftp,
                HostName = Settings.AppConfig.Settings.DownloadHostName,
                UserName = Settings.AppConfig.Settings.DownloadUserName,
                Password = Settings.AppConfig.Settings.DownloadPassword,
                //SshHostKeyFingerprint = "ssh-rsa 2048 AAAA..." // 實際填上                 
                GiveUpSecurityAndAcceptAnySshHostKey = true,
                //GiveUpSecurityAndAcceptAnyTlsHostCertificate=true,
            };

            string localTempFile = @$"{localPath}\group-3_instance-4_traffic_2025-04-14-27.log";
            string downloadFile = $"{downloadRemotePath}/group-3_instance-4_traffic_2025-04-14-27.log";
            using (Session sessionDownload = new Session())
            {
                sessionDownload.Open(sessionOptionsDownload);
                sessionDownload.GetFiles(downloadFile, localTempFile).Check();
                Console.WriteLine("已從第一台下載至本地");
            }   

        }

        /// <summary>
        /// SFTP多檔案下載
        /// </summary>
        public void SftpFilesDownload()
        {

            string? localPath = Settings.AppConfig.Settings.LocalPath;
            string? downloadRemotePath = Settings.AppConfig.Settings.DownloadRemotePath;

            // 第一步：從第一台主機下載檔案
            SessionOptions sessionOptionsDownload = new SessionOptions
            {
                Protocol = Protocol.Sftp,
                HostName = Settings.AppConfig.Settings.DownloadHostName,
                UserName = Settings.AppConfig.Settings.DownloadUserName,
                Password = Settings.AppConfig.Settings.DownloadPassword,
                //SshHostKeyFingerprint = "ssh-rsa 2048 AAAA..." // 實際填上                 
                GiveUpSecurityAndAcceptAnySshHostKey = true,
                //GiveUpSecurityAndAcceptAnyTlsHostCertificate=true,
            };

            try
            {
                using (Session sessionDownload = new Session())
                {                    
                    // 開啟連線
                    sessionDownload.Open(sessionOptionsDownload);
        
                    // 取得所有檔案清單
                    RemoteDirectoryInfo directory = sessionDownload.ListDirectory(downloadRemotePath);

                    // 計算時間：目前時間往回推 24 小時
                    DateTime lastTime = DateTime.UtcNow.AddHours(8).AddHours(Settings.AppConfig.Settings.LastTime);

                    foreach (RemoteFileInfo file in directory.Files)
                    {

                        // 排除目錄與舊檔
                        if (!file.IsDirectory && file.LastWriteTime.ToUniversalTime() >= lastTime)
                        {

                            labelState.Text = $"下載 : {file.Name.ToString()}";

                            // 下載到本地路徑
                            sessionDownload.GetFiles(@$"{downloadRemotePath}/{file.Name}", @$"{localPath}\{file.Name}").Check();

                        }

                    }

                    labelState.Text = "下載完成";

                }

                labelState.Text = "執行完成";

            }
            catch (Exception exception)
            {
                labelState.Text = exception.Message;
            }

        }

        /// <summary>
        /// SFTP單檔案上傳(未完成)
        /// </summary>
        public void SftpFileUpload()
        {
           

        }

        /// <summary>
        /// SFTP多檔案上傳(未完成)
        /// </summary>
        public void SftpFilesUpload()
        {


        }


        /// <summary>
        /// 本機單檔案刪除(未完成)
        /// </summary>
        public void LocalFileUpload()
        {

        }

        /// <summary>
        /// 本機多檔案刪除(未完成)
        /// </summary>
        public void LocalFilesDelete()
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            countdown--;

            if (countdown > 0)
            {
                labelState.Text = $"{ countdown.ToString() }秒後執行。";
            }
            else
            {
                timer1.Stop(); // 停止計時器（只執行一次）
                timer1.Tick -= timer1_Tick; // 移除事件避免記憶體問題
                timer1.Dispose();
                labelState.Text = "執行中";

                SftpFilesDownload();

                //關閉程式
                Application.Exit();

            }
        }
    }
}

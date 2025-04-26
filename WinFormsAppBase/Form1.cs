using WinSCP;

namespace WinFormsAppBase
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// 倒數
        /// </summary>
        private int countDown = Settings.AppConfig.Setting.CountDown;
        public Form1()
        {
            InitializeComponent();
        }    

        private void Form1_Load(object sender, EventArgs e)
        {

           
            this.Text = Settings.AppConfig.Setting.AppName;

            //labelState.Text = "10秒後開始執行。";
            //timer1.Interval = 1000;
            //timer1.Start();
        }

        /// <summary>
        /// SFTP單檔案下載
        /// </summary>
        public void SftpFileDownload()
        {
            string? localPath = Settings.AppConfig.Setting.LocalPath;
            string? downloadRemotePath = Settings.AppConfig.Setting.DownloadRemotePath;

            // 第一步：從第一台主機下載檔案
            SessionOptions sessionOptionsDownload = new SessionOptions
            {
                Protocol = Protocol.Sftp,
                HostName = Settings.AppConfig.Setting.DownloadHostName,
                UserName = Settings.AppConfig.Setting.DownloadUserName,
                Password = Settings.AppConfig.Setting.DownloadPassword,
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

            string? localPath = Settings.AppConfig.Setting.LocalPath;
            string? downloadRemotePath = Settings.AppConfig.Setting.DownloadRemotePath;

            // 第一步：從第一台主機下載檔案
            SessionOptions sessionOptionsDownload = new SessionOptions
            {
                Protocol = Protocol.Sftp,
                HostName = Settings.AppConfig.Setting.DownloadHostName,
                UserName = Settings.AppConfig.Setting.DownloadUserName,
                Password = Settings.AppConfig.Setting.DownloadPassword,
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
                    DateTime lastTime = DateTime.UtcNow.AddHours(8).AddHours(Settings.AppConfig.Setting.LastTime);

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

            string folderPath = @"C:\路徑\資料夾";

            if (Directory.Exists(folderPath))
            {
                string[] files = Directory.GetFiles(folderPath);
                foreach (string file in files)
                {
                    File.Delete(file);
                    Console.WriteLine($"已刪除：{file}");
                }
            }
            else
            {
                Console.WriteLine("找不到資料夾");
            }
        }

        /// <summary>
        /// 本機多檔案刪除
        /// </summary>
        public void LocalFilesDelete()
        {
            
            string? localPath = Settings.AppConfig.Setting.LocalPath;

            if (Directory.Exists(localPath))
            {
                string[] files = Directory.GetFiles(localPath);
                foreach (string file in files)
                {
                    File.Delete(file);
                    labelState.Text = $"檔案已刪除{file}";
                }
            }
            else
            {
                labelState.Text = $"找不到資料夾";
            }

            labelState.Text = $"暫存多檔案刪除執行完成。";

        }

        /// <summary>
        /// 計時器設定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void timer1_Tick(object sender, EventArgs e)
        {
            countDown--;

            if (countDown > 0)
            {
                labelState.Text = $"{ countDown.ToString() }秒後執行。";
            }
            else
            {
                timer1.Stop(); // 停止計時器（只執行一次）
                timer1.Tick -= timer1_Tick; // 移除事件避免記憶體問題
                timer1.Dispose();
                labelState.Text = "執行中";

         
                await Task.Run(() =>
                {
                    SftpFilesDownload();

                    if (Settings.AppConfig.Setting.LocalFileStore == "N") 
                    {
                        LocalFilesDelete();
                    }

                });

                //關閉程式
                Application.Exit();

            }
        }
    }
}

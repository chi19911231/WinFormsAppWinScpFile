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
            //SftpFilesUpload();
            //NetworkDriveUpload();
            
            this.Text = Settings.AppConfig.Setting.AppName;

            labelState.Text = $"{countDown.ToString()}秒後執行。";
            timer1.Interval = 1000;
            timer1.Start();
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
                labelState.Text = $"{countDown.ToString()}秒後執行。";
            }
            else
            {
                timer1.Stop(); // 停止計時器（只執行一次）
                timer1.Tick -= timer1_Tick; // 移除事件避免記憶體問題
                timer1.Dispose();
                labelState.Text = "執行中";


                await Task.Run(() =>
                {


                    if (Settings.AppConfig.Setting.SftpFilesUploadEnable == "Y")
                    {
                        
                    }
                    if (Settings.AppConfig.Setting.SftpFilesDownloadEnable == "Y")
                    {
                        SftpFilesDownload();
                    }


                    if (Settings.AppConfig.Setting.NetworkDriveUploadEnable == "Y")
                    {
                        NetworkDriveUpload();
                    }
                    if (Settings.AppConfig.Setting.NetworkDriveDownloadEnable == "Y")
                    {
                       
                    }


                    if (Settings.AppConfig.Setting.LocalFileDelete == "Y")
                    {
                        LocalFilesDelete();
                    }




                });

                //關閉程式
                Application.Exit();

            }
        }


        /// <summary>
        /// SFTP單檔案下載
        /// </summary>
        public void SftpFileDownload()
        {
            string? localPath = Settings.AppConfig.Setting.LocalPath;
            string? downloadRemotePath = Settings.AppConfig.Setting.SftpDownloadRemotePath;

            // 第一步：從第一台主機下載檔案
            SessionOptions sessionOptionsDownload = new SessionOptions
            {
                Protocol = Protocol.Sftp,
                HostName = Settings.AppConfig.Setting.SftpDownloadHostName,
                UserName = Settings.AppConfig.Setting.SftpDownloadUserName,
                Password = Settings.AppConfig.Setting.SftpDownloadPassword,
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
            string? downloadRemotePath = Settings.AppConfig.Setting.SftpDownloadRemotePath;

            // 第一步：從第一台主機下載檔案
            SessionOptions sessionOptionsDownload = new SessionOptions
            {
                Protocol = Protocol.Sftp,
                HostName = Settings.AppConfig.Setting.SftpDownloadHostName,
                UserName = Settings.AppConfig.Setting.SftpDownloadUserName,
                Password = Settings.AppConfig.Setting.SftpDownloadPassword,
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

                    // 計算時間：目前時間往回推 N 小時
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
            MessageBox.Show("SftpFilesUpload" );
            try
            {
                string? localPath = Settings.AppConfig.Setting.LocalPath;
                string? localFile = @$"{localPath}";

                string? uploadRemotePath = Settings.AppConfig.Setting.SftpUploadRemotePath;

                MessageBox.Show("uploadRemotePath:"+ uploadRemotePath);
                // 建立 session 選項
                SessionOptions sessionOptions = new SessionOptions
                {
                    Protocol = Protocol.Sftp,
                    HostName = Settings.AppConfig.Setting.SftpUploadHostName,
                    UserName = Settings.AppConfig.Setting.SftpDownloadUserName,
                    Password = Settings.AppConfig.Setting.SftpUploadPassword,
                    //SshHostKeyFingerprint = "ssh-rsa 2048 AAAA..." // 實際填上                 
                    GiveUpSecurityAndAcceptAnySshHostKey = true,
                    //GiveUpSecurityAndAcceptAnyTlsHostCertificate=true,
                };

                using (Session session = new Session())
                {
                    // 連線
                    session.Open(sessionOptions);

                    // 要上傳的檔案資訊
                    TransferOptions transferOptions = new TransferOptions();
                    transferOptions.TransferMode = TransferMode.Binary;
                    transferOptions.OverwriteMode = OverwriteMode.Overwrite;

                    MessageBox.Show("1");


                    // 開始上傳
                    TransferOperationResult transferResult;
                    //transferResult = session.PutFiles(@$"{localPath}/Test.txt", @$"{uploadRemotePath}/", false, transferOptions);

                    // 先刪除遠端檔案
                    //session.RemoveFiles(@$"{uploadRemotePath}/Test.txt");
                    //MessageBox.Show(@$"{uploadRemotePath}/Test.txt");

                    // 再上傳檔案
                    transferResult = session.PutFiles(@$"{localPath}\Test.txt", $"{uploadRemotePath}", false, transferOptions);


                    MessageBox.Show(@$"{localPath}/Test.txt");


                    MessageBox.Show(@$"{uploadRemotePath}/");


                    // 檢查結果
                    transferResult.Check();
                    MessageBox.Show("3");
                    Console.WriteLine("檔案上傳成功！");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("錯誤訊息： " + e.Message);
                Console.WriteLine("錯誤訊息： " + e.Message);
            }
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
        /// 網路磁碟上傳
        /// </summary>
        public void NetworkDriveUpload()
        {
            // 本機資料夾
            string localFolderPath = $@"{Settings.AppConfig.Setting.LocalPath}";

            //網路磁碟資料夾
            string networkFolderPath = $@"{Settings.AppConfig.Setting.NetworkDriveUploadPath}";
                      
            // 取得本機資料夾內所有檔案（不含子資料夾）
            string[] files = Directory.GetFiles(localFolderPath);

            foreach (var file in files)
            {
                // 複製檔案（true：如果有同名就覆蓋）
                File.Copy(file,  Path.Combine(networkFolderPath, Path.GetFileName(file) ) , true);
                labelState.Text = $"檔案上傳:{Path.GetFileName(file)}";
            }
            
            labelState.Text = $"檔案上傳成功";

        }

        /// <summary>
        /// 網路磁碟下載
        /// </summary>
        public void NetworkDriveDownload()
        {   

            // 假設你的網路磁碟是 Z:\ 資料夾
            string networkPath = @"Z:\SomeFolder\SomeFile.txt";

            // 讀取文字檔案內容
            string content = File.ReadAllText(networkPath);

            // 如果是列出資料夾內的檔案
            string[] files = Directory.GetFiles(@"Z:\SomeFolder");

            foreach (var file in files)
            {
                Console.WriteLine(file);
            }
        }

      
    }
}

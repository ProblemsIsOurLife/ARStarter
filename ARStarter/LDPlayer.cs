using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Emgu;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.Util;
namespace Auto_LDPlayer
{
    public class LDPlayer
    {
        

        public static string PathLd = @"C:\LDPlayer\LDPlayer4.0\ldconsole.exe";
        public static string _PathLd
        {
            set
            {
                PathLd = value;
            }
        }

        public static void Open( string name)
        {
            ExecuteLd($"launch --name {name}");
        }

        public static void Open_App( string name, string packageName)
        {
            ExecuteLd($"launchex --name {name} --packagename {packageName}");
        }

        public static void Close( string name)
        {
            ExecuteLd($"quit --name {name}");
        }

        public static void CloseAll()
        {
            ExecuteLd("quitall");
        }

        public static void ReBoot( string name)
        {
            ExecuteLd($"reboot --name {name}");
        }

        //Nhóm 2 - Tuỳ Chỉnh Thêm

        public static void Create(string name)
        {
            ExecuteLd("add --name " + name);
        }

        public static void Copy(string name, string fromname)
        {
            ExecuteLd($"copy --name {name} --from {fromname}");
        }

        public static void Delete( string name)
        {
            ExecuteLd($"remove --name {name}");
        }

        public static void ReName( string name, string titleNew)
        {
            ExecuteLd($"rename --name {name} --title {titleNew}");
        }

        //Nhóm 3 - Change Setting

        public static void InstallApp_File( string name, string fileName)
        {
            ExecuteLd($@"installapp --name {name} --filename ""{fileName}""");
        }

        public static void InstallApp_Package( string name, string packageName)
        {
            ExecuteLd($"installapp --name {name} --packagename {packageName}");
        }

        public static void UnInstallApp( string name, string packageName)
        {
            ExecuteLd($"uninstallapp --name {name} --packagename {packageName}");
        }

        public static void RunApp( string name, string packageName)
        {
            ExecuteLd($"runapp --name {name} --packagename {packageName}");
        }

        public static void KillApp( string name, string packageName)
        {
            ExecuteLd($"killapp --name {name} --packagename {packageName}");
        }

        public static void Locate( string name, string lng, string lat)
        {
            ExecuteLd($"locate --name {name} --LLI {lng},{lat}");
        }

        public static void Change_Property( string name, string cmd)
        {
            ExecuteLd($"modify --name {name} {cmd}");
            //[--resolution ]
            //[--cpu < 1 | 2 | 3 | 4 >]
            //[--memory < 512 | 1024 | 2048 | 4096 | 8192 >]
            //[--manufacturer asus]
            //[--model ASUS_Z00DUO]
            //[--pnumber 13812345678]
            //[--imei ]
            //[--imsi ]    
            //[--simserial ]
            //[--androidid ]
            //[--mac ]
            //[--autorotate < 1 | 0 >]
            //[--lockwindow < 1 | 0 >]
        }

        public static void SetProp( string name, string key, string value)
        {
            ExecuteLd($"setprop --name {name} --key {key} --value {value}");
        }

        public static string GetProp( string name, string key)
        {
            return ExecuteLdForResult($"getprop --name {name} --key {key}");
        }

        public static string Adb( string name, string cmd, int timeout = 10000, int retry = 1)
        {
            return ExecuteLdForResult($"adb --name \"{name}\" --command \"{cmd}\"", timeout,
                retry);
        }

        public static void DownCpu( string name, string rate)
        {
            ExecuteLd($"downcpu --name {name} --rate {rate}");
        }

        public static void Backup( string name, string filePath)
        {
            ExecuteLd($@"backup --name {name} --file ""{filePath}""");
        }

        public static void Restore( string name, string filePath)
        {
            ExecuteLd($@"restore --name {name} --file ""{filePath}""");
        }

        public static void Action( string name, string key, string value)
        {
            ExecuteLd($"action --name {name} --key {key} --value {value}");
        }

        public static void Scan( string name, string filePath)
        {
            ExecuteLd($"scan --name {name} --file {filePath}");
        }

        public static void SortWnd()
        {
            ExecuteLd("sortWnd");
        }

        public static void ZoomIn( string name)
        {
            ExecuteLd($"zoomIn --name {name}");
        }

        public static void ZoomOut( string name)
        {
            ExecuteLd($"zoomOut --name {name}");
        }

        public static void Pull( string name, string remoteFilePath, string localFilePath)
        {
            ExecuteLd($@"pull --name {name} --remote ""{remoteFilePath}"" --local ""{localFilePath}""");
        }

        public static void Push( string name, string remoteFilePath, string localFilePath)
        {
            ExecuteLd($@"push --name {name} --remote ""{remoteFilePath}"" --local ""{localFilePath}""");
        }

        public static void BackupApp( string name, string packageName, string filePath)
        {
            ExecuteLd($@"backupapp --name {name} --packagename {packageName} --file ""{filePath}""");
        }

        public static void RestoreApp( string name, string packageName, string filePath)
        {
            ExecuteLd($@"restoreapp --name {name} --packagename {packageName} --file ""{filePath}""");
        }

        public static void GlobalConfig( string name, string fps, string audio, string fastPlay,
            string cleanMode)
        {
            //  [--fps <0~60>] [--audio <1 | 0>] [--fastplay <1 | 0>] [--cleanmode <1 | 0>]
            ExecuteLd(
                $"globalsetting --name {name} --audio {audio} --fastplay {fastPlay} --cleanmode {cleanMode}");
        }

        public static List<string> GetDevices()
        {
            var arr = ExecuteLdForResult("list",0).Trim().Split('\n');
            for (var i = 0; i < arr.Length; i++)
            {
                if (arr[i] == "")
                    return new List<string>();
                arr[i] = arr[i].Trim();
            }

            //System.Windows.Forms.MessageBox.Show(string.Join("|", arr));
            return arr.ToList();
        }

        public static bool isWorking(string name)
        {/*
            Adb(name, $"wait-for-device");*/
            int counter = 0;
            try
            {
                if (IsDeviceRunning(name))
                {
                    if (Adb(name, $"shell getprop sys.boot_completed").Trim() != "1")
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
           
        }
        public static List<string> GetDevicesRunning()
        {
            var arr = ExecuteLdForResult("runninglist").Trim().Split('\n');
            for (var i = 0; i < arr.Length; i++)
            {
                if (arr[i] == "")
                    return new List<string>();
                arr[i] = arr[i].Trim();
            }

            //System.Windows.Forms.MessageBox.Show(string.Join("|", arr));
            return arr.ToList();
        }

        public static bool IsDeviceRunning( string name)
        {
            var result = ExecuteLdForResult($"isrunning --name {name}").Trim();
            return result == "running";
        }

        

        public static string[] GetDevices2Running()
        {
            try
            {
                string[] listLdPlayer = null;
                var deviceRunning = GetDevicesRunning();
                listLdPlayer = ExecuteLdForResult("list2").Trim().Split('\n');
                

                return listLdPlayer;
            }
            catch
            {
                return null;
            }
            //System.Windows.Forms.MessageBox.Show(string.Join("\n", arr));
        }

        public static void ExecuteLd(string cmd)
        {
            var p = new Process();
            p.StartInfo.FileName = PathLd;
            p.StartInfo.Arguments = cmd;
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = true;
            p.EnableRaisingEvents = true;
            p.Start();
            p.WaitForExit();
            p.Close();
        }

        public static string ExecuteLdForResult(string cmdCommand, int timeout = 1000, int retry = 2)
        {
            string result;
            int retr = retry;
            var process = new Process();
            try
            {
               /* foreach (var processes in Process.GetProcessesByName("ldconsole"))
                {
                    processes.Kill();
                }*/
                
                process.StartInfo = new ProcessStartInfo
                {
                    FileName = PathLd,
                    Arguments = cmdCommand,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    RedirectStandardOutput = true,
                };
                
                process.Start();
                 while (retry >= 0 && retr == 1)
                 {
                     retry--;
                     process.Start();
                     if (!process.WaitForExit(timeout))
                     {
                         process.Kill();
                     }
                     else
                     {
                         break;
                     }
                 }
                var text = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                result = text;

            }
            catch
            {
                result = null;
            }
            try
            {
                if (!process.HasExited)
                {
                    process.Kill();
                }
                
            }
            catch
            {

            }
            return result;
        }

        public static Point GetScreenResolution( string name)
        {
            var str1 = Adb( name, "shell dumpsys display | grep \"mCurrentDisplayRect\"");
            var str2 = str1.Substring(str1.IndexOf("- ", StringComparison.Ordinal));
            var strArray = str2.Substring(str2.IndexOf(' '), str2.IndexOf(')') - str2.IndexOf(' ')).Split(',');
            return new Point(Convert.ToInt32(strArray[0].Trim()), Convert.ToInt32(strArray[1].Trim()));
        }

        public static void TapByPercent( string name, double x, double y, int count = 1)
        {
            var screenResolution = GetScreenResolution( name);
            var num1 = (int)(x * (screenResolution.X * 1.0 / 100.0));
            var num2 = (int)(y * (screenResolution.Y * 1.0 / 100.0));
            Tap( name, num1, num2, count);
        }

        public static void Tap( string name, int x, int y, int count = 1)
        {
            var cmdCommand = $"shell input tap {x} {y}";
            for (var index = 1; index < count; ++index)
                cmdCommand += (" && " + cmdCommand);
            Adb( name, cmdCommand, 2);
        }

        public static void PressKey( string name, LDKeyEvent key)
        {
            Adb( name, $"shell input keyevent {key}", 200);
        }

        public static void SwipeByPercent( string name, double x1, double y1, double x2, double y2,
            int duration = 100)
        {
            var screenResolution = GetScreenResolution( name);
            var num1 = (int)(x1 * (screenResolution.X * 1.0 / 100.0));
            var num2 = (int)(y1 * (screenResolution.Y * 1.0 / 100.0));
            var num3 = (int)(x2 * (screenResolution.X * 1.0 / 100.0));
            var num4 = (int)(y2 * (screenResolution.Y * 1.0 / 100.0));
            Swipe( name, num1, num2, num3, num4, duration);
        }

        public static void Swipe( string name, int x1, int y1, int x2, int y2, int duration = 100)
        {
            Adb( name, $"shell input swipe {x1} {y1} {x2} {y2} {duration}", 200);
        }


        public static void InputText( string name, string text)
        {
            Adb( name,
                $"shell input text \"{text.Replace(" ", "%s").Replace("&", "\\&").Replace("<", "\\<").Replace(">", "\\>").Replace("?", "\\?").Replace(":", "\\:").Replace("{", "\\{").Replace("}", "\\}").Replace("[", "\\[").Replace("]", "\\]").Replace("|", "\\|")}\""
            );
        }

        public static void LongPress( string name, int x, int y, int duration = 100)
        {
            Swipe( name, x, y, x, y, duration);
        }

        public static Bitmap ScreenShoot( string name, bool isDeleteImageAfterCapture = true,
            string fileName = "screenShoot.png")
        {
            var str1 = "_" + name;


            var path = Path.GetFileNameWithoutExtension(fileName) + str1 + Path.GetExtension(fileName);
            if (File.Exists(path))
                try
                {
                    File.Delete(path);
                }
                catch (Exception)
                {
                    // ignored
                }

            var filename = Directory.GetCurrentDirectory() + "\\" + path;
            var str2 = $"\"{Directory.GetCurrentDirectory().Replace("\\\\", "\\")}\"";
            var cmdCommand1 = $"shell screencap -p \"/sdcard/{path}\"";
            var cmdCommand2 = $"pull /sdcard/{path} {str2}";
            Adb(name, cmdCommand1);
            Adb(name, cmdCommand2);
            Bitmap bitmap = null;
            try
            {
                using (var original = new Bitmap(filename))
                {
                    bitmap = new Bitmap(original);
                }
            }
            catch
            {
                // ignored
            }

            if (!isDeleteImageAfterCapture) return bitmap;
            try
            {
                File.Delete(path);
            }
            catch
            {
                // ignored
            }

            try
            {
                Adb(name, $"shell \"rm /sdcard/{path}\"");
            }
            catch
            {
                // ignored
            }

            return bitmap;
        
        }

        public static void PlanModeOn( string name, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return;
            Adb( name, " settings put global airplane_mode_on 1");
            Adb( name, "am broadcast -a android.intent.action.AIRPLANE_MODE");
        }

        public static void PlanModeOff( string name, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return;
            Adb( name, " settings put global airplane_mode_on 1");
            Adb( name, "am broadcast -a android.intent.action.AIRPLANE_MODE");
        }

        public static void Delay(double delayTime)
        {
            for (var num = 0.0; num < delayTime; num += 100.0)
                Thread.Sleep(TimeSpan.FromMilliseconds(100.0));
        }

        public static Point? FindImage( string name, string imagePath, int count = 5)
        {
            do
            {
                Bitmap mainBitmap = null;
                var num = 3;
                do
                {
                    try
                    {
                        mainBitmap = ScreenShoot( name);
                        break;
                    }
                    catch (Exception)
                    {
                        --num;
                        Delay(1000.0);
                    }
                } while (num > 0);

                if (mainBitmap == null)
                    return new Point?();
                var image = new Point?();
                Image<Bgr, byte> imgToFind = new Image<Bgr, byte>(imagePath);
                image = FindCountour(mainBitmap.ToImage<Bgr, byte>(), imgToFind);
                imgToFind.Dispose();
                mainBitmap.Dispose();
                if (image.HasValue)
                    return image;
                Delay(2000.0);
                --count;
            } while (count > 0);

            return new Point?();
        }






        public static Point? FindCountour(Image<Bgr, byte> image, Image<Bgr, byte> imageToFind)
        {
            int dsres = 0;
            
            
            Image<Gray, float> result = image.MatchTemplate(imageToFind, Emgu.CV.CvEnum.TemplateMatchingType.CcoeffNormed);
            float[,,] matches = result.Data;
            int x = 0, y = 0, kol = 0;
            Point[] Center = new Point[10];
            Rectangle rect = Rectangle.Empty;
            for (y = 0; y < matches.GetLength(0); y++)
            {
                for (x = 0; x < matches.GetLength(1); x++)
                {
                    double matchScore = matches[y, x, 0];
                    if (matchScore > 0.65)
                    {
                        rect = new Rectangle(x, y, imageToFind.Width, imageToFind.Height);
                        Center[kol] = new Point(x + imageToFind.Width / 2, y + imageToFind.Height / 2);
                        break;
                    }
                }
            }
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            result.Dispose();
            image.Dispose();
            imageToFind.Dispose();
            if (rect != Rectangle.Empty)
            {

                return new Point(rnd.Next(rect.X, rect.X + rect.Width), rnd.Next(rect.Y, rect.Y+rect.Height));
            }
            else
            {
                return null;
            }
        }




        public static Point? FindImageInSubSpace(string name, string imagePath, Point point1, Point point2, int count = 5)
        {
            do
            {
                Rectangle rect = new Rectangle(point1.X, point1.Y, point2.X - point1.X, point2.Y - point1.Y);
                Bitmap mainBitmap = null;
                var num = 3;
                do
                {
                    try
                    {
                        mainBitmap = ScreenShoot(name);
                        break;
                    }
                    catch (Exception)
                    {
                        --num;
                        Delay(1000.0);
                    }
                } while (num > 0);

                if (mainBitmap == null)
                    return new Point?();
                var image = new Point?();
                Image<Bgr,byte> bitmap = mainBitmap.ToImage<Bgr, byte>().Copy(rect);
                mainBitmap.Dispose();
                Image<Bgr, byte> imgToFind = new Image<Bgr, byte>(imagePath);
                image = FindCountour(bitmap, imgToFind);
                imgToFind.Dispose();
                mainBitmap.Dispose();
                bitmap.Dispose();
                if (image.HasValue)
                    return image;
                Delay(2000.0);
                --count;
            } while (count > 0);

            return new Point?();
        }



        












        public static bool FindImageAndClick( string name, string imagePath, int count = 5)
        {
            var point = FindImage( name, imagePath, count);
            if (!point.HasValue) return false;
            Tap( name, point.Value.X, point.Value.Y);
            return true;
        }


        // Điều Hướng
        public static void Back( string name)
        {
            PressKey( name, LDKeyEvent.KEYCODE_BACK);
        }

        public static void Home( string name)
        {
            PressKey( name, LDKeyEvent.KEYCODE_HOME);
        }

        public static void Menu( string name)
        {
            PressKey( name, LDKeyEvent.KEYCODE_APP_SWITCH);
        }


    }
}
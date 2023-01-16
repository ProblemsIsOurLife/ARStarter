using System;
using Auto_LDPlayer;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
//AutoEnterPoker
namespace ARStarter
{
    class EmulatorController 
    {
        public static bool updateState(Form1 form, int index, string status)
        {
            if (checkPauseStop(form,index))
            {
                return true;
            }
            form.LDDevicesList.Invoke((MethodInvoker)(() => form.LDDevicesList.Items[index].SubItems[1].Text = status));
            return false;
        }
        public static bool updateStatus(Form1 form, int index, string status)
        {
            if (checkPauseStop(form,index))
            {
                return true;
            }
            form.LDDevicesList.Invoke((MethodInvoker)(() => form.LDDevicesList.Items[index].SubItems[2].Text = status));
            return false;
        }
        public static bool updateStateExceptPauseAndStop(Form1 form, int index, string status)
        {
            
            form.LDDevicesList.Invoke((MethodInvoker)(() => form.LDDevicesList.Items[index].SubItems[1].Text = status));
            return false;
        }
        public static bool updateStatusExceptPauseAndStop(Form1 form, int index, string status)
        {
            
            form.LDDevicesList.Invoke((MethodInvoker)(() => form.LDDevicesList.Items[index].SubItems[2].Text = status));
            return false;
        }
        public static bool checkPauseStop(Form1 form, int index)
        {
            if (form.Stop)
            {
                form.Stop = false;
                return true;
            }
            else
            {
                if (form.Pause)
                {
                    form.LDDevicesList.Invoke((MethodInvoker)(() => form.LDDevicesList.Items[index].SubItems[2].Text = "На паузе"));
                    while (form.Pause)
                    {
                        if (form.Stop)
                        {
                            form.Stop = false;
                            return true;
                        }
                        Task.Delay(1000).Wait();
                    }
                    form.LDDevicesList.Invoke((MethodInvoker)(() => form.LDDevicesList.Items[index].SubItems[2].Text = "Выполняется"));
                }
            }
            
            
            
            return false;
        }
        public static void workingActions(Form1 form, int index, string emulatorName, CancellationToken ct) 
        {
            
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            bool result = false;
            Point? point = null;
            if(updateStatus(form,index,"Выполняется"))
            {
                updateStateExceptPauseAndStop(form, index, "");
                updateStatusExceptPauseAndStop(form, index, "Остановлен");
                return;
            }
            
            if(updateState(form, index, "В работе"))
            {
                updateStateExceptPauseAndStop(form, index, "");
                updateStatusExceptPauseAndStop(form, index, "Остановлен");
                return;
            }
            if(updateState(form, index, "Запускаю эмулятор"))
            {
                updateStateExceptPauseAndStop(form, index, "");
                updateStatusExceptPauseAndStop(form, index, "Остановлен");
                return;
            }
            Task.Delay(1000).Wait();
            LDPlayer.Open(emulatorName);

            while (LDPlayer.IsDeviceRunning(emulatorName) != true)
            {
                Task.Delay(1000).Wait();
            }
            Process[] processes = Process.GetProcessesByName("dnplayer");
            foreach (Process proc in processes)
            {
                if (proc.MainWindowTitle == emulatorName)
                {
                    MoveWindow(proc.MainWindowHandle, 0, 0, 452, 764, true);

                }
            }
            if (updateState(form, index, "Жду запуск эмулятора"))
            {
                updateStateExceptPauseAndStop(form, index, "");
                updateStatusExceptPauseAndStop(form, index, "Остановлен");
                return;
            }
            int countt = 0;
            while (!LDPlayer.isWorking(emulatorName))
            {
                countt++;
                Task.Delay(1000).Wait();
                if(countt > 60)
                {
                    updateStateExceptPauseAndStop(form, index, "Эмулятор не запустился");
                    updateStatusExceptPauseAndStop(form, index, "Остановлен");
                    return;
                }
                if (updateState(form, index, "Жду запуск эмулятора"))
                {
                    updateStateExceptPauseAndStop(form, index, "");
                    updateStatusExceptPauseAndStop(form, index, "Остановлен");
                    return;
                }
            }
            if (updateState(form, index, "Эмулятор запущен"))
            {
                updateStateExceptPauseAndStop(form, index, "");
                updateStatusExceptPauseAndStop(form, index, "Остановлен");
                return;
            }
            Task.Delay(2000).Wait();
            if(updateState(form, index, "Ищу иконку SocksDroid"))
            {
                updateStateExceptPauseAndStop(form, index, "");
                updateStatusExceptPauseAndStop(form, index, "Остановлен");
                return;
            }
            result = false;
            //result = LDPlayer.FindImageAndClick(emulatorName, PicturesPaths.SocksDroid, 1);
            if (!result)
            {
                while (!result)
                {

                    if(updateState(form, index, "Ищу иконку SocksDroid"))
                    {
                        updateStateExceptPauseAndStop(form, index, "");
                        updateStatusExceptPauseAndStop(form, index, "Остановлен");
                        return;
                    }
                    Task.Delay(1000).Wait();
                    result = LDPlayer.FindImageAndClick(emulatorName, PicturesPaths.SocksDroid, 1);
                    Task.Delay(1000).Wait();
                    if (result)
                    {
                        break;
                    }
                }
            }
            result = false;
            if(updateState(form, index, "Нажал на SocksDroid"))
            {
                updateStateExceptPauseAndStop(form, index, "");
                updateStatusExceptPauseAndStop(form, index, "Остановлен");
                return;
            }
            Task.Delay(1000).Wait();
            point = null;
            while (point == null)
            {
                try
                {
                    point = LDPlayer.FindImage(emulatorName, PicturesPaths.SocksDroidOpened, 1);
                }
                catch (System.ArgumentException e)
                {
                    point = null;
                }
                if (point != null)
                {
                    if(updateState(form, index, "Активирую SocksDroid"))
                    {
                        {
                            updateStateExceptPauseAndStop(form, index, "");
                            updateStatusExceptPauseAndStop(form, index, "Остановлен");
                            return;
                        }
                    }
                    break;
                }
                else
                {
                    if(updateState(form, index, "Жду запуск SocksDroid"))
                    {
                        updateStateExceptPauseAndStop(form, index, "");
                        updateStatusExceptPauseAndStop(form, index, "Остановлен");
                        return;
                    }
                }
                Task.Delay(1000).Wait();
            }
            result = false;

            Task.Delay(1000).Wait();
            result = LDPlayer.FindImageAndClick(emulatorName, PicturesPaths.SocksDroidEnable, 1);
            if (!result)
            {
                while (!result)
                {

                    if(updateState(form, index, "Не вижу ползунок активации SocksDroid"))
                    {
                        updateStateExceptPauseAndStop(form, index, "");
                        updateStatusExceptPauseAndStop(form, index, "Остановлен");
                        return;
                    }
                    Task.Delay(1000).Wait();
                    result = LDPlayer.FindImageAndClick(emulatorName, PicturesPaths.SocksDroidEnable, 1);
                }
            }

            if(updateState(form, index, "Сделал активацию SocksDroid"))
            {

                updateStateExceptPauseAndStop(form, index, "");
                updateStatusExceptPauseAndStop(form, index, "Остановлен");
                 return;
                
            }


            Task.Delay(1000).Wait();
            point = null;
            int count = 0;
            while (point == null)
            {
                if(count > 30)
                {
                    updateStateExceptPauseAndStop(form, index, "Socksdroid оборвал подключение");
                    updateStatusExceptPauseAndStop(form, index, "Остановлен");
                    return;
                }
                try
                {
                    point = LDPlayer.FindImage(emulatorName, PicturesPaths.SocksDroidEnabled, 1);
                }
                catch (System.ArgumentException e)
                {
                    point = null;
                }
                if (point != null)
                {
                    if(updateState(form, index, "SocksDroid включился"))
                    {
                        updateStateExceptPauseAndStop(form, index, "");
                        updateStatusExceptPauseAndStop(form, index, "Остановлен");
                        return;
                    }
                    break;
                }
                else
                {
                    if(updateState(form, index, "Жду активацию SocksDroid"))
                    {
                        updateStateExceptPauseAndStop(form, index, "");
                        updateStatusExceptPauseAndStop(form, index, "Остановлен");
                        return;
                    }
                }
                Task.Delay(1000).Wait();
                count++;
            }
            Task.Delay(1000).Wait();
            if(updateState(form, index, "Выхожу на главный экран"))
            {
                updateStateExceptPauseAndStop(form, index, "");
                updateStatusExceptPauseAndStop(form, index, "Остановлен");
                return;
            }
            LDPlayer.Home(emulatorName);
            result = false;

            Task.Delay(1000).Wait();
            if(updateState(form, index, "Запускаю покер"))
            {
                updateStateExceptPauseAndStop(form, index, "");
                updateStatusExceptPauseAndStop(form, index, "Остановлен");
                return;
            }
            LDPlayer.Open_App(emulatorName, "com.lein.pppoker.android");

            Task.Delay(1000).Wait();

            if(updateState(form, index, "Жду включение игры и закрываю левые окна"))
            {
                updateStateExceptPauseAndStop(form, index, "");
                updateStatusExceptPauseAndStop(form, index, "Остановлен");
                return;
            }
            point = null;
            int counter = 0;
            result = false;
            count = rnd.Next(5,15);
            /*if (!result)
            {
                while (!result)
                {
                    counter++;
                    if(counter > count)
                    {
                        break;
                    }
                    if (updateState(form, index, "Нажимаю закрыть все левые окна"))
                    {
                        updateStateExceptPauseAndStop(form, index, "");
                        updateStatusExceptPauseAndStop(form, index, "Остановлен");
                        return;
                    }
                    Task.Delay(rnd.Next(500, 1000)).Wait();
                    ///////////////////////
                    Point p3 = new Point(98, 641);
                    Point p4 = new Point(440, 703);
                    point = LDPlayer.FindImageInSubSpace(emulatorName, PicturesPaths.EnterToPokerButton, p3, p4, 1);
                    if (point != null)
                    {
                        updateStateExceptPauseAndStop(form, index, "Аккаунт не залогинен");
                        updateStatusExceptPauseAndStop(form, index, "Завершено");
                        return;
                    }
                    ///////////////////////
                   *//* if (counter % 2 == 0)
                    {
                        //если сломается фиксить тут вход
                        Point p1 = new Point(432,880);
                        Point p2 = new Point(540, 960);
                        point = LDPlayer.FindImageInSubSpace(emulatorName, PicturesPaths.PokerOpened,p1,p2, 1);
                        break;
                    }
                    else
                    {
                        Point p1 = new Point(432, 880);
                        Point p2 = new Point(540, 960);
                        point = LDPlayer.FindImageInSubSpace(emulatorName, PicturesPaths.PokerOpenedEn, p1, p2, 1);
                        break;
                    }*//*
                    
                    
                    result = false;
                }
            }*/


            point = null;
            count = 0;
            counter = 0;
            Task.Delay(rnd.Next(1000, 4000)).Wait();
            bool hasTables = false;
            while (point == null)
            {

                try
                {
                    if(counter > 30)
                    {
                        updateStateExceptPauseAndStop(form, index, "Не запустилась игра");
                        updateStatusExceptPauseAndStop(form, index, "Завершено");
                        return;
                    }
                    point = null;
                    Point p3 = new Point(132, 768);
                    Point p4 = new Point(413, 845);
                    point = LDPlayer.FindImageInSubSpace(emulatorName, PicturesPaths.EnterToPokerButton, p3, p4, 1);
                    if (point != null)
                    {
                        updateStateExceptPauseAndStop(form, index, "Аккаунт не залогинен");
                        updateStatusExceptPauseAndStop(form, index, "Завершено");
                        return;
                    }
                    if (counter % 2 == 0)
                    {
                        Point p1 = new Point(432, 880);
                        Point p2 = new Point(540, 960);
                        point = LDPlayer.FindImageInSubSpace(emulatorName, PicturesPaths.PokerOpened, p1, p2, 1);
                        if (point != null)
                        {
                            Point mailp1 = new Point(460, 85);
                            Point mailp2 = new Point(537, 162);
                            point = LDPlayer.FindImageInSubSpace(emulatorName, PicturesPaths.PokerOpened2, mailp1, mailp2, 1);
                            if (point != null)
                            {
                                Point addp1 = new Point(85, 5);
                                Point addp2 = new Point(157, 74);
                                point = LDPlayer.FindImageInSubSpace(emulatorName, PicturesPaths.PokerOpened3, addp1, addp2, 1);
                                if (point != null)
                                {
                                    break;
                                }
                            }
                            else
                            {
                                if (counter > 3 && count == 0)
                                {
                                    LDPlayer.KillApp(emulatorName, "com.lein.pppoker.android");
                                    Task.Delay(1000).Wait();
                                    LDPlayer.Open_App(emulatorName, "com.lein.pppoker.android");
                                    count++;
                                }
                                if (counter > 6 && count == 1)
                                {
                                    LDPlayer.KillApp(emulatorName, "com.lein.pppoker.android");
                                    Task.Delay(1000).Wait();
                                    LDPlayer.Open_App(emulatorName, "com.lein.pppoker.android");
                                    count++;
                                }
                                if (counter > 9 && count == 2)
                                {
                                    LDPlayer.KillApp(emulatorName, "com.lein.pppoker.android");
                                    Task.Delay(1000).Wait();
                                    LDPlayer.Open_App(emulatorName, "com.lein.pppoker.android");
                                    count++;
                                }
                                if (counter > 12 && count == 3)
                                {
                                    LDPlayer.KillApp(emulatorName, "com.lein.pppoker.android");
                                    Task.Delay(1000).Wait();
                                    LDPlayer.Open_App(emulatorName, "com.lein.pppoker.android");
                                    count++;
                                }
                                /*Image<Bgr, byte> image = LDPlayer.ScreenShoot(emulatorName).ToImage<Bgr, byte>();
                                Image<Bgr, byte> imgToFind = new Image<Bgr, byte>(PicturesPaths.SettingsHeader);
                                Image<Bgr, byte> imgToFind1 = new Image<Bgr, byte>(PicturesPaths.CloseSettings);
                                Image<Bgr, byte> imgToFind2 = new Image<Bgr, byte>(PicturesPaths.POPUP2);
                                Image<Bgr, byte> imgToFind3 = new Image<Bgr, byte>(PicturesPaths.POPUP3);
                                Image<Bgr, byte> imgToFind4 = new Image<Bgr, byte>(PicturesPaths.POPUP4);
                                Image<Bgr, byte> imgToFind5 = new Image<Bgr, byte>(PicturesPaths.POPUP5);
                                Image<Bgr, byte> imgToFind6 = new Image<Bgr, byte>(PicturesPaths.POPUP6);
                                Image<Bgr, byte> imgToFind7 = new Image<Bgr, byte>(PicturesPaths.POPUP7);

                                point = LDPlayer.FindCountour(image, imgToFind);
                                if(point != null)
                                {
                                    LDPlayer.Tap(emulatorName, point.Value.X, point.Value.Y);
                                    image.Dispose();
                                    image = LDPlayer.ScreenShoot(emulatorName).ToImage<Bgr, byte>();
                                }
                                point = LDPlayer.FindCountour(image, imgToFind2);
                                if (point != null)
                                {
                                    LDPlayer.Tap(emulatorName, point.Value.X, point.Value.Y);
                                    image.Dispose();
                                    image = LDPlayer.ScreenShoot(emulatorName).ToImage<Bgr, byte>();
                                }
                                point = LDPlayer.FindCountour(image, imgToFind3);
                                if (point != null)
                                {
                                    LDPlayer.Tap(emulatorName, point.Value.X, point.Value.Y);
                                    image.Dispose();
                                    image = LDPlayer.ScreenShoot(emulatorName).ToImage<Bgr, byte>();
                                }
                                point = LDPlayer.FindCountour(image, imgToFind4);
                                if (point != null)
                                {
                                    LDPlayer.Tap(emulatorName, point.Value.X, point.Value.Y);
                                    image.Dispose();
                                    image = LDPlayer.ScreenShoot(emulatorName).ToImage<Bgr, byte>();
                                }
                                point = LDPlayer.FindCountour(image, imgToFind5);
                                if (point != null)
                                {
                                    LDPlayer.Tap(emulatorName, point.Value.X, point.Value.Y);
                                    image.Dispose();
                                    image = LDPlayer.ScreenShoot(emulatorName).ToImage<Bgr, byte>();
                                }
                                point = LDPlayer.FindCountour(image, imgToFind6);
                                if (point != null)
                                {
                                    LDPlayer.Tap(emulatorName, point.Value.X, point.Value.Y);
                                    image.Dispose();
                                    image = LDPlayer.ScreenShoot(emulatorName).ToImage<Bgr, byte>();
                                }
                                point = LDPlayer.FindCountour(image, imgToFind7);
                                if (point != null)
                                {
                                    LDPlayer.Tap(emulatorName, point.Value.X, point.Value.Y);
                                    image.Dispose();
                                    image = LDPlayer.ScreenShoot(emulatorName).ToImage<Bgr, byte>();
                                }
                                image.Dispose();
                                imgToFind.Dispose();
                                imgToFind2.Dispose();
                                imgToFind3.Dispose();
                                imgToFind4.Dispose();
                                imgToFind5.Dispose();
                                imgToFind6.Dispose();
                                imgToFind7.Dispose();*/

                            }

                        }
                        else
                        {
                            if (counter > 3 && count == 0)
                            {
                                LDPlayer.KillApp(emulatorName, "com.lein.pppoker.android");
                                Task.Delay(1000).Wait();
                                LDPlayer.Open_App(emulatorName, "com.lein.pppoker.android");
                                count++;
                            }
                            if (counter > 6 && count == 1)
                            {
                                LDPlayer.KillApp(emulatorName, "com.lein.pppoker.android");
                                Task.Delay(1000).Wait();
                                LDPlayer.Open_App(emulatorName, "com.lein.pppoker.android");
                                count++;
                            }
                            if (counter > 9 && count == 2)
                            {
                                LDPlayer.KillApp(emulatorName, "com.lein.pppoker.android");
                                Task.Delay(1000).Wait();
                                LDPlayer.Open_App(emulatorName, "com.lein.pppoker.android");
                                count++;
                            }
                            if (counter > 12 && count == 3)
                            {
                                LDPlayer.KillApp(emulatorName, "com.lein.pppoker.android");
                                Task.Delay(1000).Wait();
                                LDPlayer.Open_App(emulatorName, "com.lein.pppoker.android");
                                count++;
                            }
                            /* Image<Bgr, byte> image = LDPlayer.ScreenShoot(emulatorName).ToImage<Bgr, byte>();
                             Image<Bgr, byte> imgToFind = new Image<Bgr, byte>(PicturesPaths.SettingsHeader);
                             Image<Bgr, byte> imgToFind1 = new Image<Bgr, byte>(PicturesPaths.CloseSettings);
                             Image<Bgr, byte> imgToFind2 = new Image<Bgr, byte>(PicturesPaths.POPUP2);
                             Image<Bgr, byte> imgToFind3 = new Image<Bgr, byte>(PicturesPaths.POPUP3);
                             Image<Bgr, byte> imgToFind4 = new Image<Bgr, byte>(PicturesPaths.POPUP4);
                             Image<Bgr, byte> imgToFind5 = new Image<Bgr, byte>(PicturesPaths.POPUP5);
                             Image<Bgr, byte> imgToFind6 = new Image<Bgr, byte>(PicturesPaths.POPUP6);
                             Image<Bgr, byte> imgToFind7 = new Image<Bgr, byte>(PicturesPaths.POPUP7);

                             point = LDPlayer.FindCountour(image, imgToFind);
                             if (point != null)
                             {
                                 LDPlayer.Tap(emulatorName, point.Value.X, point.Value.Y);
                                 image.Dispose();
                                 image = LDPlayer.ScreenShoot(emulatorName).ToImage<Bgr, byte>();
                             }
                             point = LDPlayer.FindCountour(image, imgToFind2);
                             if (point != null)
                             {
                                 LDPlayer.Tap(emulatorName, point.Value.X, point.Value.Y);
                                 image.Dispose();
                                 image = LDPlayer.ScreenShoot(emulatorName).ToImage<Bgr, byte>();
                             }
                             point = LDPlayer.FindCountour(image, imgToFind3);
                             if (point != null)
                             {
                                 LDPlayer.Tap(emulatorName, point.Value.X, point.Value.Y);
                                 image.Dispose();
                                 image = LDPlayer.ScreenShoot(emulatorName).ToImage<Bgr, byte>();
                             }
                             point = LDPlayer.FindCountour(image, imgToFind4);
                             if (point != null)
                             {
                                 LDPlayer.Tap(emulatorName, point.Value.X, point.Value.Y);
                                 image.Dispose();
                                 image = LDPlayer.ScreenShoot(emulatorName).ToImage<Bgr, byte>();
                             }
                             point = LDPlayer.FindCountour(image, imgToFind5);
                             if (point != null)
                             {
                                 LDPlayer.Tap(emulatorName, point.Value.X, point.Value.Y);
                                 image.Dispose();
                                 image = LDPlayer.ScreenShoot(emulatorName).ToImage<Bgr, byte>();
                             }
                             point = LDPlayer.FindCountour(image, imgToFind6);
                             if (point != null)
                             {
                                 LDPlayer.Tap(emulatorName, point.Value.X, point.Value.Y);
                                 image.Dispose();
                                 image = LDPlayer.ScreenShoot(emulatorName).ToImage<Bgr, byte>();
                             }
                             point = LDPlayer.FindCountour(image, imgToFind7);
                             if (point != null)
                             {
                                 LDPlayer.Tap(emulatorName, point.Value.X, point.Value.Y);
                                 image.Dispose();
                                 image = LDPlayer.ScreenShoot(emulatorName).ToImage<Bgr, byte>();
                             }
                             image.Dispose();
                             imgToFind.Dispose();
                             imgToFind2.Dispose();
                             imgToFind3.Dispose();
                             imgToFind4.Dispose();
                             imgToFind5.Dispose();
                             imgToFind6.Dispose();
                             imgToFind7.Dispose();*/
                        }
                    }
                    else
                    {
                        Point p1 = new Point(432, 880);
                        Point p2 = new Point(540, 960);
                        point = LDPlayer.FindImageInSubSpace(emulatorName, PicturesPaths.PokerOpenedEn, p1, p2, 1);
                        
                        if (point != null)
                        {
                            Point mailp1 = new Point(460, 85);
                            Point mailp2 = new Point(537, 162);
                            point = LDPlayer.FindImageInSubSpace(emulatorName, PicturesPaths.PokerOpened2, mailp1, mailp2, 1);
                            if (point != null)
                            {
                                Point addp1 = new Point(85, 5);
                                Point addp2 = new Point(157, 74);
                                point = LDPlayer.FindImageInSubSpace(emulatorName, PicturesPaths.PokerOpened3, addp1, addp2, 1);
                                if (point != null)
                                {
                                    break;
                                }
                            }
                            else
                            {
                                if (counter > 3 && count == 0)
                                {
                                    LDPlayer.KillApp(emulatorName, "com.lein.pppoker.android");
                                    Task.Delay(1000).Wait();
                                    LDPlayer.Open_App(emulatorName, "com.lein.pppoker.android");
                                    count++;
                                }
                                if (counter > 6 && count == 1)
                                {
                                    LDPlayer.KillApp(emulatorName, "com.lein.pppoker.android");
                                    Task.Delay(1000).Wait();
                                    LDPlayer.Open_App(emulatorName, "com.lein.pppoker.android");
                                    count++;
                                }
                                if (counter > 9 && count == 2)
                                {
                                    LDPlayer.KillApp(emulatorName, "com.lein.pppoker.android");
                                    Task.Delay(1000).Wait();
                                    LDPlayer.Open_App(emulatorName, "com.lein.pppoker.android");
                                    count++;
                                }
                                if (counter > 12 && count == 3)
                                {
                                    LDPlayer.KillApp(emulatorName, "com.lein.pppoker.android");
                                    Task.Delay(1000).Wait();
                                    LDPlayer.Open_App(emulatorName, "com.lein.pppoker.android");
                                    count++;
                                }
                                /* Image<Bgr, byte> image = LDPlayer.ScreenShoot(emulatorName).ToImage<Bgr, byte>();
                                 Image<Bgr, byte> imgToFind = new Image<Bgr, byte>(PicturesPaths.SettingsHeader);
                                 Image<Bgr, byte> imgToFind1 = new Image<Bgr, byte>(PicturesPaths.CloseSettings);
                                 Image<Bgr, byte> imgToFind2 = new Image<Bgr, byte>(PicturesPaths.POPUP2);
                                 Image<Bgr, byte> imgToFind3 = new Image<Bgr, byte>(PicturesPaths.POPUP3);
                                 Image<Bgr, byte> imgToFind4 = new Image<Bgr, byte>(PicturesPaths.POPUP4);
                                 Image<Bgr, byte> imgToFind5 = new Image<Bgr, byte>(PicturesPaths.POPUP5);
                                 Image<Bgr, byte> imgToFind6 = new Image<Bgr, byte>(PicturesPaths.POPUP6);
                                 Image<Bgr, byte> imgToFind7 = new Image<Bgr, byte>(PicturesPaths.POPUP7);

                                 point = LDPlayer.FindCountour(image, imgToFind);
                                 if (point != null)
                                 {
                                     LDPlayer.Tap(emulatorName, point.Value.X, point.Value.Y);
                                     image.Dispose();
                                     image = LDPlayer.ScreenShoot(emulatorName).ToImage<Bgr, byte>();
                                 }
                                 point = LDPlayer.FindCountour(image, imgToFind2);
                                 if (point != null)
                                 {
                                     LDPlayer.Tap(emulatorName, point.Value.X, point.Value.Y);
                                     image.Dispose();
                                     image = LDPlayer.ScreenShoot(emulatorName).ToImage<Bgr, byte>();
                                 }
                                 point = LDPlayer.FindCountour(image, imgToFind3);
                                 if (point != null)
                                 {
                                     LDPlayer.Tap(emulatorName, point.Value.X, point.Value.Y);
                                     image.Dispose();
                                     image = LDPlayer.ScreenShoot(emulatorName).ToImage<Bgr, byte>();
                                 }
                                 point = LDPlayer.FindCountour(image, imgToFind4);
                                 if (point != null)
                                 {
                                     LDPlayer.Tap(emulatorName, point.Value.X, point.Value.Y);
                                     image.Dispose();
                                     image = LDPlayer.ScreenShoot(emulatorName).ToImage<Bgr, byte>();
                                 }
                                 point = LDPlayer.FindCountour(image, imgToFind5);
                                 if (point != null)
                                 {
                                     LDPlayer.Tap(emulatorName, point.Value.X, point.Value.Y);
                                     image.Dispose();
                                     image = LDPlayer.ScreenShoot(emulatorName).ToImage<Bgr, byte>();
                                 }
                                 point = LDPlayer.FindCountour(image, imgToFind6);
                                 if (point != null)
                                 {
                                     LDPlayer.Tap(emulatorName, point.Value.X, point.Value.Y);
                                     image.Dispose();
                                     image = LDPlayer.ScreenShoot(emulatorName).ToImage<Bgr, byte>();
                                 }
                                 point = LDPlayer.FindCountour(image, imgToFind7);
                                 if (point != null)
                                 {
                                     LDPlayer.Tap(emulatorName, point.Value.X, point.Value.Y);
                                     image.Dispose();
                                     image = LDPlayer.ScreenShoot(emulatorName).ToImage<Bgr, byte>();
                                 }
                                 image.Dispose();
                                 imgToFind.Dispose();
                                 imgToFind2.Dispose();
                                 imgToFind3.Dispose();
                                 imgToFind4.Dispose();
                                 imgToFind5.Dispose();
                                 imgToFind6.Dispose();
                                 imgToFind7.Dispose();*/
                            }

                        }
                        else
                        {
                            if (counter > 3 && count == 0)
                            {
                                LDPlayer.KillApp(emulatorName, "com.lein.pppoker.android");
                                Task.Delay(1000).Wait();
                                LDPlayer.Open_App(emulatorName, "com.lein.pppoker.android");
                                count++;
                            }
                            if (counter > 6 && count == 1)
                            {
                                LDPlayer.KillApp(emulatorName, "com.lein.pppoker.android");
                                Task.Delay(1000).Wait();
                                LDPlayer.Open_App(emulatorName, "com.lein.pppoker.android");
                                count++;
                            }
                            if (counter > 9 && count == 2)
                            {
                                LDPlayer.KillApp(emulatorName, "com.lein.pppoker.android");
                                Task.Delay(1000).Wait();
                                LDPlayer.Open_App(emulatorName, "com.lein.pppoker.android");
                                count++;
                            }
                            if (counter > 12 && count == 3)
                            {
                                LDPlayer.KillApp(emulatorName, "com.lein.pppoker.android");
                                Task.Delay(1000).Wait();
                                LDPlayer.Open_App(emulatorName, "com.lein.pppoker.android");
                                count++;
                            }
                            /*Image<Bgr, byte> image = LDPlayer.ScreenShoot(emulatorName).ToImage<Bgr, byte>();
                            Image<Bgr, byte> imgToFind = new Image<Bgr, byte>(PicturesPaths.SettingsHeader);
                            Image<Bgr, byte> imgToFind1 = new Image<Bgr, byte>(PicturesPaths.CloseSettings);
                            Image<Bgr, byte> imgToFind2 = new Image<Bgr, byte>(PicturesPaths.POPUP2);
                            Image<Bgr, byte> imgToFind3 = new Image<Bgr, byte>(PicturesPaths.POPUP3);
                            Image<Bgr, byte> imgToFind4 = new Image<Bgr, byte>(PicturesPaths.POPUP4);
                            Image<Bgr, byte> imgToFind5 = new Image<Bgr, byte>(PicturesPaths.POPUP5);
                            Image<Bgr, byte> imgToFind6 = new Image<Bgr, byte>(PicturesPaths.POPUP6);
                            Image<Bgr, byte> imgToFind7 = new Image<Bgr, byte>(PicturesPaths.POPUP7);

                            point = LDPlayer.FindCountour(image, imgToFind);
                            if (point != null)
                            {
                                LDPlayer.Tap(emulatorName, point.Value.X, point.Value.Y);
                                image.Dispose();
                                image = LDPlayer.ScreenShoot(emulatorName).ToImage<Bgr, byte>();
                            }
                            point = LDPlayer.FindCountour(image, imgToFind2);
                            if (point != null)
                            {
                                LDPlayer.Tap(emulatorName, point.Value.X, point.Value.Y);
                                image.Dispose();
                                image = LDPlayer.ScreenShoot(emulatorName).ToImage<Bgr, byte>();
                            }
                            point = LDPlayer.FindCountour(image, imgToFind3);
                            if (point != null)
                            {
                                LDPlayer.Tap(emulatorName, point.Value.X, point.Value.Y);
                                image.Dispose();
                                image = LDPlayer.ScreenShoot(emulatorName).ToImage<Bgr, byte>();
                            }
                            point = LDPlayer.FindCountour(image, imgToFind4);
                            if (point != null)
                            {
                                LDPlayer.Tap(emulatorName, point.Value.X, point.Value.Y);
                                image.Dispose();
                                image = LDPlayer.ScreenShoot(emulatorName).ToImage<Bgr, byte>();
                            }
                            point = LDPlayer.FindCountour(image, imgToFind5);
                            if (point != null)
                            {
                                LDPlayer.Tap(emulatorName, point.Value.X, point.Value.Y);
                                image.Dispose();
                                image = LDPlayer.ScreenShoot(emulatorName).ToImage<Bgr, byte>();
                            }
                            point = LDPlayer.FindCountour(image, imgToFind6);
                            if (point != null)
                            {
                                LDPlayer.Tap(emulatorName, point.Value.X, point.Value.Y);
                                image.Dispose();
                                image = LDPlayer.ScreenShoot(emulatorName).ToImage<Bgr, byte>();
                            }
                            point = LDPlayer.FindCountour(image, imgToFind7);
                            if (point != null)
                            {
                                LDPlayer.Tap(emulatorName, point.Value.X, point.Value.Y);
                                image.Dispose();
                                image = LDPlayer.ScreenShoot(emulatorName).ToImage<Bgr, byte>();
                            }
                            image.Dispose();
                            imgToFind.Dispose();
                            imgToFind2.Dispose();
                            imgToFind3.Dispose();
                            imgToFind4.Dispose();
                            imgToFind5.Dispose();
                            imgToFind6.Dispose();
                            imgToFind7.Dispose();*/
                        }
                    }
                    Task.Delay(500).Wait();
                    counter++;
                    point = null;

                }
                catch (System.ArgumentException e)
                {
                    point = null;
                }
                if (point != null)
                {
                    break;
                }
                else
                {
                    if(updateState(form, index, "Жду включение игры и закрываю левые окна"))
                    {
                        updateStateExceptPauseAndStop(form, index, "");
                        updateStatusExceptPauseAndStop(form, index, "Остановлен");
                        return;
                    }
                }
                Task.Delay(500).Wait();
            }

            if(updateState(form, index, "Покер Запущен"))
            {
                updateStateExceptPauseAndStop(form, index, "");
                updateStatusExceptPauseAndStop(form, index, "Остановлен");
                return;
            }
            Task.Delay(rnd.Next(500, 2000)).Wait();
            if(updateState(form, index, "Проверяю есть ли клуб"))
            {
                updateStateExceptPauseAndStop(form, index, "");
                updateStatusExceptPauseAndStop(form, index, "Остановлен");
                return;
            }
            point = null;
            count = 0;
            bool isInClub = false;
            while (point == null)
            {
                if (count > 0)
                {

                    if(updateState(form, index, "Клуб есть"))
                    {
                        updateStateExceptPauseAndStop(form, index, "");
                        updateStatusExceptPauseAndStop(form, index, "Остановлен");
                        return;
                    }
                    isInClub = true;
                    break;
                }
                try
                {
                    point = LDPlayer.FindImage(emulatorName, PicturesPaths.ButtonEnterToClub, 1);
                }
                catch (System.ArgumentException e)
                {
                    point = null;
                }
                if (point != null)
                {
                    if(updateState(form, index, "Клуба нет"))
                    {
                        updateStatusExceptPauseAndStop(form, index, "Остановлен");
                        return;
                    }
                    break;
                }
                else
                {
                    if(updateState(form, index, "Проверяю есть ли клуб"))
                    {
                        updateStateExceptPauseAndStop(form, index, "");
                        updateStatusExceptPauseAndStop(form, index, "Остановлен");
                        return;
                    }
                }
                Task.Delay(rnd.Next(500, 1000)).Wait();
                count++;
            }

            result = false;
            count = 0;
            if (isInClub)
            {
                Task.Delay(rnd.Next(500, 2000)).Wait();
                if(updateState(form, index, "Захожу в клуб"))
                {
                    updateStateExceptPauseAndStop(form, index, "");
                    updateStatusExceptPauseAndStop(form, index, "Остановлен");
                    return;
                }
                result = LDPlayer.FindImageAndClick(emulatorName, PicturesPaths.ClickToClub, 1);
                if (!result)
                {
                    while (!result)
                    {
                        if (count > 20)
                        {
                            updateStateExceptPauseAndStop(form, index, "Не нашел клуб");
                            updateStatusExceptPauseAndStop(form, index, "Остановлен");
                            return;
                        }
                        if(updateState(form, index, "Захожу в клуб"))
                        {
                            updateStateExceptPauseAndStop(form, index, "");
                            updateStatusExceptPauseAndStop(form, index, "Остановлен");
                            return;
                        }
                        Task.Delay(rnd.Next(500, 1000)).Wait();
                        result = LDPlayer.FindImageAndClick(emulatorName, PicturesPaths.ClickToClub, 1);
                        count++;
                    }
                }
                if(updateState(form, index, "Зашел в клуб"))
                {
                    updateStateExceptPauseAndStop(form, index, "");
                    updateStatusExceptPauseAndStop(form, index, "Остановлен");
                    return;
                }

                Task.Delay(rnd.Next(500, 2000)).Wait();
                if(updateState(form, index, "Ищу столы"))
                {
                    updateStateExceptPauseAndStop(form, index, "");
                    updateStatusExceptPauseAndStop(form, index, "Остановлен");
                    return;
                }
                point = null;
                counter = 0;
                
                while (point == null)
                {
                    counter++;
                    if (counter > 3)
                    {
                        if(updateState(form, index, "Столов нет"))
                        {
                            updateStateExceptPauseAndStop(form, index, "");
                            updateStatusExceptPauseAndStop(form, index, "Остановлен");
                            return;
                        }
                        break;
                    }
                    try
                    {
                        Point p1 = new Point(1, 390);
                        Point p2 = new Point(540, 830);
                        point = LDPlayer.FindImageInSubSpace(emulatorName, PicturesPaths.PokerTable,p1,p2, 1);
                    }
                    catch (System.ArgumentException e)
                    {
                        point = null;
                    }
                    if (point != null)
                    {
                        hasTables = true;

                        if(updateState(form, index, "Стол найден, захожу"))
                        {
                            updateStateExceptPauseAndStop(form, index, "");
                            updateStatusExceptPauseAndStop(form, index, "Остановлен");
                            return;
                        }
                        break;
                    }
                    else
                    {
                        if(updateState(form, index, "Ищу столы"))
                        {
                            updateStateExceptPauseAndStop(form, index, "");
                            updateStatusExceptPauseAndStop(form, index, "Остановлен");
                            return;
                        }
                    }
                    Task.Delay(rnd.Next(500, 2000)).Wait();
                }

                if (hasTables)
                {
                    result = false;

                    if(updateState(form, index, "Нажимаю зайти на стол"))
                    {
                        updateStateExceptPauseAndStop(form, index, "");
                        updateStatusExceptPauseAndStop(form, index, "Остановлен");
                        return;
                    }
                    Task.Delay(rnd.Next(500, 2000)).Wait();
                    Point pTable1 = new Point(1, 390);
                    Point pTable2 = new Point(540, 830);
                    point = LDPlayer.FindImageInSubSpace(emulatorName, PicturesPaths.PokerTable, pTable1, pTable2, 1);
                    if (point == null)
                    {
                        while (point == null)
                        {

                            if(updateState(form, index, "Нажимаю зайти на стол"))
                            {
                                updateStateExceptPauseAndStop(form, index, "");
                                updateStatusExceptPauseAndStop(form, index, "Остановлен");
                                return;
                            }
                            Task.Delay(rnd.Next(500, 2000)).Wait();
                            point = LDPlayer.FindImageInSubSpace(emulatorName, PicturesPaths.PokerTable, pTable1, pTable2, 1);

                        }
                        LDPlayer.Tap(emulatorName, point.Value.X+1, point.Value.Y+390);

                    }
                    else
                    {
                        LDPlayer.Tap(emulatorName, point.Value.X+1, point.Value.Y+390);

                    }

                    Task.Delay(rnd.Next(500, 2000)).Wait();
                    if(updateState(form, index, "Нажимаю меню"))
                    {
                        updateStateExceptPauseAndStop(form, index, "");
                        updateStatusExceptPauseAndStop(form, index, "Остановлен");
                        return;
                    }
                    counter = 0;
                    point = null;
                    /*while (point == null)
                    {
                        counter++;
                        if(counter > 20)
                        {
                            updateStateExceptPauseAndStop(form, index, "Не нашел меню на столе");
                            updateStatusExceptPauseAndStop(form, index, "Остановлен");
                            return;
                        }
                        try
                        {
                            Point p1 = new Point(1,1);
                            Point p2 = new Point(80,80);
                            point = LDPlayer.FindImageInSubSpace(emulatorName, PicturesPaths.TableMenu,p1,p2, 1);
                        }
                        catch (System.ArgumentException e)
                        {
                            point = null;
                        }
                        if (point != null)
                        {
                            hasTables = true;
                            LDPlayer.Tap(emulatorName, point.Value.X, point.Value.Y);
                            if (updateState(form, index, "Зашел в меню"))
                            {
                                updateStateExceptPauseAndStop(form, index, "");
                                updateStatusExceptPauseAndStop(form, index, "Остановлен");
                                return;
                            }
                            break;
                        }
                        else
                        {
                            if(updateState(form, index, "Нажимаю меню"))
                            {
                                updateStateExceptPauseAndStop(form, index, "");
                                updateStatusExceptPauseAndStop(form, index, "Остановлен");
                                return;
                            }
                        }
                        Task.Delay(rnd.Next(500, 1000)).Wait();
                    }*/
                    

                    result = false;

                    
                    Task.Delay(rnd.Next(1000, 1000)).Wait();
                    if(updateState(form, index, "Нажимаю настройки"))
                    {
                        updateStateExceptPauseAndStop(form, index, "");
                        updateStatusExceptPauseAndStop(form, index, "Остановлен");
                        return;
                    }
                    counter = 0;
                    point = null;
                    bool targetPos = false;
                    bool secondSettings = false;
                    while (point == null)
                    {

                        counter++;
                        if( counter < 20)
                        {
                            Point p1 = new Point(1, 1);
                            Point p2 = new Point(80, 80);
                            point = LDPlayer.FindImageInSubSpace(emulatorName, PicturesPaths.TableMenu, p1, p2, 1);
                            if(point != null)
                            {
                               // LDPlayer.Tap(emulatorName, point.Value.X, point.Value.Y);
                                LDPlayer.Tap(emulatorName, rnd.Next(21, 51), rnd.Next(31, 53));
                            }
                            Task.Delay(500).Wait();
                        }
                        
                        if(counter > 20)
                        {
                            updateStateExceptPauseAndStop(form, index, "Не нашел меню на столе");
                            updateStatusExceptPauseAndStop(form, index, "Остановлен");
                            return;
                        }
                        try
                        {
                            Point p1 = new Point(21, 338);
                            Point p2 = new Point(231, 383);
                            Point p3 = new Point(16, 593);
                            Point p4 = new Point(233, 650);
                            //если сломается поменять в point p1 и p2 на p3 и p4
                            /*point = LDPlayer.FindImageInSubSpace(emulatorName, PicturesPaths.ExitTable, p3, p4, 1);
                            if(point == null)
                            {
                                Task.Delay(1000).Wait();
                                point = LDPlayer.FindImageInSubSpace(emulatorName, PicturesPaths.ExitTableEn, p3, p4, 1);
                                if(point != null)
                                {
                                    targetPos = false;
                                    LDPlayer.Tap(emulatorName, rnd.Next(21, 235), rnd.Next(207, 260));
                                    
                                    p1 = new Point(21, 338);
                                    p2 = new Point(231, 383);
                                    break;
                                }
                            }
                            else
                            {
                                targetPos = false;
                                LDPlayer.Tap(emulatorName, rnd.Next(21, 235), rnd.Next(207, 260));
                                
                                p1 = new Point(21, 338);
                                p2 = new Point(231, 383);
                                break;
                            }
                            if (point == null)
                            {
                                Task.Delay(1000).Wait();
                                point = LDPlayer.FindImageInSubSpace(emulatorName, PicturesPaths.ExitTable, p3, p4, 1);
                                if(point == null)
                                {
                                    Task.Delay(1000).Wait();
                                    point = LDPlayer.FindImageInSubSpace(emulatorName, PicturesPaths.ExitTableEn, p3, p4, 1);
                                    if(point!= null)
                                    {
                                        targetPos = true;
                                        LDPlayer.Tap(emulatorName, rnd.Next(21, 231), rnd.Next(338, 383));
                                        p1 = new Point(21, 207);
                                        p2 = new Point(235, 260);
                                        secondSettings = true;
                                        break;
                                    }
                                }
                                if(point!= null)
                                {
                                    targetPos = true;
                                    LDPlayer.Tap(emulatorName, rnd.Next(21, 231), rnd.Next(338, 383));
                                    p1 = new Point(21, 207);
                                    p2 = new Point(235, 260);
                                    secondSettings = true;
                                    break;
                                }
                            }*/
                            
                            point = LDPlayer.FindImageInSubSpace(emulatorName, PicturesPaths.ExitTable, p3, p4, 1);
                            if (point == null)
                            {
                                Task.Delay(1000).Wait();
                                point = LDPlayer.FindImageInSubSpace(emulatorName, PicturesPaths.ExitTableEn, p3, p4, 1);
                                if (point != null)
                                {
                                    targetPos = true;
                                    LDPlayer.Tap(emulatorName, rnd.Next(21, 231), rnd.Next(338, 383));
                                    p1 = new Point(21, 207);
                                    p2 = new Point(235, 260);
                                    secondSettings = true;
                                    break;
                                }
                            }
                            if (point != null)
                            { 
                                targetPos = true;
                                LDPlayer.Tap(emulatorName, rnd.Next(21, 231), rnd.Next(338, 383));
                                p1 = new Point(21, 207);
                                p2 = new Point(235, 260);
                                secondSettings = true;
                                break;
                            }
                            
                            point = LDPlayer.FindImageInSubSpace(emulatorName, PicturesPaths.ExitTable, p1, p2, 1);
                            if (point == null)
                            {
                                Task.Delay(1000).Wait();
                                point = LDPlayer.FindImageInSubSpace(emulatorName, PicturesPaths.ExitTableEn, p1, p2, 1);
                                if (point != null)
                                {
                                    targetPos = false;
                                    LDPlayer.Tap(emulatorName, rnd.Next(21, 235), rnd.Next(207, 260));

                                    p1 = new Point(21, 338);
                                    p2 = new Point(231, 383);
                                    break;
                                }
                            }
                            else
                            {
                                targetPos = false;
                                LDPlayer.Tap(emulatorName, rnd.Next(21, 235), rnd.Next(207, 260));

                                p1 = new Point(21, 338);
                                p2 = new Point(231, 383);
                                break;
                            }
                            

                            /*if(counter % 2 == 0)
                            {
                                point = LDPlayer.FindImageInSubSpace(emulatorName, PicturesPaths.TableOptions, p1, p2, 1);

                            }
                            else
                            {
                                point = LDPlayer.FindImageInSubSpace(emulatorName, PicturesPaths.TableOptionsEn, p1, p2, 1);

                            }*/
                            Task.Delay(rnd.Next(500, 1000)).Wait();
                        }
                        catch (System.ArgumentException e)
                        {
                            point = null;
                        }
                        if (point != null)
                        {
                            
                            if (updateState(form, index, "Зашел в настройки"))
                            {
                                
                                updateStateExceptPauseAndStop(form, index, "");
                                updateStatusExceptPauseAndStop(form, index, "Остановлен");
                                return;
                            }
                            break;
                        }
                        else
                        {
                            if (updateState(form, index, "Нажимаю на кнопку настройки"))
                            {
                                updateStateExceptPauseAndStop(form, index, "");
                                updateStatusExceptPauseAndStop(form, index, "Остановлен");
                                return;
                            }
                        }
                        
                    }
                    Task.Delay(rnd.Next(500, 1000)).Wait();
                    if(updateState(form, index, "Жду шапку настроек"))
                    {
                        updateStateExceptPauseAndStop(form, index, "");
                        updateStatusExceptPauseAndStop(form, index, "Остановлен");
                        return;
                    }
                    point = null;
                    counter = 0;
                    while (point == null)
                    {
                        counter++;
                        if(counter> 20)
                        {
                            updateStateExceptPauseAndStop(form, index, "Не открылись настройки стола");
                            updateStatusExceptPauseAndStop(form, index, "Остановлен");
                            return;
                        }
                        try
                        {
                            point = LDPlayer.FindImage(emulatorName, PicturesPaths.SettingsHeader, 1);
                        }
                        catch (System.ArgumentException e)
                        {
                            point = null;
                        }
                        if (point != null)
                        {
                            hasTables = true;

                            if(updateState(form, index, "Зашел в настройки"))
                            {
                                updateStateExceptPauseAndStop(form, index, "");
                                updateStatusExceptPauseAndStop(form, index, "Остановлен");
                                return;
                            }
                            break;
                        }
                        else
                        {
                            if(updateState(form, index, "Жду шапку настроек"))
                            {
                                updateStateExceptPauseAndStop(form, index, "");
                                updateStatusExceptPauseAndStop(form, index, "Остановлен");
                                return;
                            }
                        }
                        Task.Delay(rnd.Next(500, 1000)).Wait();
                    }
                    int posx = rnd.Next(137, 380);
                    int posy = rnd.Next(218, 300);
                    int swipes = rnd.Next(1, 2);
                    for (int i =0; i < swipes; i++)
                    {
                        LDPlayer.Swipe(emulatorName, posx, posy, posx, rnd.Next(740, 945), rnd.Next(300, 600));
                        Task.Delay(rnd.Next(300, 800)).Wait();
                    }
                    
                    //LDPlayer.Swipe(emulatorName, posx, posy, posx, rnd.Next(740, 945), rnd.Next(500, 1000));

                    Task.Delay(rnd.Next(1000, 1000)).Wait();
                    /*if(updateState(form, index, "Проверяю эквити"))
                    {
                        updateStateExceptPauseAndStop(form, index, "");
                        updateStatusExceptPauseAndStop(form, index, "Остановлен");
                        return;
                    }
                    point = null;
                    counter = 0;
                    while (point == null)
                    {
                        counter++;
                        if (counter > 1)
                        {
                            if(updateState(form, index, "Эквити выключен"))
                            {
                                updateStateExceptPauseAndStop(form, index, "");
                                updateStatusExceptPauseAndStop(form, index, "Остановлен");
                                return;
                            }
                            break;
                        }
                        try
                        {
                            Point p1 = new Point(403,208);
                            Point p2 = new Point(500,237);
                            point = LDPlayer.FindImageInSubSpace(emulatorName, PicturesPaths.SettingIsOn,p1,p2, 1);
                        }
                        catch (System.ArgumentException e)
                        {
                            point = null;
                        }
                        if (point != null)
                        {
                            hasTables = true;

                            if(updateState(form, index, "Эквити включен, выключаю"))
                            {
                                updateStateExceptPauseAndStop(form, index, "");
                                updateStatusExceptPauseAndStop(form, index, "Остановлен");
                                return;
                            }
                            LDPlayer.Tap(emulatorName, 403 + point.Value.X, 208 + point.Value.Y);
                            break;
                        }
                        else
                        {
                            if(updateState(form, index, "Проверяю эквити"))
                            {
                                updateStateExceptPauseAndStop(form, index, "");
                                updateStatusExceptPauseAndStop(form, index, "Остановлен");
                                return;
                            }
                        }
                        Task.Delay(rnd.Next(500, 2000)).Wait();
                    }
                    if(updateState(form, index, "Проверяю Сдать 2/3 раза"))
                    {
                        updateStateExceptPauseAndStop(form, index, "");
                        updateStatusExceptPauseAndStop(form, index, "Остановлен");
                        return;
                    }
                    point = null;
                    counter = 0;
                    while (point == null)
                    {
                        counter++;
                        if (counter > 1)
                        {
                            if(updateState(form, index, "Сдать 2/3 раза выключен"))
                            {
                                updateStateExceptPauseAndStop(form, index, "");
                                updateStatusExceptPauseAndStop(form, index, "Остановлен");
                                return;
                            }
                            break;
                        }
                        try
                        {
                            Point p1 = new Point(409, 267);
                            Point p2 = new Point(492, 296);
                            point = LDPlayer.FindImageInSubSpace(emulatorName, PicturesPaths.SettingIsOn, p1, p2, 1);
                        }
                        catch (System.ArgumentException e)
                        {
                            point = null;
                        }
                        if (point != null)
                        {
                            hasTables = true;

                            if(updateState(form, index, "Сдать 2/3 раза включен, выключаю"))
                            {
                                updateStateExceptPauseAndStop(form, index, "");
                                updateStatusExceptPauseAndStop(form, index, "Остановлен");
                                return;
                            }
                            LDPlayer.Tap(emulatorName, 409 + point.Value.X, 267 + point.Value.Y);
                            break;
                        }
                        else
                        {
                            if(updateState(form, index, "Проверяю Сдать 2/3 раза"))
                            {
                                updateStateExceptPauseAndStop(form, index, "");
                                updateStatusExceptPauseAndStop(form, index, "Остановлен");
                                return;
                            }
                        }
                        Task.Delay(rnd.Next(500, 1000)).Wait();
                    }
                    
                    if(updateState(form, index, "Проверяю Голосовое сообщение"))
                    {
                        updateStateExceptPauseAndStop(form, index, "");
                        updateStatusExceptPauseAndStop(form, index, "Остановлен");
                        return;
                    }
                    point = null;
                    counter = 0;
                    while (point == null)
                    {
                        counter++;
                        if (counter > 1)
                        {
                            if(updateState(form, index, "Голосовые сообщения выключены"))
                            {
                                updateStateExceptPauseAndStop(form, index, "");
                                updateStatusExceptPauseAndStop(form, index, "Остановлен");
                                return;
                            }
                            break;
                        }
                        try
                        {
                            Point p1 = new Point(409, 345);
                            Point p2 = new Point(492, 373);
                            point = LDPlayer.FindImageInSubSpace(emulatorName, PicturesPaths.SettingIsOn, p1, p2, 1);
                        }
                        catch (System.ArgumentException e)
                        {
                            point = null;
                        }
                        if (point != null)
                        {
                            hasTables = true;

                            if(updateState(form, index, "Голосовые сообщение включено, выключаю"))
                            {
                                updateStateExceptPauseAndStop(form, index, "");
                                updateStatusExceptPauseAndStop(form, index, "Остановлен");
                                return;
                            }
                            LDPlayer.Tap(emulatorName, 409 + point.Value.X, 345 + point.Value.Y);
                            break;
                        }
                        else
                        {
                            if(updateState(form, index, "Проверяю Голосовое сообщение"))
                            {
                                updateStateExceptPauseAndStop(form, index, "");
                                updateStatusExceptPauseAndStop(form, index, "Остановлен");
                                return;
                            }
                        }
                        Task.Delay(rnd.Next(500, 1000)).Wait();
                    }
                    Task.Delay(rnd.Next(500, 2000)).Wait();
                    if(updateState(form, index, "Проверяю Текстовые сообщение"))
                    {
                        updateStateExceptPauseAndStop(form, index, "");
                        updateStatusExceptPauseAndStop(form, index, "Остановлен");
                        return;
                    }
                    point = null;
                    counter = 0;
                    while (point == null)
                    {
                        counter++;
                        if (counter > 1)
                        {
                            if(updateState(form, index, "Текстовые сообщения выключены"))
                            {
                                updateStateExceptPauseAndStop(form, index, "");
                                updateStatusExceptPauseAndStop(form, index, "Остановлен");
                                return;
                            }
                            break;
                        }
                        try
                        {
                            Point p1 = new Point(408, 399);
                            Point p2 = new Point(495, 435);
                            point = LDPlayer.FindImageInSubSpace(emulatorName, PicturesPaths.SettingIsOn, p1, p2, 1);
                        }
                        catch (System.ArgumentException e)
                        {
                            point = null;
                        }
                        if (point != null)
                        {
                            hasTables = true;

                            if(updateState(form, index, "Текстовые сообщение включено, выключаю"))
                            {
                                updateStateExceptPauseAndStop(form, index, "");
                                updateStatusExceptPauseAndStop(form, index, "Остановлен");
                                return;
                            }
                            LDPlayer.Tap(emulatorName, 408 + point.Value.X, 399 + point.Value.Y);
                            break;
                        }
                        else
                        {
                            if(updateState(form, index, "Проверяю Текстовые сообщение"))
                            {
                                updateStateExceptPauseAndStop(form, index, "");
                                updateStatusExceptPauseAndStop(form, index, "Остановлен");
                                return;
                            }
                        }
                        Task.Delay(rnd.Next(500, 1000)).Wait();
                    }
                    Task.Delay(rnd.Next(1000, 2000)).Wait();
                    if(updateState(form, index, "Проверяю Сквиз карт"))
                    {
                        updateStateExceptPauseAndStop(form, index, "");
                        updateStatusExceptPauseAndStop(form, index, "Остановлен");
                        return;
                    }*/
                    point = null;
                    counter = 0;
                    while (point == null)
                    {
                        counter++;
                        if (counter > 2)
                        {
                            if(updateState(form, index, "Сквиз карт выключен"))
                            {
                                updateStateExceptPauseAndStop(form, index, "");
                                updateStatusExceptPauseAndStop(form, index, "Остановлен");
                                return;
                            }
                            break;
                        }
                        try
                        {
                            Point p1 = new Point(407, 617);
                            Point p2 = new Point(493, 650);
                            point = LDPlayer.FindImageInSubSpace(emulatorName, PicturesPaths.SettingIsOn, p1, p2, 1);
                        }
                        catch (System.ArgumentException e)
                        {
                            point = null;
                        }
                        if (point != null)
                        {
                            hasTables = true;

                            if(updateState(form, index, "Сквиз карт включен, выключаю"))
                            {
                                updateStateExceptPauseAndStop(form, index, "");
                                updateStatusExceptPauseAndStop(form, index, "Остановлен");
                                return;
                            }
                            LDPlayer.Tap(emulatorName, 407 + point.Value.X, 617 + point.Value.Y);
                            break;
                        }
                        else
                        {
                            if(updateState(form, index, "Проверяю Сквиз карт"))
                            {
                                updateStateExceptPauseAndStop(form, index, "");
                                updateStatusExceptPauseAndStop(form, index, "Остановлен");
                                return;
                            }
                        }
                        Task.Delay(rnd.Next(500, 1000)).Wait();
                    }
                    result = false;

                    if(updateState(form, index, "Закрываю Настройки"))
                    {
                        updateStateExceptPauseAndStop(form, index, "");
                        updateStatusExceptPauseAndStop(form, index, "Остановлен");
                        return;
                    }
                    Task.Delay(rnd.Next(500, 1000)).Wait();
                    result = LDPlayer.FindImageAndClick(emulatorName, PicturesPaths.CloseSettings, 1);
                    if (!result)
                    {
                        while (!result)
                        {

                            if(updateState(form, index, "Закрываю настройки"))
                            {
                                updateStateExceptPauseAndStop(form, index, "");
                                updateStatusExceptPauseAndStop(form, index, "Остановлен");
                                return;
                            }
                            Task.Delay(rnd.Next(500, 1000)).Wait();
                            result = LDPlayer.FindImageAndClick(emulatorName, PicturesPaths.CloseSettings, 1);
                        }
                    }
                    Task.Delay(rnd.Next(500, 1000)).Wait();
                    if(updateState(form, index, "Нажимаю меню"))
                    {
                        updateStateExceptPauseAndStop(form, index, "");
                        updateStatusExceptPauseAndStop(form, index, "Остановлен");
                        return;
                    }
                    point = null;
                    while (point == null)
                    {
                        counter++;
                        try
                        {
                            point = LDPlayer.FindImage(emulatorName, PicturesPaths.TableMenu, 1);
                        }
                        catch (System.ArgumentException e)
                        {
                            point = null;
                        }
                        if (point != null)
                        {
                            hasTables = true;

                            if(updateState(form, index, "Зашел в меню"))
                            {
                                updateStateExceptPauseAndStop(form, index, "");
                                updateStatusExceptPauseAndStop(form, index, "Остановлен");
                                return;
                            }
                            break;
                        }
                        else
                        {
                            if(updateState(form, index, "Нажимаю меню"))
                            {
                                updateStateExceptPauseAndStop(form, index, "");
                                updateStatusExceptPauseAndStop(form, index, "Остановлен");
                                return;
                            }
                        }
                        Task.Delay(rnd.Next(500, 1000)).Wait();
                    }
                    LDPlayer.Tap(emulatorName, point.Value.X, point.Value.Y);
                    Task.Delay(rnd.Next(1000, 2000)).Wait();
                    if(updateState(form, index, "Нажимаю выход со стола"))
                    {
                        updateStateExceptPauseAndStop(form, index, "");
                        updateStatusExceptPauseAndStop(form, index, "Остановлен");
                        return;
                    }
                    point = null;
                    counter = 0;
                    while (point == null)
                    {
                        counter++;
                        if (counter < 20)
                        {
                            Point p1 = new Point(1, 1);
                            Point p2 = new Point(80, 80);
                            point = LDPlayer.FindImageInSubSpace(emulatorName, PicturesPaths.TableMenu, p1, p2, 1);
                            if (point != null)
                            {
                                //LDPlayer.Tap(emulatorName, point.Value.X, point.Value.Y);
                                LDPlayer.Tap(emulatorName, rnd.Next(21,51), rnd.Next(31,53));
                            }
                            Task.Delay(500).Wait();
                        }
                        if (counter > 10)
                        {
                            
                            updateStateExceptPauseAndStop(form, index, "Не нашел выход со стола");
                            updateStatusExceptPauseAndStop(form, index, "Остановлен");
                            return;
                            
                        }
                        try
                        {
                            Point p1 = new Point(21, 338);
                            Point p2 = new Point(231, 383);
                            Point p3 = new Point(16, 593);
                            Point p4 = new Point(233, 650);





                            
                            if (secondSettings)
                            {
                                point = LDPlayer.FindImageInSubSpace(emulatorName, PicturesPaths.ExitTable, p3, p4, 1);
                                if (point == null)
                                {
                                    Task.Delay(1000).Wait();
                                    point = LDPlayer.FindImageInSubSpace(emulatorName, PicturesPaths.ExitTableEn, p3, p4, 1);
                                }
                                if (point != null)
                                {
                                    LDPlayer.Tap(emulatorName, rnd.Next(16, 233), rnd.Next(593, 650));
                                }
                            }
                            else
                            {//если сломается поменять в point p1 и p2 на p3 и p4
                                point = LDPlayer.FindImageInSubSpace(emulatorName, PicturesPaths.ExitTable, p1, p2, 1);
                                if (point == null)
                                {
                                    Task.Delay(1000).Wait();
                                    point = LDPlayer.FindImageInSubSpace(emulatorName, PicturesPaths.ExitTableEn, p1, p2, 1);
                                }
                                if (point != null)
                                {
                                    LDPlayer.Tap(emulatorName, rnd.Next(22, 232), rnd.Next(473, 512));
                                }

                            }

                            
                            
                            if (point != null)
                            {
                                if (updateState(form, index, "Вышел со стола"))
                                {
                                    updateStateExceptPauseAndStop(form, index, "");
                                    updateStatusExceptPauseAndStop(form, index, "Остановлен");
                                    return;
                                }
                                
                                break;
                            }
                            else
                            {
                                if (updateState(form, index, "Вышел со стола"))
                                {
                                    updateStateExceptPauseAndStop(form, index, "");
                                    updateStatusExceptPauseAndStop(form, index, "Остановлен");
                                    return;
                                }
                                
                            }
                        }
                        catch (System.ArgumentException e)
                        {
                            point = null;
                        }
                        if(updateState(form, index, "Нажимаю выход со стола"))
                        {
                                updateStateExceptPauseAndStop(form, index, "");
                                updateStatusExceptPauseAndStop(form, index, "Остановлен");
                                return;
                        }
                        
                        Task.Delay(rnd.Next(500, 1000)).Wait();
                    }
                }
                
                

                Task.Delay(rnd.Next(1000, 1000)).Wait();
                if(updateState(form, index, "Нажимаю выход на стартовый экран"))
                {
                    updateStateExceptPauseAndStop(form, index, "");
                    updateStatusExceptPauseAndStop(form, index, "Остановлен");
                    return;
                }
                point = null;
                counter = 0;
                while (point == null)
                {
                    counter++;
                    if(counter > 5)
                    {
                        updateStateExceptPauseAndStop(form, index, "Не смог выйти на главный экран");
                        updateStatusExceptPauseAndStop(form, index, "Остановлен");
                        return;
                    }
                    try
                    {
                        Point p1 = new Point(4, 5);
                        Point p2 = new Point(69, 53);
                        point = LDPlayer.FindImageInSubSpace(emulatorName, PicturesPaths.ExitToMainWindow, p1, p2, 1);
                    }
                    catch (System.ArgumentException e)
                    {
                        point = null;
                    }
                    if (point != null)
                    {
                        

                        if(updateState(form, index, "Вышел на стартовый экран"))
                        {
                            updateStateExceptPauseAndStop(form, index, "");
                            updateStatusExceptPauseAndStop(form, index, "Остановлен");
                            return;
                        }
                        LDPlayer.Tap(emulatorName, rnd.Next(4,69), rnd.Next(5,53));
                        break;
                    }
                    else
                    {
                        if(updateState(form, index, "Нажимаю выход на стартовый экран"))
                        {
                            updateStateExceptPauseAndStop(form, index, "");
                            updateStatusExceptPauseAndStop(form, index, "Остановлен");
                            return;
                        }
                    }
                    Task.Delay(rnd.Next(500, 1000)).Wait();
                }




            }





            if(updateStatus(form, index, "Выполнено"))
            {
                updateStateExceptPauseAndStop(form, index, "");
                updateStatusExceptPauseAndStop(form, index, "Остановлен");
                return;
            }
            if (isInClub)
            {
                if (hasTables)
                {
                    if (updateState(form, index, ""))
                    {
                        updateStateExceptPauseAndStop(form, index, "");
                        updateStatusExceptPauseAndStop(form, index, "Остановлен");
                        return;
                    }
                }
                else
                {
                    if (updateState(form, index, "Столов не было"))
                    {
                        updateStateExceptPauseAndStop(form, index, "");
                        updateStatusExceptPauseAndStop(form, index, "Остановлен");
                        return;
                    }

                }
            }
            else
            {
                if (updateState(form, index, "Не было клуба"))
                {
                    updateStateExceptPauseAndStop(form, index, "");
                    updateStatusExceptPauseAndStop(form, index, "Остановлен");
                    return;
                }
            }
            return;
        }
        
        
        
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        
    }
    public static class PicturesPaths
    {
        public static string SocksDroid = "Resources\\SocksDroid.bmp";//
        public static string SocksDroidEnable = "Resources\\SocksDroidEnable.bmp";//
        public static string SocksDroidEnabled = "Resources\\SocksDroidEnabled.bmp";//
        public static string SocksDroidOpened = "Resources\\SocksDroidOpened.bmp";//
        //public static string OKPatch = "Resources\\OKpatch.bmp";
        public static string ButtonEnterToClub = "Resources\\ButtonEnterToClub.bmp";//
        public static string PokerOpened = "Resources\\PokerOpened.bmp";//
        public static string PokerOpened2 = "Resources\\PokerOpened2.bmp";//
        public static string PokerOpened3 = "Resources\\PokerOpened3.bmp";
        public static string PokerOpenedEn = "Resources\\PokerOpenedEn.bmp";//
        public static string ClickToClub = "Resources\\ClickToClub.bmp";
        public static string PokerTable = "Resources\\PokerTable.bmp";//
        public static string TableMenu = "Resources\\TableMenu.bmp";//
        public static string TableOptions = "Resources\\TableOptions.bmp";//

        public static string TableOptionsEn = "Resources\\TableOptionsEn.bmp";//
        public static string SettingsHeader = "Resources\\SettingsHeader.bmp";//
        public static string CloseSettings = "Resources\\CloseSettings.bmp";//
        public static string SettingIsOn = "Resources\\SettingIsOn.bmp";//
        public static string ExitTable = "Resources\\ExitTable.bmp";//

        public static string ExitTableEn = "Resources\\ExitTableEn.bmp";//
        public static string ExitToMainWindow = "Resources\\ExitToMainWindow.bmp";//
        public static string EnterToPokerButton = "Resources\\EnterToPokerButton.bmp";//
        public static string POPUP2 = "Resources\\POPUP2.bmp";
        public static string POPUP3 = "Resources\\POPUP3.bmp";
        public static string POPUP4 = "Resources\\POPUP4.bmp";
        public static string POPUP5 = "Resources\\POPUP5.bmp";
        public static string POPUP6 = "Resources\\POPUP6.bmp";
        public static string POPUP7 = "Resources\\POPUP7.bmp";
        
    }

}


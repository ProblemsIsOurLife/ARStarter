using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ARStarter.Properties;

namespace ARStarter
{
    
    public partial class Form1 : Form
    {
        public  bool IsPaused = false;
        public  bool Pause = false;
        public  bool Stop = false;
        public static string PathLd = @"C:\LDPlayer\LDPlayer4.0\ldconsole.exe";
        private List<String> LDDevices = null;
        public Form1()
        {
            
            InitializeComponent();
            string advices = "1. Убедитесь что включен ADB\n2. Выключить bypass mode\n3. SocsDroid AppList - com.lein.pppoker.android\n4. Имя эмуля должно быть - pidXXXXXX";
            label2.Text = advices;
            /*foreach (var process in Process.GetProcessesByName("adb"))
            {
                process.Kill();
            }*/
            
            OpenFileDialog openLDconsole = new OpenFileDialog();
            openLDconsole.Filter = "LDconsole file | ldconsole.exe";
            DialogResult dialog = DialogResult.None;
            bool toExit = false;
            while(dialog != DialogResult.OK)
            {
                dialog = openLDconsole.ShowDialog();
                if(dialog == DialogResult.Cancel || dialog == DialogResult.Abort || dialog == DialogResult.None)
                {
                    toExit = true;
                    break;
                    
                }
            }
            if (toExit)
            {
                Application.Exit();
                this.Dispose();
                return;
            }
            //this.TopMost = true;
            this.Activate();
            PathLd = openLDconsole.FileName;

            Auto_LDPlayer.LDPlayer._PathLd = PathLd;
            

            initializeLDData();

        }
        private void initializeLDData()
        {

            Task<List<string>> task = Task.Run(() => Auto_LDPlayer.LDPlayer.GetDevices());
            
            LDDevices = task.Result;
            task.Dispose();
            LDDevicesList.CheckBoxes = true;
            LDDevicesList.Columns.Add("названия LDPlayer");
            LDDevicesList.Columns.Add("Текущее действие");
            LDDevicesList.Columns.Add("Статус");
            LDDevicesList.Columns[0].Width = 160;
            LDDevicesList.Columns[1].Width = 320;
            LDDevicesList.Columns[2].Width = 160;
            TextToFindBox.Height = 53;
            LDDevicesList.Font = new Font("Segoe UI", 13, FontStyle.Regular);
            TextToFindBox.Font = new Font("Segoe UI", 13, FontStyle.Regular);
            
            int index = 0;
            string[] row = { "", "Не запускался" };
            //ImageList imageListSmall = new ImageList();
            //TODO: подумать как врутить иконки состояния через imageList
            int firstDuplicateID = -1;
            string[] allLDs = Auto_LDPlayer.LDPlayer.GetDevices2Running();
            LDDevicesList.BeginUpdate();

            var hashSetDuplicates = new HashSet<string>();
            var hashSetLDs = new HashSet<string>();
            string LDID = "";
            foreach (string item in LDDevices)
            {
                if (!hashSetDuplicates.Add(item))
                {
                    foreach(string LD in allLDs)
                    {
                        if (!hashSetLDs.Add(LD.Split(',')[1]))
                        {
                            if (hashSetLDs.Contains(item))
                            {
                                LDID = LD.Split(',')[0];
                                allLDs = allLDs.Where(w => w != LD).ToArray();
                                hashSetLDs.Remove(item);
                                break;

                            }
                           
                        }
                    }
                    row[1] = item + " ++ ID : " + LDID;
                    LDDevicesList.Items.Add(item).SubItems.AddRange(row);
                }
                else
                {
                    row[1] = "Не запускался";
                    LDDevicesList.Items.Add(item).SubItems.AddRange(row);
                }
            }
            LDDevicesList.EndUpdate();
            List<string> duplicates = new List<string>();
            List<string> ldlist = new List<string>();
            
            bool isDuplicate = false;
            /*LDDevicesList.BeginUpdate();
            foreach (ListViewItem item in LDDevicesList.Items)
            {
                string name = item.Text;
                if (item.Index != LDDevicesList.Items.Count - 1)
                {
                    if (!duplicates.Contains(name))
                    {
                        string LDID = "";
                        for (int i = item.Index + 1; i < LDDevicesList.Items.Count; i++)
                        {
                            if (LDDevicesList.Items[i].Text == name)
                            {
                                isDuplicate = true;
                                if (!duplicates.Contains(name))
                                {
                                    duplicates.Add(name);
                                }

                                foreach (string str in allLDs)
                                {
                                    if (str.Split(',')[1] == name)
                                    {
                                        LDID = str.Split(',')[0];
                                        allLDs = allLDs.Where(w => w != str).ToArray();
                                        break;
                                    }
                                }
                                row[1] = name + " ++ ID : " + LDID;
                                LDDevicesList.Items[i].SubItems[2].Text = row[1];

                            }

                        }
                        if (isDuplicate)
                        {
                            foreach (string str in allLDs)
                            {
                                if (str.Split(',')[1] == name)
                                {
                                    LDID = str.Split(',')[0];
                                    allLDs = allLDs.Where(w => w != str).ToArray();
                                    break;
                                }
                            }
                            LDDevicesList.Items[item.Index].SubItems[2].Text = row[1];
                            isDuplicate = false;
                        }


                    }
                }


            }
            LDDevicesList.EndUpdate();*/
        }





        private CancellationTokenSource cancelTS = new CancellationTokenSource();
        private void StartButton_Click(object sender, EventArgs e)
        {
            if(LDDevicesList.CheckedItems.Count != 0)
            {
                if(taskList.Count == 0)
                {
                    PauseButton.Enabled = true;
                    StopButton.Enabled = true;
                    Stop = false;
                    AddTasks();
                    //Task.Run(new Action(RunTask));
                    Task task = new Task(() => RunTask(), TaskCreationOptions.LongRunning);
                    task.Start();
                    LDDevicesList.SelectedItems.Clear();
                    foreach (ListViewItem item in LDDevicesList.CheckedItems)
                    {
                        item.Checked = false;
                    }
                }
                else
                {
                    AddTasks();
                    LDDevicesList.SelectedItems.Clear();
                    foreach (ListViewItem item in LDDevicesList.CheckedItems)
                    {
                        item.Checked = false;
                    }
                }
            }
            
        }
        public List<int> taskListIDs = new List<int>();
        public List<Task> taskList = new List<Task>();
        public void RunTask()
        {
            while (taskList.Count > 0)
            {
                Stop = false;
                //Action act = taskList[0];
                Task.Delay(1000).Wait();
                taskList[0].Start();
                taskList[0].Wait();
                taskList[0].Dispose();
                /*Task task = new Task(act);
                task.Start();
                task.Wait();
                task.Dispose();*/
                //Task.Run(act).Wait();
                //task.Wait();
                taskListIDs.RemoveAt(0);
                taskList.RemoveAt(0);
            }
            Auto_LDPlayer.LDPlayer.SortWnd();
        }
        public void AddTasks()
        {
            foreach (ListViewItem selectedLDDevice in LDDevicesList.CheckedItems)
            {
                if(selectedLDDevice.SubItems[2].Text == "Остановлен" || selectedLDDevice.SubItems[2].Text == "Не запускался" || selectedLDDevice.SubItems[2].Text == "Выполнено" || selectedLDDevice.SubItems[2].Text == "Завершено")
                {
                    int index = (int)selectedLDDevice.Index;

                    LDDevicesList.Items[index].SubItems[2].Text = "Ожидает запуск";
                    
                    Task working = new Task(() => EmulatorController.workingActions(this, index, selectedLDDevice.Text.ToString(), cancelTS.Token), cancelTS.Token);
                    //taskList.Add(() => EmulatorController.workingActions(this, index, selectedLDDevice.Text.ToString(), cancelTS.Token));
                    taskListIDs.Add(index);
                    taskList.Add(working);
                }
                
            }

        }
        

        /*public static List<string> GetDevices()
        {
            var arr = Auto_LDPlayer.LDPlayer.ExecuteLdForResult("list").Trim().Split('\n');
            for (var i = 0; i < arr.Length; i++)
            {
                if (arr[i] == "")
                    return new List<string>();
                arr[i] = arr[i].Trim();
            }
            //List<string> strs = "pid7716594,pid7985424,pid7985276,pid8031953,pid7985216,pid7985244,pid7993594,pid7916926,pid7985063,pid7918163,pid8017653,pid7993621,pid7989684,pid7984686,pid7997439,pid7998123,pid7989109,pid8000964,pid7993449,pid7993285,pid8009638,pid8021422,pid7912999,pid7989586,pid7993099,pid7989019,pid7993864,pid7985375,pid7993320,pid7863263,pid7873358,pid7710259,pid8030157,pid7998203,pid7847548,pp7854188,pid7854579,pid7984930,pid7985276,pid8010148,pid8034996,LDPlayer-41,pid7989273,pid7912721,pid7911917,pid7984883,pid7984760,pid7922701,pid8021131,pid8021453,pid8021271,pid8021241,pid8021212,pid8021191,pid8021160,pid8004929,pid7744578,pid8005079,pid7239851,pid8021070,pid7630687,pid7630782,pid8021048,pid7741174,pid8021004,pid8029954,11,12,13,14,15,15DKM,pid7850647,pid8030065,77luckyHand,pid8030090,7553827 BOSS RU,7553959 astana boss,7553959 BOSS KZ,pid8030108,7710438,pid8030123,Abele,pid8030157,pid7728251,AmeRiKann,pid8030228,pid8034070,archigabriil7,pid8032094,pid8032121,pid8032135,Barmley,pid7732904,BelarusRubber,BL0NDEEE,pid7714066,Boss Aqvaryum baligi Турция,Boss Millioner,Bulletproof515,pid7581648,pid7605865,pid7748052,pid7749813,CerryUkal,Chalkerto,pid7998018,pid7744453,pid8018181,pid8005298,pid7989109,Crispbreadooo,pid7993864,pid7615627,pid7781508,pid8005249,DaveMasterson,pid8030448,Dedillsoryst,Destr0eR,pid8030466,Discotato,pid7752739,pid8030490,pid8030600,pid7713668,EdisonMind,pid7534710,pid6750328,pid8030643,EskanorPride,pid7164524,pid8030867,pid7724074,pid8030922,pid7724166,pid7728343,pid8030995,pid7746032,17653,pid7921449,pid8039576,pid8032006,pid7741314,Game changerr,GAspar1p,GastonRed,pid8032080,pid7786505,pid8036925,pid8032296,GoodShotss,pid610717,pid8034251,pid8034331,pid8034365,pid8034459,Hellessence,pid8034497,pid8034896,pid7710534,pid8034926,pid600958,IranildoGordo,Jesulousfv,pid7748307,pid8034954,pid8034996,Juicel963,pid8035060,pid8035095,pid7762381,pid8035120,pid8035152,pid7672788,pid7713204,pid7894424,pid7745510,pid6750338,KxHWJiw7,pid7753320,pid7576275,pid8035169,pid598903,pid7912383,LeprikonGXZBoss,pid7710354,LIFECULA1583,pid7739859,pid7672693,pid7728523,pid7609111,pid7733917,pid7609033,MAGIC97,pid7860574,pid8018472,Marv1n45,pid8031984,pid7672882,MelakanitBoss,pid8032038,pid7615278,pid8039595,pid7150851,pid7241767,pid7863465,pid7864117,pid7733993,pid7917157,pid7672550,pid7241388,pid7608744,pid7608592,pid7842794,pid7712025,PicPaguei,pid7061618,pid7135500,pid7303355,pid7601478,pid7715941,pid7731879,pid7731960,pid7752693,pid7752902,pid7753577,pid7753679,pid7758961,pid7759186,pid7759303,pid7759426,pid7759504,pid7759668,pid7759822,pid7759974,pid7760101,pid7760191,pid7760361,pid7762058,pid7768391,pid7768525,pid7768761,pid7769024,pid7771053,pid7771151,pid7771320,pid7771906,pid7781227,pid7872844,pid7783506,pid7785629,pid7787529,pid7790456,pid7790725,pid7803399,pid7803471,pid7803521,pid7807255,pid7807827,pid7807927,pid7807996,pid7808106,pid7808192,pid7808288,pid7808382,pid7811198,pid7811317,pid7811384,pid7811456,pid7811594,pid7811657,pid7811714,pid7811789,pid7811865,pid7811936,pid7812032,pid7812164,pid7812235,pid7812314,pid7817550,pid7817663,pid7817751,pid7817876,pid7818006,pid7818232,pid7818296,pid7820148,pid7820230,pid7820300,pid7820441,pid7820507,pid7820575,pid7821839,pid7821964,pid7822246,pid7822414,pid7881745,pid7894165,pid7894276,pid7894358,pid7894642,pid7894761,pid7894895,pid7895327,pid7895443,pid7895633,pid7898545,pid7898737,pid7903315,pid7903412,pid7903915,pid7904259,pid7904536,pid7906706,pid7906794,pid7906857,pid7907469,pid7917541,pid7917637,pp7712297,pid7917672,pid7917962,pid7918020,pid7918325,pid7921561,pid7921593,pid7921635,pid7921697,pid7921755,pid7921811,pid7921859,pid7922106,pid7922193,pid7922254,pid7922317,pid7930461,pid7930462,pid7930486,pp577ZZuj,pp6773415,pp7239680,pp7239851,pp7240549,pp7515696 boss,pp7578785 LuckyBet agent,pp7710139,pp7710438,pp7712348,pp7712413,pp7712476,pp7713109,pp7713220,pp7715941,pp7716678,pp7717541,pp7719656,pp7724339,pp7728399,pp7728731,pp7732795,pp7733209,pp7733292,pp7733796,pp7735832,pp7736148,pp7740240,pp7740980,pp7741314,pp7741570,pp7741668,pp7741875,pp7742144,pp7743568,pp7743640,pp7743699,pp7743751,pp7744147,pp7744221,pid7771693,pp7744379,Choco8un,pp7744523,pp7744578,pp7744661,pp7744749,pp7744832,pp7745008,pp7745157,pp7745266,pp7748477,pp7748634,pp7749291,pp7818296,pp77343813,pid7723927,pid7723795,RainDroppp,Rapier_22,pid7883050,pid7717145,pid7785467,pid7812235,pid7741875,pid7762303,pid7672457,pid7729085,pid599372,pid7576177,pid7883171,pid7582315,pid7785869,pid7872258,pid599138,supreme sheloski,Sychoke_7av5,pid7850213,pid7786072,pid7758438,Tigz_Gangg,pid7781785,pid7576364,pid7765851,8031984,pid7716768,Victa12,pid7771408,pid7744221,pid7781364,pid7765767,pid7876414,pid7842209,yukoSun,ZeroZone,pid7717363,pid7242280,pid7576498,pid7581934,pid7582160,pid7615892,pid7239680,pid7872258,ЕбаТеррор,pid7790528,pid7617283,pid7242140,pid7716297,pid7240549,pid7630987,pid7615732,pid7615391,Хитрый3000,pid7615515,pid7534804,crazy mashine,pid7869281,pid7913145,pid7912598,pid7887179,pid7882544,pid7885799,pid7882921,pid7913079,pid7917920,pid7868844,pid7922677,pid7911630,pid7872739,pid7872637,pid7882209,pid7916968,pid7887251,pid7913580,pid7916845,pid7912006,pid7922611,pid7847548,pid7917011,pid7912875,pid7913824,pid7913400,pid7913324,pid7917104,pid7912076,pid7925610,pid7882014,pid7854579,pid7847373,pid7989198,pid7989543,pid7985334".Split(',').ToList();
            //System.Windows.Forms.MessageBox.Show(string.Join("|", arr));
            //return strs;
            return arr.ToList();
        }*/

        private void pauseButton_Click(object sender, EventArgs e)
        {
            if (IsPaused)
            {
                PauseButton.Image = Resources.PauseButton;
                Pause = false;
                PauseButton.Text = "Пауза";
                IsPaused = false;
            }
            else
            {
                PauseButton.Image = Resources.ResumeButton;
                Pause = true;
                IsPaused = true;
                PauseButton.Text = "Возобновить";
            }
        }

        private void updateLDList_Click(object sender, EventArgs e)
        {
            string[] row = { "", "Не запускался" };
             /*Если нужен более быстрый обновлятор, использовать этот. Но он полностью сбрасывает список*/
             LDDevicesList.BeginUpdate();
             LDDevicesList.Items.Clear();
            Task<List<string>> task = Task.Run(() => Auto_LDPlayer.LDPlayer.GetDevices());

            LDDevices = task.Result;
            task.Dispose();
            string[] allLDs = Auto_LDPlayer.LDPlayer.GetDevices2Running();
            
            
            Dictionary<int, string> LDemulatorsID = new Dictionary<int, string>();
            foreach (string LD in allLDs)
            {
                LDemulatorsID.Add (int.Parse((LD.Split(',')[0])),LD.Split(',')[1]);
            }
            var hashSetDuplicates = new HashSet<string>();
            var hashSetLDs = new HashSet<string>();
            string LDID = "";
            foreach (string item in LDDevices)
            {
                if (!hashSetDuplicates.Add(item))
                {
                    LDID = LDemulatorsID.FirstOrDefault(x => x.Value == item).Key.ToString();
                    LDemulatorsID.Remove(int.Parse(LDID));
                    row[1] = item + " ++ ID : " + LDID;
                    LDDevicesList.Items.Add(item).SubItems.AddRange(row);
                }
                else
                {
                    row[1] = "Не запускался";
                    LDDevicesList.Items.Add(item).SubItems.AddRange(row);
                }
            }
            LDDevicesList.EndUpdate();
             
             
             
             
             
             
             



           /* 
            List<string> newLDDevices = GetDevices();
            List<string> AllDevices = new List<string>();
            foreach (ListViewItem name in LDDevicesList.Items)
            {
                AllDevices.Add(name.Text);
            }

            List<string> devicesToRemove = AllDevices.Except(newLDDevices).ToList();
            if (devicesToRemove != null && devicesToRemove.Count != 0 && AllDevices.Count != newLDDevices.Count)
            {
                foreach (string item in devicesToRemove)
                {

                    //imageListSmall.Images.Add(Bitmap.FromFile("Resources\\working.png"));
                    ListViewItem item1 = LDDevicesList.FindItemWithText(item, false, lastFindedIndex, false);
                    if (item1 != null && item1.Index != null)
                    {
                        LDDevicesList.Items[item1.Index].Remove();
                    }
                    //ListViewItem.ListViewSubItem subImage = Bitmap.FromFile("Resources\\working.png");
                }
            }

            List<string> devicesToAdd = newLDDevices.Except(AllDevices).ToList();
            if (devicesToAdd != null && devicesToAdd.Count != 0 && AllDevices.Count != newLDDevices.Count)
            {
                foreach (string item in devicesToAdd)
                {

                    //imageListSmall.Images.Add(Bitmap.FromFile("Resources\\working.png"));
                    LDDevicesList.Items.Add(item).SubItems.AddRange(row);
                    //ListViewItem.ListViewSubItem subImage = Bitmap.FromFile("Resources\\working.png");

                    LDDevicesList.Items[LDDevicesList.Items.Count - 1].ImageIndex = LDDevicesList.Items.Count - 1;




                }
            }

            AllDevices.Clear();
            foreach (ListViewItem name in LDDevicesList.Items)
            {
                AllDevices.Add(name.Text);
            }
            if (AllDevices.Count == newLDDevices.Count)
            {
                for (int i = 0; i < AllDevices.Count; i++)
                {
                    if (LDDevicesList.Items[i].Text != newLDDevices[i])
                    {
                        LDDevicesList.Items[i].Text = newLDDevices[i];
                        LDDevicesList.Items[i].SubItems[2].Text = "Не запускался";
                    }
                }
            }

            AllDevices.Clear();
            foreach (ListViewItem name in LDDevicesList.Items)
            {
                AllDevices.Add(name.Text);
            }
            string[] allLDs = Auto_LDPlayer.LDPlayer.GetDevices2Running();
            if (AllDevices.Count != newLDDevices.Count)
            {
                if (AllDevices.Count < newLDDevices.Count)
                {
                    int firstDuplicateID = -1;
                    var hashSet = new HashSet<string>();
                    int index = 0;
                    foreach (string item in newLDDevices)
                    {
                        if (!hashSet.Add(item))
                        {
                            string LDID = "";
                            foreach (string str in allLDs)
                            {
                                if (str.Split(',')[1] == item)
                                {
                                    LDID = str.Split(',')[0];
                                    allLDs = allLDs.Where(w => w != str).ToArray();
                                    break;
                                }
                            }
                            row[1] = item + " ++ ID : " + LDID;
                            LDDevicesList.Items.Add(item);
                            LDDevicesList.Items[LDDevicesList.Items.Count - 1].SubItems.AddRange(row);



                        }
                        index++;
                    }
                }
                else
                {

                    List<string> duplicates = new List<string>();
                    bool isDuplicate = false;
                    foreach (ListViewItem item in LDDevicesList.Items)
                    {
                        if (item.SubItems[2].Text != "Остановлен" && item.SubItems[2].Text != "Не запускался" && item.SubItems[2].Text != "Выполнено" && item.SubItems[2].Text != "Завершено")
                        {
                            item.SubItems[2].Text = "Не запускался";
                        }

                    }
                    foreach (ListViewItem item in LDDevicesList.Items)
                    {
                        string name = item.Text;
                        if (item.Index != LDDevicesList.Items.Count - 1)
                        {
                            if (!duplicates.Contains(name))
                            {
                                string LDID = "";
                                for (int i = item.Index + 1; i < LDDevicesList.Items.Count; i++)
                                {
                                    if (LDDevicesList.Items[i].Text == name)
                                    {
                                        isDuplicate = true;
                                        if (!duplicates.Contains(name))
                                        {
                                            duplicates.Add(name);
                                        }

                                        foreach (string str in allLDs)
                                        {
                                            if (str.Split(',')[1] == name)
                                            {
                                                LDID = str.Split(',')[0];
                                                allLDs = allLDs.Where(w => w != str).ToArray();
                                                break;
                                            }
                                        }
                                        row[1] = name + " ++ ID : " + LDID;
                                        LDDevicesList.Items[i].SubItems[2].Text = row[1];

                                    }

                                }
                                if (isDuplicate)
                                {
                                    foreach (string str in allLDs)
                                    {
                                        if (str.Split(',')[1] == name)
                                        {
                                            LDID = str.Split(',')[0];
                                            allLDs = allLDs.Where(w => w != str).ToArray();
                                            break;
                                        }
                                    }
                                    LDDevicesList.Items[item.Index].SubItems[2].Text = row[1];
                                    isDuplicate = false;
                                }
                                else
                                {
                                    item.SubItems[2].Text = "Не запускался";
                                }


                            }


                        }


                    }


                }
            }
            else
            {





                List<string> duplicates = new List<string>();
                List<string> ldlist = new List<string>();
                allLDs = Auto_LDPlayer.LDPlayer.GetDevices2Running();
                bool isDuplicate = false;
                foreach (ListViewItem item in LDDevicesList.Items)
                {
                    string name = item.Text;
                    if (item.Index != LDDevicesList.Items.Count - 1)
                    {
                        if (!duplicates.Contains(name))
                        {
                            string LDID = "";
                            for (int i = item.Index + 1; i < LDDevicesList.Items.Count; i++)
                            {
                                if (LDDevicesList.Items[i].Text == name)
                                {
                                    isDuplicate = true;
                                    if (!duplicates.Contains(name))
                                    {
                                        duplicates.Add(name);
                                    }

                                    foreach (string str in allLDs)
                                    {
                                        if (str.Split(',')[1] == name)
                                        {
                                            LDID = str.Split(',')[0];
                                            allLDs = allLDs.Where(w => w != str).ToArray();
                                            break;
                                        }
                                    }
                                    row[1] = name + " ++ ID : " + LDID;
                                    LDDevicesList.Items[i].SubItems[2].Text = row[1];

                                }

                            }
                            if (isDuplicate)
                            {
                                foreach (string str in allLDs)
                                {
                                    if (str.Split(',')[1] == name)
                                    {
                                        LDID = str.Split(',')[0];
                                        allLDs = allLDs.Where(w => w != str).ToArray();
                                        break;
                                    }
                                }
                                LDDevicesList.Items[item.Index].SubItems[2].Text = row[1];
                                isDuplicate = false;
                            }
                            else
                            {
                                item.SubItems[2].Text = "Не запускался";
                            }


                        }


                    }


                }
            }*/
        }
        private int lastFindedIndex = 0;
        private string enteredTextToFind = "";
        private void findItemInLDList_Click(object sender, EventArgs e)
        {
            if (enteredTextToFind != TextToFindBox.Text)
            {
                enteredTextToFind = TextToFindBox.Text;
                lastFindedIndex = 0;
            }
            if (lastFindedIndex < LDDevicesList.Items.Count)
            {
               //ListViewItem item1 = LDDevicesList.FindItemWithText(enteredTextToFind, true, lastFindedIndex,true);
                for (int i = lastFindedIndex; i < LDDevicesList.Items.Count; i++)
                {
                    if (LDDevicesList.Items[i].Text.ToUpper().Contains(enteredTextToFind.ToUpper()))
                    {
                        LDDevicesList.SelectedItems.Clear();

                        LDDevicesList.Items[i].Selected = true;
                        LDDevicesList.Items[i].Focused = true;
                        LDDevicesList.EnsureVisible(i);
                        lastFindedIndex = i + 1;
                        break;
                    }
                    
                }
                

            }
           if(lastFindedIndex>= LDDevicesList.Items.Count)
            {
                lastFindedIndex = 0;
            }
            
            
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            Stop = true;
        }

        private void deleteFromWorkListButton_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem selectedLDDevice in LDDevicesList.CheckedItems)
            {
                int index = (int)selectedLDDevice.Index;
                if(LDDevicesList.Items[index].SubItems[2].Text == "Ожидает запуск")
                {
                    for(int i = 0; i < taskListIDs.Count; i++)
                    {
                        if(taskListIDs[i] == index)
                        {
                            LDDevicesList.Items[index].SubItems[2].Text = "Не запускался";
                            taskListIDs.RemoveAt(i);
                            taskList.RemoveAt(i);
                        }
                    }
                }
                if (LDDevicesList.Items[index].SubItems[2].Text == "Выполняется")
                {
                    LDDevicesList.Items[index].SubItems[2].Text = "Не запускался";
                    Stop = true;

                }

            }
        }

        private void stopAllButton_Click(object sender, EventArgs e)
        {
            PauseButton.Enabled = false;
            StopButton.Enabled = false;
            Stop = true;
            int count = taskListIDs.Count;
            for (int i =0; i < count; i++)
            {
                LDDevicesList.Items[taskListIDs[0]].Font = new Font("Segoe UI", 13, FontStyle.Regular);
                LDDevicesList.Items[taskListIDs[0]].SubItems[1].Text = "";
                LDDevicesList.Items[taskListIDs[0]].SubItems[2].Text = "Остановлен";
                
                taskListIDs.RemoveAt(0);
                taskList.RemoveAt(0);
            }
            
        }
       


        private void TextToFindBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                findItemInLDList_Click(sender, e);


            }
        }

        private void DisableAllCheckboxesButton_Click(object sender, EventArgs e)
        {
            if (LDDevicesList.CheckedItems.Count != 0)
            {
                foreach (ListViewItem item in LDDevicesList.CheckedItems)
                {
                    item.Checked = false;
                }
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            var toCheck = new LDPlayersToCheck();
            toCheck.ShowDialog();
            List<string> returned = toCheck.dataToReturn;
            int count = 0;
            if (returned.Count != 0)
            {
                foreach(string str in returned)
                {
                    for (int i = 0; i < LDDevicesList.Items.Count; i++)
                    {
                        if (LDDevicesList.Items[i].Text.ToUpper().Contains(str.ToUpper()))
                        {
                            count++;
                            LDDevicesList.Items[i].Selected = true;
                            LDDevicesList.Items[i].Checked = true;
                            break;
                        }
                    }
                }
                label3.Text = "Добавлено аккаунтов: " + count.ToString();
                label3.Visible = true;
            }
        }

        private void KillADB_Click(object sender, EventArgs e)
        {
            foreach (var process in Process.GetProcessesByName("adb"))
            {
                process.Kill();
            }
        }
    }
}

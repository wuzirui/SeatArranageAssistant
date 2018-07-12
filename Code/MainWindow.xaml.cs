using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace SeatArrangeAssistant
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public Base initBase = new Base();
        public static Student[] Students = new Student[50];
        public int n;
        public Layout lout;
        public seat[,] Room;
        public Strategy[] Strategies = new Strategy[50];
        public static int StrategyNum;

        private System.Windows.Threading.DispatcherTimer dtimer;
        private const int initWid = 10, initHei = 10, gapWid = 10, gapHei = 10;
        private const int stuWid = 98, stuHei = 38;

        public static Student GetStudent(int number)
        {
            Student temp = Students[number];
            if (temp != null && temp.Number == number) return temp;
            foreach (Student student in Students)
            {
                if (student != null && student.Number == number) return student;
            }
            throw new Exception("Student not found");
        }

        public MainWindow()
        {
            InitializeComponent();
            lout = initBase.LoadSettings();
            n = initBase.Read(Students, lout, Strategies);
            tick = DateTime.Now.Ticks;
            rnd = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));
            //MessageBoxResult result =
            //    MessageBox.Show("是否使用上次使用的布局设置？", "SAA", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (true)
            {
                Setup();
                init.IsEnabled = false;
            }
        }

        private long tick;
        private Random rnd;
        public seat Locate(Student student)
        {
            for (int i = 0; i < lout.Rows; i++)
            {
                for (int j = 0; j < lout.Colums; j++)
                {
                    if (Room[i, j].RelateStudent == student)
                        return Room[i, j];
                }
            }
            return null;
        }


        public Location RandomPick()
        {
            int tarCol, tarRow;
            tarRow = rnd.Next(0, lout.Rows - 1);
            tarCol = rnd.Next(0, lout.Colums - 1);
            if (lout.Filling[tarRow + 1, tarCol + 1] == false) return RandomPick();
            if (Room[tarRow, tarCol].IsNull == false && Room[tarRow, tarCol].isLocked == false)
            {
                Room[tarRow, tarCol].RelateStudent.SwapCounter++;
                Room[tarRow,tarCol].CheckLevel(Room[tarRow, tarCol].RelateStudent.SwapCounter);
                return Room[tarRow, tarCol].RelateStudent.Pos;
            }

            return RandomPick();
        }

        public bool LegalCheck(int row, int col)
        {
            if (row < 0 || row >= lout.Rows - 1 || col < 0 || col >= lout.Colums - 1) return false;
            return !Room[row, col].IsNull;
        }

        public void Force(seat A, seat B)
        {
            int[] arrange = new[] {0, 1, 2, 3, 4, 5, 6, 7, 8};
            int[] posRow = new[] {0, -1, -1, -1, 0, 0, 1, 1, 1};
            int[] posCol = new[] {0, 1, 0, -1, 1, -1, 1, 0, -1};
            for (int i = 1; i <= 8; i++)
            {
                int tar = rnd.Next(1, 8);
                int temp = arrange[i];
                arrange[i] = arrange[tar];
                arrange[tar] = temp;
            }
            for (int i = 1; i <= 8; i++)
            {
                if (LegalCheck(A.Row + posRow[i], A.Col + posCol[i]))
                {
                    Room[A.Row + posRow[i], A.Col + posCol[i]].swap(B);
                }
            }
        }

        public void SingleGenerate(int row, int col)
        {
            if (Room[row, col].IsNull) return;
            if (lout.Filling[row + 1, col + 1] == false) return;
            Location pos = RandomPick();
            Room[pos.RowX - 1, pos.ColY - 1].CheckLevel(++Room[pos.RowX - 1, pos.ColY - 1].RelateStudent.SwapCounter);
            Room[row, col].CheckLevel(++Room[row, col].RelateStudent.SwapCounter);

            Room[row, col].swap(Room[pos.RowX - 1, pos.ColY - 1], false);
            Room[row, col].RelateStudent.SetPosition(row + 1, col + 1);
            Room[pos.RowX - 1, pos.ColY - 1].RelateStudent.Pos = pos;
            Room[pos.RowX - 1, pos.ColY - 1].CancelHighLight();
        }

        private int genI = 0, genJ = 0;
        void dtimer_Tick(object sender, EventArgs e)
        {
            SingleGenerate(genI, genJ);

            genJ++;
            if (genJ >= lout.Colums)
            {
                genJ = 0;
                genI++;
            }
            if (genI >= lout.Rows)
            {
                genI = 0;
                genJ = 0;
                dtimer.Stop();
                fixStrategy();
                return;
            }
            Room[genI, genJ].Highlight();
        }

        public void GenerateAll()
        {
            for (int i = 0; i < lout.Rows; i++)
            {
                for (int j = 0; j < lout.Colums; j++)
                {
                    if (lout.Filling[i + 1, j + 1] == false) continue;
                    if (Room[i, j].IsNull) continue;
                    Location pos = RandomPick();
                    Room[pos.RowX - 1, pos.ColY - 1].CheckLevel(++Room[pos.RowX - 1, pos.ColY - 1].RelateStudent.SwapCounter);
                    Room[i, j].CheckLevel(++Room[i, j].RelateStudent.SwapCounter);

                    Room[i, j].swap(Room[pos.RowX - 1, pos.ColY - 1], false);
                    Room[i, j].RelateStudent.SetPosition(i + 1, j + 1);
                    Room[pos.RowX - 1, pos.ColY - 1].RelateStudent.Pos = pos;

                }
            }
        }

        public void fixStrategy()
        {
            Console.Write("\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n");
            for (int i = 1; i <= StrategyNum; i++)
            {
                if (Strategies[i].GetType() == typeof(DistanceStrategy))
                {
                    DistanceStrategy strategy = (DistanceStrategy)Strategies[i];
                    int result = rnd.Next(0, 99);
                    if (result > strategy.Probability) continue;
                    seat A = Locate(strategy.StudentA), B = Locate(strategy.StudentB);

                    Console.WriteLine(A.RelateStudent.Name + " " + B.RelateStudent.Name);

                    int tryout = 0;
                    while (!strategy.Varify())
                    {
                        if (tryout > 40)
                        {
                            if (strategy.Pattern == DistanceStrategy.Seperate) continue;
                            Force(A, B);
                            continue;
                        }
                        tryout++;
                        Location pos = RandomPick();
                        seat temp = Room[pos.RowX - 1, pos.ColY - 1];

                        A.swap(temp);

                        A = temp;
                        
                    }
                    A.Lock();
                }
            }
            for (int i = 0; i < lout.Rows; i++)
            {
                for (int j = 0; j < lout.Colums; j++)
                {
                    Room[i, j].Unlock();
                }
            }
        }

        public void Generate(bool needEffet)
        {
            long tick = DateTime.Now.Ticks;
            Random rnd = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));
            for (int i = 0; i < lout.Rows; i++)
            {
                for (int j = 0; j < lout.Colums; j++)
                {
                    if (Room[i, j].RelateStudent != null)
                    {
                        Room[i, j].RelateStudent.SwapCounter = 0;
                        Room[i, j].CheckLevel(Room[i, j].RelateStudent.SwapCounter);
                    }
                    if (lout.Filling[i + 1, j + 1] == false) 
                        Room[i, j].Lock();
                }
            }
            if (needEffet)
            {
                if (dtimer == null)
                {
                    dtimer = new System.Windows.Threading.DispatcherTimer();
                    dtimer.Tick += dtimer_Tick;
                }
                dtimer.Interval = TimeSpan.FromMilliseconds(Speed.Value);
                dtimer.Start();
            }
            else
            {
                GenerateAll();
                fixStrategy();
            }
            
            
        }

        private void Button_Click(object sender, RoutedEventArgs e) //Generate
        {
            Generate(effect.IsChecked.Value);
        }

        public void SaveFrameworkElementToImage(FrameworkElement ui, string filename)
        {
            System.IO.FileStream ms = new System.IO.FileStream(filename, System.IO.FileMode.Create);
            System.Windows.Media.Imaging.RenderTargetBitmap bmp = new System.Windows.Media.Imaging.RenderTargetBitmap((int)ui.ActualWidth, (int)ui.ActualHeight, 96d, 96d, System.Windows.Media.PixelFormats.Pbgra32);
            bmp.Render(ui);
            System.Windows.Media.Imaging.JpegBitmapEncoder encoder = new System.Windows.Media.Imaging.JpegBitmapEncoder();
            encoder.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(bmp));
            encoder.Save(ms);
            ms.Close();

            //File.Copy(filename, filename, true);
           
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFrameworkElementToImage(Board, "arrange.png");
                MessageBox.Show("Image Saved");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
            
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            dialog.Filter = "PNG文件|.png";
            string path = "";
            if (dialog.ShowDialog() == true)
                path = dialog.FileName;
            else
            {
                 return;
            }
            try
            {
                SaveFrameworkElementToImage(Board, path);
                MessageBox.Show("Image Saved");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Warning");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e) //swap places
        {
            if (seat.NumChoosen == 2)
            {
                seat.A.swap(seat.B);
                seat.A.Shut();
                seat.B.Shut();
            }
        }

        public int GetAbs(int RowX, int ColY)
        {
            return (RowX - 1) * lout.Colums + ColY;
        }

        private seat WrongPlace()
        {
            for (int i = 0; i < lout.Rows; i++)
            {
                for (int j = 0; j < lout.Colums; j++)
                {
                    if (Room[i, j].IsEnabled == false && Room[i, j].RelateStudent != null)
                        return Room[i, j];
                }
            }
            return null;
        }

        private void Setup()
        {
            Room = new seat[lout.Rows, lout.Colums];
            for (int i = 0; i < lout.Rows; i++)
            {
                for (int j = 0; j < lout.Colums; j++)
                {
                    Room[i, j] = new seat
                    {
                        Height = stuHei,
                        Width = stuWid,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top,
                        Row = i + 1,
                        Col = j + 1
                    };

                    int temp = GetAbs(i + 1, j + 1);
                    
                    
                    Room[i, j].Setup(Students[temp]);
                    if (Students[temp] != null)
                        Students[temp].SetPosition(i + 1, j + 1);
                    
                    if (!lout.Filling[i + 1, j + 1]) //如果当前位置应为空，则将位置标记，当有正常位置为空时调换
                    {
                        Room[i, j].IsEnabled = false;
                    }
                    else if (Students[temp] == null)
                    {
                        seat tempSeat = WrongPlace();
                        tempSeat.swap(Room[i, j], false);
                        Room[i, j].RelateStudent.SetPosition(i + 1, j + 1);

                    }

                    if (i == 0 && j == 0)
                    {
                        Room[i, j].Margin = new Thickness(initWid, initHei, 0, 0);
                    }
                    else
                    {
                        if (i == 0)
                        {
                            Room[i, j].Margin =
                                new Thickness(Room[i, j - 1].Margin.Left + Room[i, j - 1].body.Width + gapWid, initHei,
                                    0,
                                    0);
                        }
                        else if (j == 0)
                        {
                            Room[i, j].Margin = new Thickness(initWid,
                                Room[i - 1, j].Margin.Top + Room[i - 1, j].body.Height + gapHei, 0, 0);
                        }
                        else
                        {
                            Room[i, j].Margin =
                                new Thickness(Room[0, j - 1].Margin.Left + Room[0, j - 1].body.Width + gapWid,
                                    Room[i, 0].Margin.Top, 0, 0);
                        }

                    }

                    Board.Children.Add(Room[i, j]);
                    init.IsEnabled = false;
                    
                }
                
            }
        }

        private void init_Click(object sender, RoutedEventArgs e)
        {
            if (RowBox.Text != "" && ColBox.Text != "")
            {
                lout.Set(Convert.ToInt32(RowBox.Text), Convert.ToInt32(ColBox.Text));
                Setup();
            }
        }
    }
}
